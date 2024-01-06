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
// Generated on Friday, January 5, 2024 8:15:20 PM

#include <string>

#include "PeriodicLooper.h"
#include "utils/logging/Logger.h"
#include "configs/RobotConfigMgr.h"
#include "configs/RobotConfigCompBot_1.h"

using std::string;

void RobotConfigCompBot_1::DefineMechanisms()
{
	Logger::GetLogger()->LogData ( LOGGER_LEVEL::PRINT, string ( "Initializing mechanism" ), string ( "frontIntake" ), "" );
	frontIntake_gen* frontIntake_genmech = new frontIntake_gen();
	m_thefrontIntake = new frontIntake ( frontIntake_genmech );
	m_thefrontIntake->Create();
	m_thefrontIntake->Initialize ( RobotConfigMgr::RobotIdentifier::CompBot_1 );
	m_thefrontIntake->Init ( m_thefrontIntake );
	PeriodicLooper::GetInstance()->RegisterAll ( m_thefrontIntake );

	Logger::GetLogger()->LogData ( LOGGER_LEVEL::PRINT, string ( "Initializing mechanism" ), string ( "sideEject" ), "" );
	sideEject_gen* sideEject_genmech = new sideEject_gen();
	m_thesideEject = new sideEject ( sideEject_genmech );
	m_thesideEject->Create();
	m_thesideEject->Initialize ( RobotConfigMgr::RobotIdentifier::CompBot_1 );
	m_thesideEject->Init ( m_thesideEject );
	PeriodicLooper::GetInstance()->RegisterAll ( m_thesideEject );

	Logger::GetLogger()->LogData ( LOGGER_LEVEL::PRINT, string ( "Initializing mechanism" ), string ( "Chassis_1" ), "" );
	Chassis_1_gen* Chassis_1_genmech = new Chassis_1_gen();
	m_theChassis_1 = new Chassis_1 ( Chassis_1_genmech );
	m_theChassis_1->Create();
	m_theChassis_1->Initialize ( RobotConfigMgr::RobotIdentifier::CompBot_1 );
	m_theChassis_1->Init ( m_theChassis_1 );
	PeriodicLooper::GetInstance()->RegisterAll ( m_theChassis_1 );

}
