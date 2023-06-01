//====================================================================================================================================================
// IDragonAnglePositionSensor.h
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
#include <hw/interfaces/IDragonControlToVendorControlAdapter.h>
#include <hw/ctreadapters/DragonControlToCTREAdapter.h>
#include <hw/ctreadapters/DragonPercentOutputToCTREAdapter.h>
#include <mechanisms/controllers/ControlData.h>
#include <mechanisms/controllers/ControlModes.h>
#include <utils/logging/Logger.h>

// Third Party Includes
#include <ctre/phoenix/motorcontrol/ControlMode.h>
#include <ctre/phoenix/motorcontrol/can/WPI_BaseMotorController.h>
#include <ctre/phoenix/ErrorCode.h>

using namespace std;
using namespace ctre::phoenix;
using namespace ctre::phoenix::motorcontrol;
using namespace ctre::phoenix::motorcontrol::can;

DragonControlToCTREAdapter::DragonControlToCTREAdapter(
	std::string networkTableName,
	int controllerSlot,
	ControlData *controlInfo,
	DistanceAngleCalcStruc calcStruc,
	WPI_BaseMotorController *controller) : IDragonControlToVendorControlAdapter(),
										   m_networkTableName(networkTableName),
										   m_controllerSlot(controllerSlot),
										   m_controlData(controlInfo),
										   m_calcStruc(calcStruc),
										   m_controller(controller)
{
	SetPeakAndNominalValues(networkTableName, controlInfo);

	if (controlInfo->GetMode() == ControlModes::CONTROL_TYPE::POSITION_ABSOLUTE ||
		controlInfo->GetMode() == ControlModes::CONTROL_TYPE::POSITION_DEGREES ||
		controlInfo->GetMode() == ControlModes::CONTROL_TYPE::POSITION_DEGREES_ABSOLUTE ||
		controlInfo->GetMode() == ControlModes::CONTROL_TYPE::POSITION_INCH ||
		controlInfo->GetMode() == ControlModes::CONTROL_TYPE::VELOCITY_DEGREES ||
		controlInfo->GetMode() == ControlModes::CONTROL_TYPE::VELOCITY_INCH ||
		controlInfo->GetMode() == ControlModes::CONTROL_TYPE::VELOCITY_RPS ||
		controlInfo->GetMode() == ControlModes::CONTROL_TYPE::VOLTAGE ||
		controlInfo->GetMode() == ControlModes::CONTROL_TYPE::CURRENT ||
		controlInfo->GetMode() == ControlModes::CONTROL_TYPE::TRAPEZOID)
	{
		SetPIDConstants(networkTableName, controllerSlot, controlInfo);
	}

	if ( // controlInfo->GetMode() == ControlModes::CONTROL_TYPE::POSITION_ABSOLUTE ||
		controlInfo->GetMode() == ControlModes::CONTROL_TYPE::POSITION_DEGREES_ABSOLUTE ||
		controlInfo->GetMode() == ControlModes::CONTROL_TYPE::TRAPEZOID)
	{
		SetMaxVelocityAcceleration(networkTableName, controlInfo);
	}
}

void DragonControlToCTREAdapter::InitializeDefaults()
{
	if (m_controller != nullptr)
	{
		auto error = m_controller->ConfigFactoryDefault();
		if (error != ErrorCode::OKAY)
		{
			Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, GetErrorPrompt(), string("ConfigFactoryDefault"), string("error"));
		}

		m_controller->SetNeutralMode(NeutralMode::Brake);

		error = m_controller->ConfigNeutralDeadband(0.01, 0);
		if (error != ErrorCode::OKAY)
		{
			Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, GetErrorPrompt(), string("ConfigNeutralDeadband"), string("error"));
		}
		error = m_controller->ConfigNominalOutputForward(0.0, 0);
		if (error != ErrorCode::OKAY)
		{
			Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, GetErrorPrompt(), string("ConfigNominalOutputForward"), string("error"));
		}
		error = m_controller->ConfigNominalOutputReverse(0.0, 0);
		if (error != ErrorCode::OKAY)
		{
			Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, GetErrorPrompt(), string("ConfigNominalOutputReverse"), string("error"));
		}
		error = m_controller->ConfigOpenloopRamp(0.0, 0);
		if (error != ErrorCode::OKAY)
		{
			Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, GetErrorPrompt(), string("ConfigOpenloopRamp"), string("error"));
		}
		error = m_controller->ConfigPeakOutputForward(1.0, 0);
		if (error != ErrorCode::OKAY)
		{
			Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, GetErrorPrompt(), string("ConfigPeakOutputForward"), string("error"));
		}
		error = m_controller->ConfigPeakOutputReverse(-1.0, 0);
		if (error != ErrorCode::OKAY)
		{
			Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, GetErrorPrompt(), string("ConfigPeakOutputReverse"), string("error"));
		}
	}
}

