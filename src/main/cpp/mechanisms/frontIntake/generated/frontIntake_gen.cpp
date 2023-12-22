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
// Generated on Thursday, December 21, 2023 12:43:53 PM

#include <string>

// FRC Includes
#include <networktables/NetworkTableInstance.h>
#include "hw/interfaces/IDragonMotorController.h"

#include "frontIntake_gen.h"

using ctre::phoenixpro::signals::ForwardLimitSourceValue;
using ctre::phoenixpro::signals::ForwardLimitTypeValue;
using ctre::phoenixpro::signals::ReverseLimitSourceValue;
using ctre::phoenixpro::signals::ReverseLimitTypeValue;
using ctre::phoenixpro::signals::InvertedValue;
using ctre::phoenixpro::signals::NeutralModeValue;
using ctre::phoenix::motorcontrol::RemoteSensorSource;

frontIntake_gen::frontIntake_gen() : intakeMechanism ( MechanismTypes::MECHANISM_TYPE::FRONT_INTAKE, std::string ( "frontIntake" ) ),
	m_motorMap(),
	m_solenoidMap(),
	m_servoMap()
{
}

void frontIntake_gen::Create()
{
	m_ntName = "frontIntake";
	mainWheel = new DragonTalonFX ( "mainWheel",RobotElementNames::MOTOR_CONTROLLER_USAGE::INTAKE_MECHANISM_MAIN_WHEEL,9,"rio" );
	pushSolenoid = new DragonSolenoid ( "pushSolenoid",RobotElementNames::SOLENOID_USAGE::INTAKE_MECHANISM_PUSH_SOLENOID,1,frc::PneumaticsModuleType::CTREPCM,5,false );
	armServo = new DragonServo ( RobotElementNames::SERVO_USAGE::INTAKE_MECHANISM_ARM_SERVO,0,units::angle::degree_t ( 0 ),units::angle::degree_t ( 360 ) );
//todo add someting here for Forward;
//todo add someting here for MaxTravel;
//todo add someting here for MinTravel;
//todo add someting here for Reverse;
//todo add someting here for Stop;
//todo add someting here for Panic;

	m_motorMap[mainWheel->GetType()] = new BaseMechMotor ( m_ntName, *mainWheel, BaseMechMotor::EndOfTravelSensorOption::NONE, nullptr, BaseMechMotor::EndOfTravelSensorOption::NONE, nullptr );
	m_solenoidMap[pushSolenoid->GetType()] = new BaseMechSolenoid ( m_ntName, *pushSolenoid );
	m_servoMap[armServo->GetUsage()] = new BaseMechServo ( m_ntName, armServo );
//todo add someting here for Forward;
//todo add someting here for MaxTravel;
//todo add someting here for MinTravel;
//todo add someting here for Reverse;
//todo add someting here for Stop;
//todo add someting here for Panic;

	m_table = nt::NetworkTableInstance::GetDefault().GetTable ( m_ntName );
	m_tuningIsEnabledStr = "Enable Tuning for " + m_ntName; // since this string is used every loop, we do not want to create the string every time
	m_table.get()->PutBoolean ( m_tuningIsEnabledStr, m_tuning );
}

