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
#include <string>

#include "State.h"
#include "mechanisms/base/BaseMechMotorState.h"
#include "mechanisms/controllers/MechanismTargetData.h"
#include "mechanisms/controllers/ControlData.h"
#include "mechanisms/example/generated/ExampleBase.h"

class ExampleStateBase : public State
{
public:
    ExampleStateBase(std::string stateName,
                     int stateId,
                     ExampleBase &example);
    ExampleStateBase() = delete;
    ~ExampleStateBase() = default;

    /// @brief Set the target value for the actuator
    /// @param identifier Motor Control Usage to indicate what motor to update
    /// @param percentOutput target value
    void SetTargetIControl(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, double percentOutput);

    /// @brief Set the target value for the actuator
    /// @param identifier Motor Control Usage to indicate what motor to update
    /// @param controlConst pid constants for controling motor
    /// @param angle target value
    void SetTargetIControl(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::angle::degree_t angle);

    /// @brief Set the target value for the actuator
    /// @param identifier Motor Control Usage to indicate what motor to update
    /// @param controlConst pid constants for controling motor
    /// @param angularVelocity target value
    void SetTargetIControl(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::angular_velocity::revolutions_per_minute_t angVel);

    /// @brief Set the target value for the actuator
    /// @param identifier Motor Control Usage to indicate what motor to update
    /// @param controlConst pid constants for controling motor
    /// @param position target value
    void SetTargetIControl(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::length::inch_t position);

    /// @brief Set the target value for the actuator
    /// @param identifier Motor Control Usage to indicate what motor to update
    /// @param controlConst pid constants for controling motor
    /// @param velocity target value
    void SetTargetIControl(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::velocity::feet_per_second_t velocity);

    /// @brief Set the target value for the actuator
    /// @param identifier solenoid Usage to indicate what motor to update
    /// @param extend target value
    void SetTargetIControl(SolenoidUsage::SOLENOID_USAGE identifier, bool extend);

    void Init() override;
    void Run() override;
    void Exit() override;
    bool AtTarget() const override;

    ExampleBase GetExample() { return m_example; }

private:
    ExampleBase m_example;
    std::unordered_map<MotorControllerUsage::MOTOR_CONTROLLER_USAGE, BaseMechMotorState *> m_motorMap;
    // std::unordered_map<SolenoidUsage::SOLENOID_USAGE, BaseMechSolenoid *> m_solenoidMap;
};
