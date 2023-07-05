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

// C++ Includes
#include <string>

// FRC Includes

// Team 302 includes
#include "hw/interfaces/IDragonMotorController.h"
#include "mechanisms/base/Mech.h"
#include "mechanisms/base/BaseMechMotor.h"
#include "mechanisms/example/generated/ExampleBase.h"

using std::string;

/// @brief  This method constructs the mechanism using composition with its various actuators and sensors.
/// @param controlFileName The control file with the PID constants and Targets for each state
/// @param networkTableName Location for logging information
/// @param motor  Motor in the mechanims - code generator should probably use the usage for the variable name
/// @param otherMotor Same as previous
/// @param solenoid Solenoid in the mechanism - code generator should probably use the usage for the variable name
/// Additional actuators and sensors are also in this list.
ExampleBase::ExampleBase(string controlFileName,
                         string networkTableName) : IExample(),
                                                    Mech(MechanismTypes::MECHANISM_TYPE::UNKNOWN_MECHANISM, controlFileName, networkTableName),
                                                    m_motorMap(),
                                                    m_solenoidMap()

{
}

void ExampleBase::AddMotor(IDragonMotorController &motor)
{
    m_motorMap[motor.GetType()] = new BaseMechMotor(GetNetworkTableName(), motor, BaseMechMotor::EndOfTravelSensorOption::NONE, nullptr, BaseMechMotor::EndOfTravelSensorOption::NONE, nullptr);
}

void ExampleBase::AddSolenoid(DragonSolenoid &solenoid)
{
    m_solenoidMap[solenoid.GetType()] = new BaseMechSolenoid(GetNetworkTableName(), solenoid);
}

/// @brief  Set the control constants (e.g. PIDF values).
/// @param [in] ControlData*                                   pid:  the control constants
/// @return void
void ExampleBase::SetControlConstants(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, int slot, ControlData *pid)
{
    auto motor = GetMotorMech(identifier);
    if (motor != nullptr)
    {
        motor->SetControlConstants(slot, pid);
    }
}

/// @brief update the output to the mechanism using the current controller and target value(s)
/// @return void
void ExampleBase::Update()
{
    for (auto motor : m_motorMap)
    {
        motor.second->Update();
    }
}

void ExampleBase::UpdateTarget(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, double percentOutput)
{
    auto motor = GetMotorMech(identifier);
    if (motor != nullptr)
    {
        motor->UpdateTarget(percentOutput);
    }
}

void ExampleBase::UpdateTarget(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, units::angle::degree_t angle)
{
    auto motor = GetMotorMech(identifier);
    if (motor != nullptr)
    {
        motor->UpdateTarget(angle);
    }
}

void ExampleBase::UpdateTarget(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, units::angular_velocity::revolutions_per_minute_t angVel)
{
    auto motor = GetMotorMech(identifier);
    if (motor != nullptr)
    {
        motor->UpdateTarget(angVel);
    }
}
void ExampleBase::UpdateTarget(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, units::length::inch_t position)
{
    auto motor = GetMotorMech(identifier);
    if (motor != nullptr)
    {
        motor->UpdateTarget(position);
    }
}
void ExampleBase::UpdateTarget(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, units::velocity::feet_per_second_t velocity)
{
    auto motor = GetMotorMech(identifier);
    if (motor != nullptr)
    {
        motor->UpdateTarget(velocity);
    }
}
void ExampleBase::UpdateTarget(SolenoidUsage::SOLENOID_USAGE identifier, bool extend)
{
    auto sol = GetSolenoidMech(identifier);
    if (sol != nullptr)
    {
        sol->ActivateSolenoid(extend);
    }
}

BaseMechMotor *ExampleBase::GetMotorMech(MotorControllerUsage::MOTOR_CONTROLLER_USAGE usage) const
{
    auto itr = m_motorMap.find(usage);
    if (itr != m_motorMap.end())
    {
        return itr->second;
    }
    return nullptr;
}
BaseMechSolenoid *ExampleBase::GetSolenoidMech(SolenoidUsage::SOLENOID_USAGE usage) const
{
    auto itr = m_solenoidMap.find(usage);
    if (itr != m_solenoidMap.end())
    {
        return itr->second;
    }
    return nullptr;
}