void frontIntake_gen::Initialize ( RobotConfigMgr::RobotIdentifier robotFullName )
{
	if ( false ) {}
	else if ( RobotConfigMgr::RobotIdentifier::CompBot_1 == robotFullName )
	{
		mainWheel->SetCurrentLimits ( false,
		                              units::current::ampere_t ( 0 ),
		                              false,
		                              units::current::ampere_t ( 0 ),
		                              units::current::ampere_t ( 0 ),
		                              units::time::second_t ( 0 ) );
		mainWheel->ConfigHWLimitSW ( false, // enableForward
		                             0, // remoteForwardSensorID
		                             false, // forwardResetPosition
		                             0, // forwardPosition
		                             ForwardLimitSourceValue::LimitSwitchPin, // forwardType
		                             ForwardLimitTypeValue::NormallyOpen, // forwardOpenClose
		                             false, // enableReverse
		                             0, // remoteReverseSensorID
		                             false, // reverseResetPosition
		                             0, // reversePosition
		                             ReverseLimitSourceValue::LimitSwitchPin, // revType
		                             ReverseLimitTypeValue::NormallyOpen ); // revOpenClose
		mainWheel->ConfigMotorSettings ( InvertedValue::CounterClockwise_Positive, // ctre::phoenixpro::signals::InvertedValue
		                                 NeutralModeValue::Coast, // ctre::phoenixpro::signals::NeutralModeValue
		                                 0, // deadbandPercent
		                                 0, // peakForwardDutyCycle
		                                 0 ); // peakReverseDutyCycle
		mainWheel->SetAsFollowerMotor ( 0 ); // masterCANID
		mainWheel->SetRemoteSensor ( 0, // canID
		                             RemoteSensorSource::RemoteSensorSource_Off ); // ctre::phoenix::motorcontrol::RemoteSensorSource
		mainWheel->SetDiameter ( 0 ); // double diameter
// pushSolenoid : Solenoids do not have initialization needs
// armServo : Servos do not have initialization needs
//todo create initialization for Forward
//todo create initialization for MaxTravel
//todo create initialization for MinTravel
//todo create initialization for Reverse
//todo create initialization for Stop
//todo create initialization for Panic
	}
	else if ( RobotConfigMgr::RobotIdentifier::PracticeBot_9900 == robotFullName )
	{
		mainWheel->SetCurrentLimits ( false,
		                              units::current::ampere_t ( 0 ),
		                              false,
		                              units::current::ampere_t ( 0 ),
		                              units::current::ampere_t ( 0 ),
		                              units::time::second_t ( 0 ) );
		mainWheel->ConfigHWLimitSW ( false, // enableForward
		                             0, // remoteForwardSensorID
		                             false, // forwardResetPosition
		                             0, // forwardPosition
		                             ForwardLimitSourceValue::LimitSwitchPin, // forwardType
		                             ForwardLimitTypeValue::NormallyOpen, // forwardOpenClose
		                             false, // enableReverse
		                             0, // remoteReverseSensorID
		                             false, // reverseResetPosition
		                             0, // reversePosition
		                             ReverseLimitSourceValue::LimitSwitchPin, // revType
		                             ReverseLimitTypeValue::NormallyOpen ); // revOpenClose
		mainWheel->ConfigMotorSettings ( InvertedValue::CounterClockwise_Positive, // ctre::phoenixpro::signals::InvertedValue
		                                 NeutralModeValue::Coast, // ctre::phoenixpro::signals::NeutralModeValue
		                                 0, // deadbandPercent
		                                 0, // peakForwardDutyCycle
		                                 0 ); // peakReverseDutyCycle
		mainWheel->SetAsFollowerMotor ( 0 ); // masterCANID
		mainWheel->SetRemoteSensor ( 0, // canID
		                             RemoteSensorSource::RemoteSensorSource_Off ); // ctre::phoenix::motorcontrol::RemoteSensorSource
		mainWheel->SetDiameter ( 0 ); // double diameter
// pushSolenoid : Solenoids do not have initialization needs
// armServo : Servos do not have initialization needs
//todo create initialization for Forward
//todo create initialization for MaxTravel
//todo create initialization for MinTravel
//todo create initialization for Reverse
//todo create initialization for Stop
//todo create initialization for Panic
	}

}

/// @brief  Set the control constants (e.g. PIDF values).
/// @param [in] ControlData*                                   pid:  the control constants
/// @return void
void frontIntake_gen::SetControlConstants ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, int slot, ControlData pid )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->SetControlConstants ( slot, pid );
	}
}

/// @brief update the output to the mechanism using the current controller and target value(s)
/// @return void
void frontIntake_gen::Update()
{
	for ( auto motor : m_motorMap )
	{
		motor.second->Update();
	}
}

