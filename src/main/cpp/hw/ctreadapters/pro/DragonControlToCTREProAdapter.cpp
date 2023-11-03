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
#include <string>

// FRC includes

// Team 302 includes
#include "hw/interfaces/IDragonControlToVendorControlAdapter.h"
#include "hw/ctreadapters/pro/DragonControlToCTREProAdapter.h"
#include "mechanisms/controllers/ControlData.h"
#include "mechanisms/controllers/ControlModes.h"
#include "utils/logging/Logger.h"
#include "hw/DragonTalonFX.h"

// Third Party Includes

using std::string;
using std::to_string;

DragonControlToCTREProAdapter::DragonControlToCTREProAdapter(string networkTableName,
															 int controllerSlot,
															 const ControlData &controlInfo,
															 const DistanceAngleCalcStruc &calcStruc,
															 DragonTalonFX &controller) : IDragonControlToVendorControlAdapter(), m_networkTableName(networkTableName),
																						  m_controllerSlot(controllerSlot),
																						  m_controlData(controlInfo),
																						  m_calcStruc(calcStruc),
																						  m_controller(&controller)
{
	SetPeakAndNominalValues(networkTableName, controlInfo);

	if (controlInfo.GetMode() == ControlModes::CONTROL_TYPE::POSITION_ABS_TICKS ||
		controlInfo.GetMode() == ControlModes::CONTROL_TYPE::POSITION_DEGREES ||
		controlInfo.GetMode() == ControlModes::CONTROL_TYPE::POSITION_DEGREES_ABSOLUTE ||
		controlInfo.GetMode() == ControlModes::CONTROL_TYPE::POSITION_INCH ||
		controlInfo.GetMode() == ControlModes::CONTROL_TYPE::VELOCITY_DEGREES ||
		controlInfo.GetMode() == ControlModes::CONTROL_TYPE::VELOCITY_INCH ||
		controlInfo.GetMode() == ControlModes::CONTROL_TYPE::VELOCITY_RPS ||
		controlInfo.GetMode() == ControlModes::CONTROL_TYPE::VOLTAGE ||
		controlInfo.GetMode() == ControlModes::CONTROL_TYPE::CURRENT ||
		controlInfo.GetMode() == ControlModes::CONTROL_TYPE::TRAPEZOID)
	{
		SetPIDConstants(networkTableName, controllerSlot, controlInfo);
	}

	if ( // controlInfo.GetMode() == ControlModes::CONTROL_TYPE::POSITION_ABS_TICKS ||
		controlInfo.GetMode() == ControlModes::CONTROL_TYPE::POSITION_DEGREES_ABSOLUTE ||
		controlInfo.GetMode() == ControlModes::CONTROL_TYPE::TRAPEZOID)
	{
		SetMaxVelocityAcceleration(networkTableName, controlInfo);
	}
}

void DragonControlToCTREProAdapter::InitializeDefaults()
{
	m_controller->ResetToDefaults();
}

string DragonControlToCTREProAdapter::GetErrorPrompt() const
{
	auto prompt = string("CTRE CAN motor controller ");
	prompt += to_string(m_controller->GetID());
	return prompt;
}

void DragonControlToCTREProAdapter::SetPeakAndNominalValues(std::string networkTableName,
															const ControlData &controlInfo)
{
	// TODO:  Implement Phoenix Pro methods
}

void DragonControlToCTREProAdapter::SetMaxVelocityAcceleration(string networkTableName,
															   const ControlData &controlInfo)
{
	// TODO:  Implement Phoenix Pro methods
}

void DragonControlToCTREProAdapter::SetPIDConstants(std::string networkTableName,
													int controllerSlot,
													const ControlData &controlInfo)
{
	// TODO:  Implement Phoenix Pro methods
}