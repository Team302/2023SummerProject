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

#include <string>

// FRC Includes
#include <frc/filter/SlewRateLimiter.h>
#include <units/velocity.h>
#include <units/angle.h>

// Team302 Includes

#include <chassis/swerve/driveStates/RobotDrive.h>
#include <chassis/ChassisFactory.h>
#include <chassis/ChassisMovement.h>
#include <utils/logging/Logger.h>
#include <utils/FMSData.h>

using std::string;

RobotDrive::RobotDrive() : ISwerveDriveState::ISwerveDriveState(),
                           m_flState(),
                           m_frState(),
                           m_blState(),
                           m_brState(),
                           m_wheelbase(units::length::inch_t(20.0)),
                           m_wheeltrack(units::length::inch_t(20.0)),
                           m_centerOfRotation(m_wheelbase / 2.0, m_wheeltrack / 2.0),
                           m_maxspeed(units::velocity::feet_per_second_t(1.0))
{
    auto chassis = ChassisFactory::GetChassisFactory()->GetSwerveChassis();
    if (chassis != nullptr)
    {
        m_wheelbase = chassis->GetWheelBase();
        m_wheeltrack = chassis->GetTrack();
        m_maxspeed = chassis->GetMaxSpeed();
        m_kinematics = chassis->GetKinematics();
    }
    else
    {
        Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR, string("RobotDrive"), string("Chassis"), string("nullptr"));
    }
}

std::array<frc::SwerveModuleState, 4> RobotDrive::UpdateSwerveModuleStates(ChassisMovement &chassisMovement)
{
    if (chassisMovement.checkTipping)
    {
        DecideTipCorrection(chassisMovement);
    }

    wpi::array<frc::SwerveModuleState, 4> states = m_kinematics.ToSwerveModuleStates(chassisMovement.chassisSpeeds, chassisMovement.centerOfRotationOffset + m_centerOfRotation);

    m_kinematics.DesaturateWheelSpeeds(*states, m_maxspeed);

    [ m_flState, m_frState, m_blState, m_brState ] = states;

    return {m_flState, m_frState, m_blState, m_brState};
}

void RobotDrive::DecideTipCorrection(ChassisMovement &chassisMovement)

{
    if (frc::DriverStation::IsFMSAttached())
    {
        if (frc::DriverStation::GetMatchTime() > 20)
        {
            CorrectForTipping(chassisMovement);
        }
    }
    else
    {
        CorrectForTipping(chassisMovement);
    }
}
void RobotDrive::CorrectForTipping(ChassisMovement &chassisMovement)
{
    auto chassis = ChassisFactory::GetChassisFactory()->GetSwerveChassis();
    if (chassis != nullptr)
    {
        // pitch is positive when back of robot is lifted and negative when front of robot is lifted
        // vx is positive driving forward, so if pitch is +, need to slow down
        auto pitch = chassis->GetPitch();
        if (std::abs(pitch.to<double>()) > chassisMovement.tippingTolerance.to<double>())
        {
            auto adjust = m_maxspeed * chassisMovement.tippingCorrection * pitch.to<double>();
            chassisMovement.chassisSpeeds.vx -= adjust;
        }

        // roll is positive when the left side of the robot is lifted and negative when the right side of the robot is lifted
        // vy is positive strafing left, so if roll is +, need to strafe slower
        auto roll = chassis->GetRoll();
        if (std::abs(roll.to<double>()) > chassisMovement.tippingTolerance.to<double>())
        {
            auto adjust = m_maxspeed * chassisMovement.tippingCorrection * roll.to<double>();
            chassisMovement.chassisSpeeds.vy -= adjust;
        }
    }
}

void RobotDrive::Init(ChassisMovement &chassisMovement)
{
}