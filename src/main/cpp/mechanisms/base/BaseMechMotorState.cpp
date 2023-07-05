
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
#include "mechanisms/base/BaseMechMotor.h"
#include "mechanisms/base/BaseMechMotorState.h"
#include "mechanisms/controllers/ControlData.h"
#include "mechanisms/controllers/MechanismTargetData.h"
#include "mechanisms/base/Mech.h"
#include "utils/logging/Logger.h"

#include "teleopcontrol/TeleopControl.h"

// Third Party Includes

using namespace std;

/// @class BaseMechMotorState
/// @brief information about the control (open loop, closed loop position, closed loop velocity, etc.) for a mechanism state
BaseMechMotorState::BaseMechMotorState(string stateName,
                                       int stateId,
                                       BaseMechMotor &mech) : State(stateName, stateId),
                                                              m_mech(mech)
{
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param percentOutput target value
void BaseMechMotorState::SetTargetIControl(double percentOutput)
{
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param angle target value
void BaseMechMotorState::SetTargetIControl(ControlData &controlConst, units::angle::degree_t angle)
{
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param angularVelocity target value
void BaseMechMotorState::SetTargetIControl(ControlData &controlConst, units::angular_velocity::revolutions_per_minute_t angVel)
{
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param position target value
void BaseMechMotorState::SetTargetIControl(ControlData &controlConst, units::length::inch_t position)
{
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param velocity target value
void BaseMechMotorState::SetTargetIControl(ControlData &controlConst, units::velocity::feet_per_second_t velocity)
{
}

void BaseMechMotorState::Init()
{
}

void BaseMechMotorState::Run()
{
}

void BaseMechMotorState::Exit()
{
}

bool BaseMechMotorState::AtTarget() const
{
    return false;
}