string DragonControlToCTREAdapter::GetErrorPrompt() const
{
	auto prompt = string("CTRE CAN motor controller ");
	prompt += to_string(m_controller->GetDeviceID());
	return prompt;
}

void DragonControlToCTREAdapter::SetPeakAndNominalValues(
	std::string networkTableName,
	ControlData *controlInfo)
{
	auto peak = controlInfo->GetPeakValue();
	auto error = m_controller->ConfigPeakOutputForward(peak);
	if (error != ErrorCode::OKAY)
	{
		Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, networkTableName, GetErrorPrompt(), string("ConfigPeakOutputForward error"));
	}
	error = m_controller->ConfigPeakOutputReverse(-1.0 * peak);
	if (error != ErrorCode::OKAY)
	{
		Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, networkTableName, GetErrorPrompt(), string("ConfigPeakOutputReverse error"));
	}

	auto nominal = controlInfo->GetNominalValue();
	error = m_controller->ConfigNominalOutputForward(nominal);
	if (error != ErrorCode::OKAY)
	{
		Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, networkTableName, GetErrorPrompt(), string("ConfigNominalOutputForward error"));
	}
	error = m_controller->ConfigNominalOutputReverse(-1.0 * nominal);
	if (error != ErrorCode::OKAY)
	{
		Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, networkTableName, GetErrorPrompt(), string("ConfigNominalOutputReverse error"));
	}
}

void DragonControlToCTREAdapter::SetMaxVelocityAcceleration(
	std::string networkTableName,
	ControlData *controlInfo)
{
	auto error = m_controller->ConfigMotionAcceleration(controlInfo->GetMaxAcceleration());
	if (error != ErrorCode::OKAY)
	{
		Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, networkTableName, GetErrorPrompt(), string("ConfigMotionAcceleration error"));
	}
	error = m_controller->ConfigMotionCruiseVelocity(controlInfo->GetCruiseVelocity(), 0);
	if (error != ErrorCode::OKAY)
	{
		Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, networkTableName, GetErrorPrompt(), string("ConfigMotionCruiseVelocity error"));
	}
}

void DragonControlToCTREAdapter::SetPIDConstants(
	std::string networkTableName,
	int controllerSlot,
	ControlData *controlInfo)
{
	auto error = m_controller->Config_kP(controllerSlot, controlInfo->GetP());
	if (error != ErrorCode::OKAY)
	{
		m_controller->Config_kP(controllerSlot, controlInfo->GetP());
		Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, networkTableName, GetErrorPrompt(), string("Config_kP error"));
	}
	error = m_controller->Config_kI(controllerSlot, controlInfo->GetI());
	if (error != ErrorCode::OKAY)
	{
		m_controller->Config_kI(controllerSlot, controlInfo->GetI());
		Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, networkTableName, GetErrorPrompt(), string("Config_kI error"));
	}
	error = m_controller->Config_kD(controllerSlot, controlInfo->GetD());
	if (error != ErrorCode::OKAY)
	{
		m_controller->Config_kD(controllerSlot, controlInfo->GetD());
		Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, networkTableName, GetErrorPrompt(), string("Config_kD error"));
	}
	error = m_controller->Config_kF(controllerSlot, controlInfo->GetF());
	if (error != ErrorCode::OKAY)
	{
		m_controller->Config_kF(controllerSlot, controlInfo->GetF());
		Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, networkTableName, GetErrorPrompt(), string("Config_kF error"));
	}
	error = m_controller->SelectProfileSlot(controllerSlot, 0);
	if (error != ErrorCode::OKAY)
	{
		Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, networkTableName, GetErrorPrompt(), string("SelectProfileSlot error"));
	}
}