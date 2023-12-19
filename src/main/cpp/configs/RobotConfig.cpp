
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

#include "configs/RobotConfig.h"
#include "chassis/swerve/SwerveChassis.h"

RobotConfig::RobotConfig()
{
}

void RobotConfig::BuildRobot()
{
    DefineBuiltInControlItems();

    DefineChassis();
    DefineMechanisms();
}

RobotConfig::~RobotConfig()
{
}

void RobotConfig::DefineBuiltInControlItems()
{
}

void RobotConfig::DefineChassis()
{
}

void RobotConfig::DefineMechanisms()
{
}

SwerveChassis *RobotConfig::GetSwerveChassis() const
{
    return nullptr;
}
DifferentialChassis *RobotConfig::GetDifferentialChassis() const
{
    return nullptr;
}
MecanumChassis *RobotConfig::GetMecanumChassis() const
{
    return nullptr;
}
IChassis *RobotConfig::GetIChassis() const
{
    return nullptr;
}

IDragonMotorController *RobotConfig::GetMotorController(RobotElementNames::MOTOR_CONTROLLER_USAGE usage)
{
    return nullptr;
}

DragonSolenoid *RobotConfig::GetSolenoid(RobotElementNames::SOLENOID_USAGE usage)
{
    return nullptr;
}

DragonServo *RobotConfig::GetServo(RobotElementNames::SERVO_USAGE usage)
{
    return nullptr;
}

DragonCanCoder *RobotConfig::GetCanCoder(RobotElementNames::CANCODER_USAGE usage)
{
    return nullptr;
}

IDragonPigeon *RobotConfig::GetPigeon(RobotElementNames::PIGEON_USAGE usage)
{
    return nullptr;
}
