
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

// C++ Includes
#include <map>
#include <string>
#include <vector>

// FRC Includes
#include "units/angle.h"
#include "units/angular_velocity.h"
#include "units/length.h"
#include "units/velocity.h"

// Team 302 includes
#include "hw/usages/MotorControllerUsage.h"
#include "mechanisms/base/Mech.h"
#include "mechanisms/base/BaseMechMotor.h"
#include "mechanisms/base/BaseMechSolenoid.h"
#include "mechanisms/example/generated/IExampleGen.h"

// forward declares
class IDragonMotorController;

class ExampleGen : public IExampleGen, public Mech
{
public:
    /// @brief  This method constructs the mechanism using composition with its various actuators and sensors.
    /// @param controlFileName The control file with the PID constants and Targets for each state
    /// @param networkTableName Location for logging information
    /// @param motor  Motor in the mechanims - code generator should probably use the usage for the variable name
    /// @param otherMotor Same as previous
    /// @param solenoid Solenoid in the mechanism - code generator should probably use the usage for the variable name
    /// Additional actuators and sensors are also in this list.
    ExampleGen(std::string controlFileName,
               std::string networkTableName);
    ExampleGen() = delete;
    ~ExampleGen() = default;

    void AddMotor(IDragonMotorController &motor) override;
    void AddSolenoid(DragonSolenoid &solenoid) override;

    /// @brief Set the control constants (e.g. PIDF values).
    /// @param indentifier the motor controller usage to identify the motor
    /// @param slot position on the motor controller to set
    /// @param pid control data / constants
    void SetControlConstants(MotorControllerUsage::MOTOR_CONTROLLER_USAGE indentifier, int slot, ControlData *pid) override;

    /// @brief update the output to the mechanism using the current controller and target value(s)
    void Update() override;

    /// @brief Set the target value for the actuator
    /// @param identifier Motor Control Usage to indicate what motor to update
    /// @param percentOutput target value
    void UpdateTarget(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, double percentOutput) override;

    /// @brief Set the target value for the actuator
    /// @param identifier Motor Control Usage to indicate what motor to update
    /// @param angle target value
    void UpdateTarget(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, units::angle::degree_t angle) override;

    /// @brief Set the target value for the actuator
    /// @param identifier Motor Control Usage to indicate what motor to update
    /// @param angularVelocity target value
    void UpdateTarget(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, units::angular_velocity::revolutions_per_minute_t angVel) override;

    /// @brief Set the target value for the actuator
    /// @param identifier Motor Control Usage to indicate what motor to update
    /// @param position target value
    void UpdateTarget(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, units::length::inch_t position) override;

    /// @brief Set the target value for the actuator
    /// @param identifier Motor Control Usage to indicate what motor to update
    /// @param velocity target value
    void UpdateTarget(MotorControllerUsage::MOTOR_CONTROLLER_USAGE identifier, units::velocity::feet_per_second_t velocity) override;

    /// @brief Set the target value for the actuator
    /// @param identifier solenoid Usage to indicate what motor to update
    /// @param extend target value
    void UpdateTarget(SolenoidUsage::SOLENOID_USAGE identifier, bool extend) override;

    std::vector<MotorControllerUsage::MOTOR_CONTROLLER_USAGE> GetMotorUsages() const;
    BaseMechMotor *GetMotorMech(MotorControllerUsage::MOTOR_CONTROLLER_USAGE usage) const;

    std::vector<SolenoidUsage::SOLENOID_USAGE> GetSolenoidUsages() const;
    BaseMechSolenoid *GetSolenoidMech(SolenoidUsage::SOLENOID_USAGE usage) const;

private:
    std::unordered_map<MotorControllerUsage::MOTOR_CONTROLLER_USAGE, BaseMechMotor *> m_motorMap;
    std::unordered_map<SolenoidUsage::SOLENOID_USAGE, BaseMechSolenoid *> m_solenoidMap;
};