void frontIntake_gen::UpdateTarget ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, double percentOutput )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->UpdateTarget ( percentOutput );
	}
}

void frontIntake_gen::UpdateTarget ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, units::angle::degree_t angle )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->UpdateTarget ( angle );
	}
}

void frontIntake_gen::UpdateTarget ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, units::angular_velocity::revolutions_per_minute_t angVel )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->UpdateTarget ( angVel );
	}
}
void frontIntake_gen::UpdateTarget ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, units::length::inch_t position )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->UpdateTarget ( position );
	}
}
void frontIntake_gen::UpdateTarget ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier, units::velocity::feet_per_second_t velocity )
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		motor->UpdateTarget ( velocity );
	}
}
void frontIntake_gen::UpdateTarget ( RobotElementNames::SOLENOID_USAGE identifier, bool extend )
{
	auto sol = GetSolenoidMech ( identifier );
	if ( sol != nullptr )
	{
		sol->ActivateSolenoid ( extend );
	}
}

bool frontIntake_gen::IsAtMinPosition ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier ) const
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		return motor->IsAtMinTravel();
	}
	return false;
}
bool frontIntake_gen::IsAtMinPosition ( RobotElementNames::SOLENOID_USAGE identifier ) const
{
	auto sol = GetSolenoidMech ( identifier );
	if ( sol != nullptr )
	{
		return !sol->IsSolenoidActivated();
	}
	return false;
}
bool frontIntake_gen::IsAtMaxPosition ( RobotElementNames::MOTOR_CONTROLLER_USAGE identifier ) const
{
	auto motor = GetMotorMech ( identifier );
	if ( motor != nullptr )
	{
		return motor->IsAtMaxTravel();
	}
	return false;
}
bool frontIntake_gen::IsAtMaxPosition ( RobotElementNames::SOLENOID_USAGE identifier ) const
{
	auto sol = GetSolenoidMech ( identifier );
	if ( sol != nullptr )
	{
		return sol->IsSolenoidActivated();
	}
	return false;
}

BaseMechMotor *frontIntake_gen::GetMotorMech ( RobotElementNames::MOTOR_CONTROLLER_USAGE usage ) const
{
	auto itr = m_motorMap.find ( usage );
	if ( itr != m_motorMap.end() )
	{
		return itr->second;
	}
	return nullptr;
}

std::vector<RobotElementNames::MOTOR_CONTROLLER_USAGE> frontIntake_gen::GetMotorUsages() const
{
	std::vector<RobotElementNames::MOTOR_CONTROLLER_USAGE> output;
	for ( auto itr = m_motorMap.begin(); itr != m_motorMap.end(); ++itr )
	{
		output.emplace_back ( itr->first );
	}
	return output;
}

BaseMechSolenoid *frontIntake_gen::GetSolenoidMech ( RobotElementNames::SOLENOID_USAGE usage ) const
{
	auto itr = m_solenoidMap.find ( usage );
	if ( itr != m_solenoidMap.end() )
	{
		return itr->second;
	}
	return nullptr;
}

std::vector<RobotElementNames::SOLENOID_USAGE> frontIntake_gen::GetSolenoidUsages() const
{
	std::vector<RobotElementNames::SOLENOID_USAGE> output;
	for ( auto itr = m_solenoidMap.begin(); itr != m_solenoidMap.end(); ++itr )
	{
		output.emplace_back ( itr->first );
	}
	return output;
}

BaseMechServo *frontIntake_gen::GetServoMech ( RobotElementNames::SERVO_USAGE usage ) const
{
	auto itr = m_servoMap.find ( usage );
	if ( itr != m_servoMap.end() )
	{
		return itr->second;
	}
	return nullptr;
}

std::vector<RobotElementNames::SERVO_USAGE> frontIntake_gen::GetServoUsages() const
{
	std::vector<RobotElementNames::SERVO_USAGE> output;
	for ( auto itr = m_servoMap.begin(); itr != m_servoMap.end(); ++itr )
	{
		output.emplace_back ( itr->first );
	}
	return output;
}