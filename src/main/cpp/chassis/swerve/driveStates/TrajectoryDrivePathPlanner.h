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

#pragma once

// FRC Includes
#include <frc/Timer.h>

// Team302 Includes
#include <chassis/swerve/driveStates/RobotDrive.h>
#include "chassis/swerve/SwerveChassis.h"

// Third party includes
#include <pathplanner/lib/PathPlannerTrajectory.h>
#include <pathplanner/lib/controllers/PPHolonomicDriveController.h>

class TrajectoryDrivePathPlanner : public RobotDrive
{
public:
    TrajectoryDrivePathPlanner(RobotDrive *robotDrive);

    std::array<frc::SwerveModuleState, 4> UpdateSwerveModuleStates(
        ChassisMovement &chassisMovement) override;

    void Init(
        ChassisMovement &chassisMovement) override;

    std::string WhyDone() const { return m_whyDone; };
    bool IsDone();

private:
    void CalcCurrentAndDesiredStates();

    bool IsSamePose(frc::Pose2d currentPose, frc::Pose2d previousPose, double xyTolerance, double rotTolerance);

    pathplanner::PathPlannerTrajectory m_trajectory;
    RobotDrive *m_robotDrive;
    pathplanner::PPHolonomicDriveController m_holonomicController;
    pathplanner::PathPlannerTrajectory::PathPlannerState m_desiredState;
    std::vector<pathplanner::PathPlannerTrajectory::PathPlannerState> m_trajectoryStates;
    pathplanner::PathPlannerTrajectory::PathPlannerState m_finalState;
    frc::Pose2d m_prevPose;
    bool m_wasMoving;
    frc::Transform2d m_delta;
    std::unique_ptr<frc::Timer> m_timer;

    SwerveChassis *m_chassis;
    std::string m_whyDone;
};