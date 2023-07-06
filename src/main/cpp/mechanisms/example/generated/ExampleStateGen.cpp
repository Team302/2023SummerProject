
//====================================================================================================================================================
/// Copyright 2023 Lake Orion Robotics FIRST Team 302
///
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
/// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
/// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
/// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
/// OR OTHER DEALINGS IN THE SOFTWARE.
//====================================================================================================================================================

// C++ Includes
#include <memory>
#include <string>

// FRC includes

// Team 302 includes
#include "State.h"
#include "mechanisms/base/BaseMechMotorState.h"
#include "mechanisms/base/BaseMechServoState.h"
#include "mechanisms/base/BaseMechSolenoidState.h"
#include "mechanisms/example/generated/ExampleGen.h"
#include "mechanisms/example/generated/ExampleStateGen.h"
#include "mechanisms/controllers/ControlData.h"
#include "mechanisms/controllers/MechanismTargetData.h"
#include "mechanisms/base/Mech.h"
#include "utils/logging/Logger.h"

#include "teleopcontrol/TeleopControl.h"

// Third Party Includes

using namespace std;

/// @class ExampleStateGen
/// @brief information about the control (open loop, closed loop position, closed loop velocity, etc.) for a mechanism state
ExampleStateGen::ExampleStateGen(string stateName,
                                 int stateId,
                                 ExampleGen &mech) : State(stateName, stateId),
                                                     m_example(mech),
                                                     m_motorMap(),
                                                     m_solenoidMap()
{
    auto motorUsages = m_example.GetMotorUsages();
    for (auto usage : motorUsages)
    {
        auto motormech = m_example.GetMotorMech(usage);
        m_motorMap[usage] = new BaseMechMotorState(stateName, stateId, *motormech);
    }
    auto solUsages = m_example.GetSolenoidUsages();
    for (auto usage : solUsages)
    {
        auto solmech = m_example.GetSolenoidMech(usage);
        m_solenoidMap[usage] = new BaseMechSolenoidState(stateName, stateId, *solmech);
    }
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param percentOutput target value
void ExampleStateGen::SetTargetControl(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, double percentOutput)
{
    auto motormech = GetMotorMechState(identifier);
    if (motormech != nullptr)
    {
        motormech->SetTargetControl(percentOutput);
    }
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param angle target value
void ExampleStateGen::SetTargetControl(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::angle::degree_t angle)
{
    auto motormech = GetMotorMechState(identifier);
    if (motormech != nullptr)
    {
        motormech->SetTargetControl(controlConst, angle);
    }
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param angularVelocity target value
void ExampleStateGen::SetTargetControl(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::angular_velocity::revolutions_per_minute_t angVel)
{
    auto motormech = GetMotorMechState(identifier);
    if (motormech != nullptr)
    {
        motormech->SetTargetControl(controlConst, angVel);
    }
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param position target value
void ExampleStateGen::SetTargetControl(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::length::inch_t position)
{
    auto motormech = GetMotorMechState(identifier);
    if (motormech != nullptr)
    {
        motormech->SetTargetControl(controlConst, position);
    }
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param velocity target value
void ExampleStateGen::SetTargetControl(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::velocity::feet_per_second_t velocity)
{
    auto motormech = GetMotorMechState(identifier);
    if (motormech != nullptr)
    {
        motormech->SetTargetControl(controlConst, velocity);
    }
}

/// @brief Set the target value for the actuator
/// @param identifier solenoid Usage to indicate what motor to update
/// @param extend target value
void ExampleStateGen::SetTargetControl(SolenoidUsage::SOLENOID_USAGE identifier, bool extend)
{
    auto solmech = GetSolenoidMechState(identifier);
    if (solmech != nullptr)
    {
        solmech->SetTarget(extend);
    }
}

void ExampleStateGen::Init()
{
    auto motorUsages = m_example.GetMotorUsages();
    for (auto usage : motorUsages)
    {
        auto state = GetMotorMechState(usage);
        if (state != nullptr)
        {
            state->Init();
        }
    }
    auto solUsages = m_example.GetSolenoidUsages();
    for (auto usage : solUsages)
    {
        auto state = GetSolenoidMechState(usage);
        if (state != nullptr)
        {
            state->Init();
        }
    }
}

void ExampleStateGen::Run()
{
    auto motorUsages = m_example.GetMotorUsages();
    for (auto usage : motorUsages)
    {
        auto state = GetMotorMechState(usage);
        if (state != nullptr)
        {
            state->Run();
        }
    }
    auto solUsages = m_example.GetSolenoidUsages();
    for (auto usage : solUsages)
    {
        auto state = GetSolenoidMechState(usage);
        if (state != nullptr)
        {
            state->Run();
        }
    }
}

void ExampleStateGen::Exit()
{
    auto motorUsages = m_example.GetMotorUsages();
    for (auto usage : motorUsages)
    {
        auto state = GetMotorMechState(usage);
        if (state != nullptr)
        {
            state->Exit();
        }
    }
    auto solUsages = m_example.GetSolenoidUsages();
    for (auto usage : solUsages)
    {
        auto state = GetSolenoidMechState(usage);
        if (state != nullptr)
        {
            state->Exit();
        }
    }
}

bool ExampleStateGen::AtTarget() const
{
    auto attarget = true;
    auto motorUsages = m_example.GetMotorUsages();
    for (auto usage : motorUsages)
    {
        auto state = GetMotorMechState(usage);
        if (state != nullptr)
        {
            attarget = state->AtTarget();
            if (!attarget)
            {
                break;
            }
        }
    }

    if (attarget)
    {
        auto solUsages = m_example.GetSolenoidUsages();
        for (auto usage : solUsages)
        {
            auto state = GetSolenoidMechState(usage);
            if (state != nullptr)
            {
                attarget = state->AtTarget();
                if (!attarget)
                {
                    break;
                }
            }
        }
    }
    return attarget;
}

BaseMechMotorState *ExampleStateGen::GetMotorMechState(MotorControllerUsage::MOTOR_CONTROLLER_USAGE usage) const
{
    auto itr = m_motorMap.find(usage);
    if (itr != m_motorMap.end())
    {
        return itr->second;
    }
    return nullptr;
}

BaseMechSolenoidState *ExampleStateGen::GetSolenoidMechState(SolenoidUsage::SOLENOID_USAGE usage) const
{
    auto itr = m_solenoidMap.find(usage);
    if (itr != m_solenoidMap.end())
    {
        return itr->second;
    }
    return nullptr;
}
