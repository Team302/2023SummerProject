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
// Generated on Friday, December 1, 2023 6:29:53 AM

#include "configs/RobotConfigMgr.h"
#include "configs/RobotConfig.h"
#include "configs/RobotConfigCompBot_302.h"
#include "configs/RobotConfigPracticeBot_9900.h"

RobotConfigMgr *RobotConfigMgr::m_instance = nullptr;
RobotConfigMgr *RobotConfigMgr::GetInstance()
{
	if ( RobotConfigMgr::m_instance == nullptr )
	{
		RobotConfigMgr::m_instance = new RobotConfigMgr();
	}
	return RobotConfigMgr::m_instance;
}

RobotConfigMgr::RobotConfigMgr() : m_config ( nullptr )
{
}

void RobotConfigMgr::InitRobot ( RobotIdentifier id )
{
	switch ( id )
	{
	case RobotIdentifier::CompBot_302:
		m_config = new RobotConfigCompBot_302();
		break;
	case RobotIdentifier::PracticeBot_9900:
		m_config = new RobotConfigPracticeBot_9900();
		break;

	default:
		break;
	}

	if ( m_config != nullptr )
	{
		m_config->BuildRobot();
	}
}
