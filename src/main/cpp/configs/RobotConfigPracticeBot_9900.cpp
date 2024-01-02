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
// Generated on Tuesday, January 2, 2024 9:37:28 AM

#include <string>

#include "PeriodicLooper.h"
#include "utils/logging/Logger.h"
#include "configs/RobotConfigMgr.h"
#include "configs/RobotConfigPracticeBot_9900.h"

using std::string;

void RobotConfigPracticeBot_9900::DefineMechanisms()
{
	Logger::GetLogger()->LogData ( LOGGER_LEVEL::PRINT, string ( "Initializing mechanism" ), string ( "frontIntake" ), "" );
	frontIntake_gen* frontIntake_genmech = new frontIntake_gen();
	m_thefrontIntake = new frontIntake ( frontIntake_genmech );
	m_thefrontIntake->Create();
	m_thefrontIntake->Initialize ( RobotConfigMgr::RobotIdentifier::PracticeBot_9900 );
	m_thefrontIntake->Init ( m_thefrontIntake );
	PeriodicLooper::GetInstance()->RegisterAll ( m_thefrontIntake );

}
