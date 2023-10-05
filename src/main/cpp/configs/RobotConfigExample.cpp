
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

#include "configs/RobotConfigExample.h"
#include "hw/DragonFalcon.h"
#include "mechanisms/example/decoratormods/Example.h"
#include "mechanisms/example/generated/ExampleGen.h"

void RobotConfigExample::DefineMotors()
{

    m_motor1 = new DragonFalcon("ExampleMech_Motor1",
                                MotorControllerUsage::MOTOR_CONTROLLER_USAGE::EXAMPLE_MOTOR1,
                                1,
                                "Canivore",
                                1);
    m_motor1->SetCurrentLimits(true,
                               units::current::ampere_t(25.0),
                               true,
                               units::current::ampere_t(25.0),
                               units::current::ampere_t(30.0),
                               units::time::second_t(0.02));
    m_motor1->ConfigMotorSettings(ctre::phoenixpro::signals::InvertedValue::CounterClockwise_Positive,
                                  ctre::phoenixpro::signals::NeutralModeValue::Brake,
                                  0.01, -1, 1);

    m_motor2 = new DragonFalcon("ExampleMech_Motor2",
                                MotorControllerUsage::MOTOR_CONTROLLER_USAGE::EXAMPLE_MOTOR2,
                                2,
                                "Canivore",
                                2);
    m_motor2->SetCurrentLimits(true,
                               units::current::ampere_t(25.0),
                               true,
                               units::current::ampere_t(25.0),
                               units::current::ampere_t(30.0),
                               units::time::second_t(0.02));
    m_motor2->ConfigMotorSettings(ctre::phoenixpro::signals::InvertedValue::CounterClockwise_Positive,
                                  ctre::phoenixpro::signals::NeutralModeValue::Brake,
                                  0.01, -1, 1);
}

void RobotConfigExample::DefineSolenoids()
{
}

void RobotConfigExample::DefineMechanisms()
{
    // TODO:  utilize the motors, solenoids, etc. to create the mechanisms
    auto genmech = new ExampleGen("Example.xml", "ExampleMech");
    m_example = new Example(genmech);
}

void RobotConfigExample::DefineCANSensors()
{
}
