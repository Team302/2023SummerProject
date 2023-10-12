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

// C++ Includes
#include <map>
#include <memory>
#include <string>

// FRC includes

// Team 302 includes
#include "configs/usages/CanCoderUsage.h"
#include "utils/logging/Logger.h"

// Third Party Includes

using namespace std;

CanCoderUsage *CanCoderUsage::m_instance = nullptr;
CanCoderUsage *CanCoderUsage::GetInstance()
{
	if (m_instance == nullptr)
	{
		m_instance = new CanCoderUsage();
	}
	return m_instance;
}

CanCoderUsage::CanCoderUsage()
{

	m_usageMap["LEFT_FRONT_SWERVE_ANGLE"] = CANCODER_USAGE::LEFT_FRONT_SWERVE_ANGLE;
	m_usageMap["RIGHT_FRONT_SWERVE_ANGLE"] = CANCODER_USAGE::RIGHT_FRONT_SWERVE_ANGLE;
	m_usageMap["LEFT_BACK_SWERVE_ANGLE"] = CANCODER_USAGE::LEFT_BACK_SWERVE_ANGLE;
	m_usageMap["RIGHT_BACK_SWERVE_ANGLE"] = CANCODER_USAGE::RIGHT_BACK_SWERVE_ANGLE;
	m_usageMap["ARM_ANGLE"] = CANCODER_USAGE::ARM_ANGLE;
	m_usageMap["EXAMPLE_CANCODER"] = CANCODER_USAGE::EXAMPLE_CANCODER;
}

CanCoderUsage::~CanCoderUsage()
{
	m_usageMap.clear();
}

CanCoderUsage::CANCODER_USAGE CanCoderUsage::GetUsage(
	string usageString)
{
	auto it = m_usageMap.find(usageString);
	if (it != m_usageMap.end())
	{
		return it->second;
	}
	Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR, string("CanCoderUsage::GetUsage"), string("unknown usage"), usageString);
	return CanCoderUsage::CANCODER_USAGE::UNKNOWN_CANCODER_USAGE;
}

std::string CanCoderUsage::GetUsage(CanCoderUsage::CANCODER_USAGE usage)
{
	for (auto thisUsage : m_usageMap)
	{
		if (thisUsage.second == usage)
		{
			return thisUsage.first;
		}
	}
	return string("");
}