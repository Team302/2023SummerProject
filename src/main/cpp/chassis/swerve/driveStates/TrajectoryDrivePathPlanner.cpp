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
#include "auton/DragonEvent.h"

using frc::Pose2d;

TrajectoryDrivePathPlanner::TrajectoryDrivePathPlanner(RobotDrive *robotDrive) : RobotDrive(),
                                                                                 m_currentChassisMovement(),
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
    m_currentChassisMovement = chassisMovement;

    m_path = std::make_shared<pathplanner::PathPlannerPath>(*chassisMovement.path);
    m_trajectory = pathplanner::PathPlannerTrajectory(m_path, frc::ChassisSpeeds{}, m_chassis->GetYaw());

    if (!m_trajectory.getStates().empty()) // only go if path name found
    {
        m_timer.get()->Reset(); // Restarts and starts timer
        m_timer.get()->Start();
    }
}

std::array<frc::SwerveModuleState, 4> TrajectoryDrivePathPlanner::UpdateSwerveModuleStates(ChassisMovement &chassisMovement)
{
    m_currentChassisMovement = chassisMovement;

    if (!m_trajectory.getStates().empty()) // If we have a path parsed / have states to run
    {
        if (chassisMovement.path != nullptr)
        {
            if (m_trajectory.getInitialDifferentialPose() != chassisMovement.path->getStartingDifferentialPose())
            {
                Init(chassisMovement);
            }
        }

        units::second_t currentTime = m_timer.get()->Get();
        pathplanner::PathPlannerTrajectory::State targetState = m_trajectory.sample(currentTime); // in path following code from pathplanner, if the drive controller
                                                                                                  // is holonomic, the sampled state is flipped, don't know why...

        frc::ChassisSpeeds targetSpeeds = m_holonomicController.calculateRobotRelativeSpeeds(m_chassis->GetPose(), targetState);

        chassisMovement.chassisSpeeds = targetSpeeds;

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
        isDone = IsSamePose(m_chassis->GetPose(), m_trajectory.getEndState().getTargetHolonomicPose(), units::meter_t(0.1), units::degree_t(1.0));
    }
    else
    {
        m_whyDone = "No states in trajectory";
        isDone = true;
        Logger::GetLogger()->LogData(LOGGER_LEVEL::PRINT, "trajectory drive path planner", "why done", m_whyDone);
    }

    return isDone;
}

std::optional<frc::Rotation2d> GetRotationOverride(ChassisMovement movementInfo)
{
    switch (movementInfo.headingOption)
    {
    case ChassisOptionEnums::HeadingOption::FACE_APRIL_TAG:
    case ChassisOptionEnums::HeadingOption::FACE_GAME_PIECE:
    case ChassisOptionEnums::HeadingOption::TOWARD_GOAL:
        // these currently don't support the system of overriding target rotation for pathplanner
        // only manipulate ChassisSpeeds.omega, don't return a target rotation
        // can maybe change or handle by overriding omega after holo controller calcs targetSpeeds
        /// @todo FIX
        return std::nullopt;
        break;
    case ChassisOptionEnums::HeadingOption::MAINTAIN:
    case ChassisOptionEnums::HeadingOption::IGNORE:
        // don't need to override anything for maintain or ignore
        return std::nullopt;
        break;
    case ChassisOptionEnums::HeadingOption::SPECIFIED_ANGLE:
        // for specified angle, just return rotation2d holding the target angle
        return frc::Rotation2d(movementInfo.yawAngle);
        break;
    default:
        return std::nullopt;
        break;
    }
}

bool TrajectoryDrivePathPlanner::IsSamePose(frc::Pose2d currentPose, frc::Pose2d previousPose, units::meter_t xyTolerance, units::degree_t rotTolerance)
{
    // Detect if the two poses are the same within a tolerance
    frc::Transform2d delta = currentPose - previousPose;

    //  If Position of X or Y has moved since last scan...
    return ((units::math::abs(delta.X()) <= xyTolerance) && (units::math::abs(delta.Y()) <= xyTolerance) && (units::math::abs(delta.Rotation().Degrees()) <= rotTolerance));
}
