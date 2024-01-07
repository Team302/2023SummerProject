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
// Generated on Sunday, January 7, 2024 11:05:46 AM

#include <string>

// FRC Includes
#include <networktables/NetworkTableInstance.h>
#include "hw/interfaces/IDragonMotorController.h"

#include "sideEject_gen.h"

#include "hw/DragonSolenoid.h"

#include "mechanisms/sideEject/decoratormods/sideEject_state_1_State.h"

sideEject_gen::sideEject_gen() : ejectMechanism ( MechanismTypes::MECHANISM_TYPE::SIDE_EJECT, std::string ( "sideEject" ) ),
	m_motorMap(),
	m_solenoidMap(),
	m_servoMap()
{
}

void sideEject_gen::Create()
{
	m_ntName = "sideEject";

	ejectSolenoid = new DragonSolenoid ( "ejectSolenoid",RobotElementNames::SOLENOID_USAGE::EJECT_MECHANISM_EJECT_SOLENOID,1,frc::PneumaticsModuleType::CTREPCM,0,0,false );

	sideEjectstate_1State* state_1State = new sideEjectstate_1State ( string ( "state_1" ), 0, new sideEjectstate_1StateGen ( string ( "state_1" ), 0, *this ) );
	AddToStateVector ( state_1State );

	state_1State->RegisterTransitionState ( state_1State );

	m_table = nt::NetworkTableInstance::GetDefault().GetTable ( m_ntName );
	m_tuningIsEnabledStr = "Enable Tuning for " + m_ntName; // since this string is used every loop, we do not want to create the string every time
	m_table.get()->PutBoolean ( m_tuningIsEnabledStr, m_tuning );
}

void sideEject_gen::Initialize ( RobotConfigMgr::RobotIdentifier robotFullName )
{
	if ( false ) {}
	else if ( RobotConfigMgr::RobotIdentifier::CompBot_1 == robotFullName )
	{

// ejectSolenoid : Solenoids do not have initialization needs

//todo create initialization for state_1
	}

}

/// @brief  Set the control constants (e.g. PIDF values).
/// @param [in] ControlData*                                   pid:  the control constants
/// @return void
void sideEject_gen::SetControlConstants ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, int slot, ControlData pid )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->SetControlConstants ( slot, pid );
	}
}

/// @brief update the output to the mechanism using the current controller and target value(s)
/// @return void
void sideEject_gen::Update()
{
	for ( auto motor : m_motorMap )
	{
		motor.second->Update();
	}
}

void sideEject_gen::UpdateTarget ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, double percentOutput )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->UpdateTarget ( percentOutput );
	}
}

void sideEject_gen::UpdateTarget ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, units::angle::degree_t angle )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->UpdateTarget ( angle );
	}
}

void sideEject_gen::UpdateTarget ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, units::angular_velocity::revolutions_per_minute_t angVel )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->UpdateTarget ( angVel );
	}
}
void sideEject_gen::UpdateTarget ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, units::length::inch_t position )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->UpdateTarget ( position );
	}
}
void sideEject_gen::UpdateTarget ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, units::velocity::feet_per_second_t velocity )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->UpdateTarget ( velocity );
	}
}
void sideEject_gen::UpdateTarget ( RobotElementNames::SOLENOID_USAGE identifier, bool extend )
{
	auto sol = GetSolenoidMech ( identifier );
	if ( sol != nullptr )
	{
		sol->ActivateSolenoid ( extend );
	}
}

bool sideEject_gen::IsAtMinPosition ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier ) const
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		return motor->IsAtMinTravel();
	}
	return false;
}
bool sideEject_gen::IsAtMinPosition ( RobotElementNames::SOLENOID_USAGE identifier ) const
{
	auto sol = GetSolenoidMech ( identifier );
	if ( sol != nullptr )
	{
		return !sol->IsSolenoidActivated();
	}
	return false;
}
bool sideEject_gen::IsAtMaxPosition ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier ) const
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		return motor->IsAtMaxTravel();
	}
	return false;
}
bool sideEject_gen::IsAtMaxPosition ( RobotElementNames::SOLENOID_USAGE identifier ) const
{
	auto sol = GetSolenoidMech ( identifier );
	if ( sol != nullptr )
	{
		return sol->IsSolenoidActivated();
	}
	return false;
}

BaseMechMotor *sideEject_gen::GetMotorMech ( RobotElementNames::MOTOR_CONTROLLER_USAGE usage ) const
{
	auto itr = m_motorMap.find ( usage );
	if ( itr != m_motorMap.end() )
	{
		return itr->second;
	}
	return nullptr;
}

std::vector<RobotElementNames::MOTOR_CONTROLLER_USAGE> sideEject_gen::GetMotorUsages() const
{
	std::vector<RobotElementNames::MOTOR_CONTROLLER_USAGE> output;
	for ( auto itr = m_motorMap.begin(); itr != m_motorMap.end(); ++itr )
	{
		output.emplace_back ( itr->first );
	}
	return output;
}

BaseMechSolenoid *sideEject_gen::GetSolenoidMech ( RobotElementNames::SOLENOID_USAGE usage ) const
{
	auto itr = m_solenoidMap.find ( usage );
	if ( itr != m_solenoidMap.end() )
	{
		return itr->second;
	}
	return nullptr;
}

std::vector<RobotElementNames::SOLENOID_USAGE> sideEject_gen::GetSolenoidUsages() const
{
	std::vector<RobotElementNames::SOLENOID_USAGE> output;
	for ( auto itr = m_solenoidMap.begin(); itr != m_solenoidMap.end(); ++itr )
	{
		output.emplace_back ( itr->first );
	}
	return output;
}

BaseMechServo *sideEject_gen::GetServoMech ( RobotElementNames::SERVO_USAGE usage ) const
{
	auto itr = m_servoMap.find ( usage );
	if ( itr != m_servoMap.end() )
	{
		return itr->second;
	}
	return nullptr;
}

std::vector<RobotElementNames::SERVO_USAGE> sideEject_gen::GetServoUsages() const
{
	std::vector<RobotElementNames::SERVO_USAGE> output;
	for ( auto itr = m_servoMap.begin(); itr != m_servoMap.end(); ++itr )
	{
		output.emplace_back ( itr->first );
	}
	return output;
}