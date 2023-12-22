// clang-format off
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
// This file was automatically generated by the Team 302 code generator version 1.1.0.0
// Generated on Thursday, December 21, 2023 11:08:14 AM

#pragma once
#include <string>

// C++ Includes
#include <memory>
#include <string>
#include <vector>

// FRC includes

// Team 302 includes
#include "mechanisms/base/BaseMech.h"
#include "utils/logging/Logger.h"
#include "teleopcontrol/TeleopControl.h"
#include "mechanisms/frontIntake/generated/frontIntake_Base_StateGen.h"

// Third Party Includes

using namespace std;

/// @class ExampleBaseStateGen
/// @brief information about the control (open loop, closed loop position, closed loop velocity, etc.) for a mechanism state
frontIntakeBaseStateGen::frontIntakeBaseStateGen ( string stateName,
        int stateId,
        frontIntake_gen &mech ) : State ( stateName, stateId ),
	m_frontIntake ( mech ),
	m_motorMap(),
	m_solenoidMap(),
	m_servoMap()
{
	auto motorUsages = m_frontIntake.GetMotorUsages();
	for ( auto usage : motorUsages )
	{
		auto motormech = m_frontIntake.GetMotorMech ( usage );
		m_motorMap[usage] = new BaseMechMotorState ( stateName, stateId, *motormech );
	}
	auto solUsages = m_frontIntake.GetSolenoidUsages();
	for ( auto usage : solUsages )
	{
		auto solmech = m_frontIntake.GetSolenoidMech ( usage );
		m_solenoidMap[usage] = new BaseMechSolenoidState ( stateName, stateId, *solmech );
	}
	auto servoUsages = m_frontIntake.GetServoUsages();
	for ( auto usage : servoUsages )
	{
		auto servoMech = m_frontIntake.GetServoMech ( usage );
		m_servoMap[usage] = new BaseMechServoState ( stateName, stateId, *servoMech );
	}
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param percentOutput target value
void frontIntakeBaseStateGen::SetTargetControl ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, double percentOutput )
{
	auto motormech = GetMotorMechState ( identifier );
	if ( motormech != nullptr )
	{
		motormech->SetTargetControl ( percentOutput );
	}
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param angle target value
void frontIntakeBaseStateGen::SetTargetControl ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::angle::degree_t angle )
{
	auto motormech = GetMotorMechState ( identifier );
	if ( motormech != nullptr )
	{
		motormech->SetTargetControl ( controlConst, angle );
	}
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param angularVelocity target value
void frontIntakeBaseStateGen::SetTargetControl ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::angular_velocity::revolutions_per_minute_t angVel )
{
	auto motormech = GetMotorMechState ( identifier );
	if ( motormech != nullptr )
	{
		motormech->SetTargetControl ( controlConst, angVel );
	}
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param position target value
void frontIntakeBaseStateGen::SetTargetControl ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::length::inch_t position )
{
	auto motormech = GetMotorMechState ( identifier );
	if ( motormech != nullptr )
	{
		motormech->SetTargetControl ( controlConst, position );
	}
}

/// @brief Set the target value for the actuator
/// @param identifier Motor Control Usage to indicate what motor to update
/// @param controlConst pid constants for controling motor
/// @param velocity target value
void frontIntakeBaseStateGen::SetTargetControl ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, ControlData &controlConst, units::velocity::feet_per_second_t velocity )
{
	auto motormech = GetMotorMechState ( identifier );
	if ( motormech != nullptr )
	{
		motormech->SetTargetControl ( controlConst, velocity );
	}
}

/// @brief Set the target value for the actuator
/// @param identifier solenoid Usage to indicate what motor to update
/// @param extend target value
void frontIntakeBaseStateGen::SetTargetControl ( RobotElementNames::SOLENOID_USAGE identifier, bool extend )
{
	auto solmech = GetSolenoidMechState ( identifier );
	if ( solmech != nullptr )
	{
		solmech->SetTarget ( extend );
	}
}

void frontIntakeBaseStateGen::SetTargetControl ( RobotElementNames::SERVO_USAGE identifier, units::angle::degree_t angle )
{
	auto servomech = GetServoMechState ( identifier );
	if ( servomech != nullptr )
	{
		servomech->SetTarget ( angle );
	}
}

void frontIntakeBaseStateGen::Init()
{
	InitMotorStates();
	InitSolenoidStates();
	InitServoStates();
}
void frontIntakeBaseStateGen::InitMotorStates()
{
	auto motorUsages = m_frontIntake.GetMotorUsages();
	for ( auto usage : motorUsages )
	{
		auto state = GetMotorMechState ( usage );
		if ( state != nullptr )
		{
			state->Init();
		}
	}
}
void frontIntakeBaseStateGen::InitSolenoidStates()
{
	auto solUsages = m_frontIntake.GetSolenoidUsages();
	for ( auto usage : solUsages )
	{
		auto state = GetSolenoidMechState ( usage );
		if ( state != nullptr )
		{
			state->Init();
		}
	}
}
void frontIntakeBaseStateGen::InitServoStates()
{
	auto servoUsages = m_frontIntake.GetServoUsages();
	for ( auto usage : servoUsages )
	{
		auto state = GetServoMechState ( usage );
		if ( state != nullptr )
		{
			state->Init();
		}
	}
}

