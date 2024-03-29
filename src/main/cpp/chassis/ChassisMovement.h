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
#include <frc/kinematics/ChassisSpeeds.h>
#include <frc/trajectory/TrajectoryUtil.h>
#include <frc/geometry/Translation2d.h>

#include "units/angle.h"

// Team302 Includes
#include <chassis/ChassisOptionEnums.h>

// Third party includes
#include <pathplanner/lib/PathPlannerTrajectory.h>

/// @brief This is used to give all neccessary data to ISwerveDriveStates

struct ChassisMovement
{
    ChassisOptionEnums::DriveStateType driveOption = ChassisOptionEnums::DriveStateType::ROBOT_DRIVE;
    frc::ChassisSpeeds chassisSpeeds = frc::ChassisSpeeds();
    frc::Trajectory trajectory = frc::Trajectory();
    pathplanner::PathPlannerTrajectory pathplannerTrajectory = pathplanner::PathPlannerTrajectory();
    frc::Translation2d centerOfRotationOffset = frc::Translation2d();
    ChassisOptionEnums::HeadingOption headingOption = ChassisOptionEnums::HeadingOption::MAINTAIN;
    ChassisOptionEnums::NoMovementOption noMovementOption = ChassisOptionEnums::NoMovementOption::STOP;
    ChassisOptionEnums::AutonControllerType controllerType = ChassisOptionEnums::AutonControllerType::RAMSETE;
    units::angle::degree_t yawAngle = units::angle::degree_t(0.0);
    bool checkTipping = false;
    units::angle::degree_t tippingTolerance = units::angle::degree_t(5.0);
    double tippingCorrection = -0.1;
    ChassisOptionEnums::RELATIVE_POSITION gridPosition = ChassisOptionEnums::RELATIVE_POSITION::CENTER;
    ChassisOptionEnums::RELATIVE_POSITION nodePosition = ChassisOptionEnums::RELATIVE_POSITION::CENTER;
};