//====================================================================================================================================================
// Copyright 2023 Lake Orion Robotics FIRST Team 302
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.
//====================================================================================================================================================

#include <cmath>

#include <frc/geometry/Pose2d.h>

// Team302 Includes
#include <chassis/swerve/driveStates/TrajectoryDrivePathPlanner.h>
#include <chassis/ChassisMovement.h>
#include <chassis/ChassisFactory.h>
#include <utils/logging/Logger.h>
#include <chassis/swerve/headingStates/SpecifiedHeading.h>

using frc::Pose2d;

TrajectoryDrivePathPlanner::TrajectoryDrivePathPlanner(RobotDrive *robotDrive) : RobotDrive(),
                                                                                 m_path(),
                                                                                 m_robotDrive(robotDrive),
                                                                                 m_chassis(ChassisFactory::GetChassisFactory()->GetSwerveChassis()),
                                                                                 m_holonomicController(pathplanner::PIDConstants(1.0, 1.0, 1.0, 1.0),                                                                                               // translation gains kP, kI, kD, iZone
                                                                                                       pathplanner::PIDConstants(1.0, 1.0, 1.0, 1.0),                                                                                               // rotational gains
                                                                                                       units::velocity::meters_per_second_t(4.0),                                                                                                   // max MODULE speed
                                                                                                       units::meter_t(std::sqrt(std::pow(m_chassis->GetWheelBase().to<double>(), 2) + std::pow(m_chassis->GetWheelBase().to<double>(), 2)) / 2.0)), // driveBaseRadius / furthest swerve module
                                                                                 // m_desiredState(),
                                                                                 m_prevPose(ChassisFactory::GetChassisFactory()->GetSwerveChassis()->GetPose()),
                                                                                 m_wasMoving(false),
                                                                                 m_timer(std::make_unique<frc::Timer>()),
                                                                                 m_whyDone("Trajectory isn't finished/Error")

{
}

void TrajectoryDrivePathPlanner::Init(ChassisMovement &chassisMovement)
{
    // m_holonomicController.setTolerance(frc::Pose2d{units::length::meter_t(0.1), units::length::meter_t(0.1), frc::Rotation2d(units::angle::degree_t(2.0))});
    m_path = std::make_shared<pathplanner::PathPlannerPath>(chassisMovement.path);
    m_trajectory = pathplanner::PathPlannerTrajectory(m_path, frc::ChassisSpeeds(){});

    if (!m_trajectory.getStates().empty()) // only go if path name found
    {
        // Desired state is first state in trajectory
        // m_desiredState = m_trajectoryStates.front(); // m_desiredState is the first state, or starting position

        m_finalState = m_trajectory.getEndState();

        m_timer.get()->Reset(); // Restarts and starts timer
        m_timer.get()->Start();
    }

    m_delta = m_finalState.getTargetHolonomicPose() - m_chassis->GetPose();
}

std::array<frc::SwerveModuleState, 4> TrajectoryDrivePathPlanner::UpdateSwerveModuleStates(ChassisMovement &chassisMovement)
{
    // Using event markers for robot actions
    // need to create dummy comands and then can call functions based on names
    // or get commands to work (maybe InstantCommand?)
    for (pathplanner::EventMarker &marker : m_path->getEventMarkers())
    {
        if (marker.shouldTrigger(m_chassis->GetPose()))
        {
            auto command = marker.getCommand();

            command.get()->GetName();
        }
    }

    if (!m_trajectory.getStates().empty()) // If we have a path parsed / have states to run
    {

        // NOTE this if statement could be troublesome in the future if these two poses never equal each other
        /// @todo is there a better way to get the initial pose for a PathPlannerPath (there is InitialDifferentialPose...)
        frc::Pose2d incomingInitialPose = frc::Pose2d(chassisMovement.path.getPoint(0).position, chassisMovement.path.getPoint(0).holonomicRotation.value());
        if (m_trajectory.getInitialTargetHolonomicPose() != incomingInitialPose)
        {
            Init(chassisMovement);
        }

        units::second_t currentTime = m_timer.get()->Get();
        pathplanner::PathPlannerTrajectory::State targetState = m_trajectory.sample(currentTime); // in path following code from pathplanner, if the drive controller
                                                                                                  // is holonomic, the sampled state is flipped, don't know why...

        frc::ChassisSpeeds targetSpeeds = m_holonomicController.calculateRobotRelativeSpeeds(m_chassis->GetPose(), targetState);

        // Set chassisMovement speeds that will be used by RobotDrive
        return m_robotDrive->UpdateSwerveModuleStates(chassisMovement);
    }
    else // If we don't have states to run, don't move the robot
    {
        // Create 0 speed frc::ChassisSpeeds
        frc::ChassisSpeeds speeds;
        speeds.vx = 0_mps;
        speeds.vy = 0_mps;
        speeds.omega = units::angular_velocity::radians_per_second_t(0);

        // Set chassisMovement speeds that will be used by RobotDrive
        chassisMovement.chassisSpeeds = speeds;

        return m_robotDrive->UpdateSwerveModuleStates(chassisMovement);
    }
}

bool TrajectoryDrivePathPlanner::IsDone()
{
    bool isDone = false;

    if (!m_trajectory.getStates().empty()) // If we have states...
    {
        // isDone = m_holonomicController.atReference();
        isDone = IsSamePose(m_chassis->GetPose(), m_trajectory.getEndState().getTargetHolonomicPose(), 10.0, 1.0);
    }
    else
    {
        m_whyDone = "No states in trajectory";
        isDone = true;
        Logger::GetLogger()->LogData(LOGGER_LEVEL::PRINT, "trajectory drive path planner", "why done", m_whyDone);
    }

    return isDone;
}

bool TrajectoryDrivePathPlanner::IsSamePose(frc::Pose2d currentPose, frc::Pose2d previousPose, double xyTolerance, double rotTolerance)
{
    // Detect if the two poses are the same within a tolerance
    double dCurPosX = currentPose.X().to<double>() * 100; // cm
    double dCurPosY = currentPose.Y().to<double>() * 100;
    double dPrevPosX = previousPose.X().to<double>() * 100;
    double dPrevPosY = previousPose.Y().to<double>() * 100;

    double dCurPosRot = currentPose.Rotation().Degrees().to<double>();
    double dPrevPosRot = previousPose.Rotation().Degrees().to<double>();

    double dDeltaX = abs(dPrevPosX - dCurPosX);
    double dDeltaY = abs(dPrevPosY - dCurPosY);
    double dDeltaRot = abs(dPrevPosRot - dCurPosRot);

    //  If Position of X or Y has moved since last scan..  Using Delta X/Y
    return ((dDeltaX <= xyTolerance) && (dDeltaY <= xyTolerance) && (dDeltaRot <= rotTolerance));
}