void frontIntakeBaseStateGen::Run()
{
	RunMotorStates();
	RunSolenoidStates();
	RunServoStates();
}
void frontIntakeBaseStateGen::RunMotorStates()
{
	auto motorUsages = m_frontIntake.GetMotorUsages();
	for ( auto usage : motorUsages )
	{
		auto state = GetMotorMechState ( usage );
		if ( state != nullptr )
		{
			state->Run();
		}
	}
}
void frontIntakeBaseStateGen::RunSolenoidStates()
{
	auto solUsages = m_frontIntake.GetSolenoidUsages();
	for ( auto usage : solUsages )
	{
		auto state = GetSolenoidMechState ( usage );
		if ( state != nullptr )
		{
			state->Run();
		}
	}
}
void frontIntakeBaseStateGen::RunServoStates()
{
	auto servoUsages = m_frontIntake.GetServoUsages();
	for ( auto usage : servoUsages )
	{
		auto state = GetServoMechState ( usage );
		if ( state != nullptr )
		{
			state->Run();
		}
	}
}

void frontIntakeBaseStateGen::Exit()
{
	ExitMotorStates();
	ExitSolenoidStates();
	ExitServoStates();
}
void frontIntakeBaseStateGen::ExitMotorStates()
{
	auto motorUsages = m_frontIntake.GetMotorUsages();
	for ( auto usage : motorUsages )
	{
		auto state = GetMotorMechState ( usage );
		if ( state != nullptr )
		{
			state->Exit();
		}
	}
}
void frontIntakeBaseStateGen::ExitSolenoidStates()
{
	auto solUsages = m_frontIntake.GetSolenoidUsages();
	for ( auto usage : solUsages )
	{
		auto state = GetSolenoidMechState ( usage );
		if ( state != nullptr )
		{
			state->Exit();
		}
	}
}
void frontIntakeBaseStateGen::ExitServoStates()
{
	auto servoUsages = m_frontIntake.GetServoUsages();
	for ( auto usage : servoUsages )
	{
		auto state = GetServoMechState ( usage );
		if ( state != nullptr )
		{
			state->Exit();
		}
	}
}

bool frontIntakeBaseStateGen::AtTarget()
{
	auto attarget = AtTargetMotorStates();
	if ( attarget )
	{
		attarget = AtTargetSolenoidStates();
		if ( attarget )
		{
			attarget = AtTargetServoStates();
		}
	}
	return attarget;
}
bool frontIntakeBaseStateGen::AtTargetMotorStates() const
{
	auto attarget = true;
	auto motorUsages = m_frontIntake.GetMotorUsages();
	for ( auto usage : motorUsages )
	{
		auto state = GetMotorMechState ( usage );
		if ( state != nullptr )
		{
			attarget = state->AtTarget();
			if ( !attarget )
			{
				break;
			}
		}
	}
	return attarget;
}
bool frontIntakeBaseStateGen::AtTargetSolenoidStates() const
{
	auto attarget = true;
	auto motorUsages = m_frontIntake.GetMotorUsages();
	for ( auto usage : motorUsages )
	{
		auto state = GetMotorMechState ( usage );
		if ( state != nullptr )
		{
			attarget = state->AtTarget();
			if ( !attarget )
			{
				break;
			}
		}
	}
	return attarget;
}
bool frontIntakeBaseStateGen::AtTargetServoStates() const
{
	auto attarget = true;
	auto servoUsages = m_frontIntake.GetServoUsages();
	for ( auto usage : servoUsages )
	{
		auto state = GetServoMechState ( usage );
		if ( state != nullptr )
		{
			attarget = state->AtTarget();
			if ( !attarget )
			{
				break;
			}
		}
	}
	return attarget;
}

BaseMechMotorState *frontIntakeBaseStateGen::GetMotorMechState ( RobotElementNames::MOTOR_CONTROLLER_USAGE usage ) const
{
	auto itr = m_motorMap.find ( usage );
	if ( itr != m_motorMap.end() )
	{
		return itr->second;
	}
	return nullptr;
}

BaseMechSolenoidState *frontIntakeBaseStateGen::GetSolenoidMechState ( RobotElementNames::SOLENOID_USAGE usage ) const
{
	auto itr = m_solenoidMap.find ( usage );
	if ( itr != m_solenoidMap.end() )
	{
		return itr->second;
	}
	return nullptr;
}
BaseMechServoState *frontIntakeBaseStateGen::GetServoMechState ( RobotElementNames::SERVO_USAGE usage ) const
{
	auto itr = m_servoMap.find ( usage );
	if ( itr != m_servoMap.end() )
	{
		return itr->second;
	}
	return nullptr;
}