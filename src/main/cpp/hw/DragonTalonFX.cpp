
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
#include <memory>
#include <string>

// FRC includes
#include <networktables/NetworkTableInstance.h>
#include <networktables/NetworkTable.h>
#include <networktables/NetworkTableEntry.h>
#include <frc/PowerDistribution.h>
#include <frc/motorcontrol/MotorController.h>

// Team 302 includes
#include "hw/DistanceAngleCalcStruc.h"
#include "hw/interfaces/IDragonMotorController.h"
#include "hw/DragonTalonFX.h"
#include "hw/factories/PDPFactory.h"
#include "hw/factories/DragonControlToCTREProAdapterFactory.h"
#include "configs/usages/MotorControllerUsage.h"
#include "utils/logging/Logger.h"
#include "utils/ConversionUtils.h"
#include "hw/ctreadapters/pro/DragonControlToCTREProAdapter.h"

// Third Party Includes
#include "ctre/phoenixpro/TalonFX.hpp"
#include "ctre/phoenixpro/controls/Follower.hpp"
#include "ctre/phoenixpro/configs/Configs.hpp"

using namespace frc;
using ctre::phoenixpro::configs::CurrentLimitsConfigs;

using ctre::phoenixpro::configs::CANcoderConfiguration;
using ctre::phoenixpro::configs::ClosedLoopRampsConfigs;
using ctre::phoenixpro::configs::HardwareLimitSwitchConfigs;
using ctre::phoenixpro::configs::MotorOutputConfigs;
using ctre::phoenixpro::configs::OpenLoopRampsConfigs;
using ctre::phoenixpro::configs::Slot0Configs;
using ctre::phoenixpro::configs::Slot1Configs;
using ctre::phoenixpro::configs::Slot2Configs;
using ctre::phoenixpro::configs::VoltageConfigs;
using ctre::phoenixpro::controls::ControlRequest;
using ctre::phoenixpro::controls::DutyCycleOut;
using ctre::phoenixpro::controls::EmptyControl;
using ctre::phoenixpro::controls::Follower;
using ctre::phoenixpro::controls::MotionMagicDutyCycle;
using ctre::phoenixpro::controls::MotionMagicTorqueCurrentFOC;
using ctre::phoenixpro::controls::MotionMagicVoltage;
using ctre::phoenixpro::controls::PositionDutyCycle;
using ctre::phoenixpro::controls::PositionTorqueCurrentFOC;
using ctre::phoenixpro::controls::PositionVoltage;
using ctre::phoenixpro::controls::TorqueCurrentFOC;
using ctre::phoenixpro::controls::VelocityDutyCycle;
using ctre::phoenixpro::controls::VelocityTorqueCurrentFOC;
using ctre::phoenixpro::controls::VelocityVoltage;
using ctre::phoenixpro::signals::AbsoluteSensorRangeValue;
using ctre::phoenixpro::signals::FeedbackSensorSourceValue;
using ctre::phoenixpro::signals::ForwardLimitSourceValue;
using ctre::phoenixpro::signals::ForwardLimitTypeValue;
using ctre::phoenixpro::signals::ForwardLimitValue;
using ctre::phoenixpro::signals::InvertedValue;
using ctre::phoenixpro::signals::NeutralModeValue;
using ctre::phoenixpro::signals::ReverseLimitSourceValue;
using ctre::phoenixpro::signals::ReverseLimitTypeValue;
using ctre::phoenixpro::signals::ReverseLimitValue;
using ctre::phoenixpro::signals::SensorDirectionValue;

using ctre::phoenixpro::configs::TalonFXConfiguration;
using ctre::phoenixpro::hardware::TalonFX;
using std::shared_ptr;
using std::string;
using std::to_string;

DragonTalonFX::DragonTalonFX(string networkTableName,
							 RobotElementNames::MOTOR_CONTROLLER_USAGE deviceType,
							 int deviceID,
							 string canBusName) : m_networkTableName(networkTableName),
												  m_type(deviceType),
												  m_talon(TalonFX(deviceID, canBusName)),
												  m_calcStruc(),
												  m_inverted(false)
{
	ResetToDefaults();
	auto factory = DragonControlToCTREProAdapterFactory::GetFactory();
	for (auto i = 1; i < 4; ++i)
	{
		m_controller[i] = factory->CreateAdapter(networkTableName,
												 i,
												 ControlData(),
												 m_calcStruc,
												 m_talon);

		m_controller[i]->InitializeDefaults();
	}
}

// DragonTalonFX::DragonTalonFX(const DragonTalonFX &other)
//{
// }
void DragonTalonFX::ConfigHWLimitSW(bool enableForward,
									int remoteForwardSensorID,
									bool forwardResetPosition,
									double forwardPosition,
									ForwardLimitSourceValue forwardType,
									ForwardLimitTypeValue forwardOpenClose,
									bool enableReverse,
									int remoteReverseSensorID,
									bool reverseResetPosition,
									double reversePosition,
									ReverseLimitSourceValue revType,
									ReverseLimitTypeValue revOpenClose)
{
	HardwareLimitSwitchConfigs hwswitch{};
	hwswitch.ForwardLimitEnable = enableForward;
	hwswitch.ForwardLimitRemoteSensorID = remoteForwardSensorID;
	hwswitch.ForwardLimitAutosetPositionEnable = forwardResetPosition;
	hwswitch.ForwardLimitAutosetPositionValue = forwardPosition;
	hwswitch.ForwardLimitSource = forwardType;
	hwswitch.ForwardLimitType = forwardOpenClose;

	hwswitch.ReverseLimitEnable = enableReverse;
	hwswitch.ReverseLimitRemoteSensorID = remoteReverseSensorID;
	hwswitch.ReverseLimitAutosetPositionEnable = reverseResetPosition;
	hwswitch.ReverseLimitAutosetPositionValue = reversePosition;
	hwswitch.ReverseLimitSource = revType;
	hwswitch.ReverseLimitType = revOpenClose;
	m_talon.GetConfigurator().Apply(hwswitch);
}

void DragonTalonFX::ConfigMotorSettings(InvertedValue inverted,
										NeutralModeValue mode,
										double deadbandPercent,
										double peakForwardDutyCycle,
										double peakReverseDutyCycle)
{
	m_inverted = inverted == InvertedValue::CounterClockwise_Positive;
	MotorOutputConfigs motorconfig{};
	motorconfig.Inverted = inverted;
	motorconfig.NeutralMode = mode;
	motorconfig.PeakForwardDutyCycle = peakForwardDutyCycle;
	motorconfig.PeakReverseDutyCycle = peakReverseDutyCycle;
	motorconfig.DutyCycleNeutralDeadband = deadbandPercent;
	m_talon.GetConfigurator().Apply(motorconfig);
}
void DragonTalonFX::SetPIDConstants(int slot, double p, double i, double d, double f)
{
	ControlData controlInfo{ControlModes::CONTROL_TYPE::VOLTAGE,
							ControlModes::CONTROL_RUN_LOCS::MOTOR_CONTROLLER,
							std::string("NewController"),
							p, i, d, f,
							ControlData::FEEDFORWARD_TYPE::DUTY_CYCLE,
							0.0, 1.0, 1.0, 1.0, 0.0, true};

	auto factory = DragonControlToCTREProAdapterFactory::GetFactory();
	delete m_controller[slot];
	m_controller[slot] = factory->CreateAdapter(m_networkTableName, slot, controlInfo, m_calcStruc, m_talon);
}
double DragonTalonFX::GetRotations()
{
	auto &possig = m_talon.GetPosition();
	possig.Refresh();

	auto &velsig = m_talon.GetVelocity();
	auto turnsPerSec = velsig.GetValue();

	auto rotations = possig.GetValue() + turnsPerSec * possig.GetTimestamp().GetLatency();
	return rotations.to<double>();
}

double DragonTalonFX::GetRPS()
{
	auto &velsig = m_talon.GetVelocity();
	auto turnsPerSec = velsig.GetValue();
	return turnsPerSec.to<double>();
}

double DragonTalonFX::GetCurrent()
{
	return m_talon.GetSupplyCurrent().GetValue().to<double>();
}

/**
void DragonTalonFX::UpdateFramePeriods
(
	ctre::phoenix::motorcontrol::StatusFrameEnhanced	frame,
	uint8_t												milliseconds
)
{
	m_talon.SetStatusFramePeriod( frame, milliseconds, 0 );
}
**/

void DragonTalonFX::Set(double value)
{
	m_controller[0]->Set(value);
}
void DragonTalonFX::Set(ControlRequest &control)
{
	m_talon.SetControl(control);
}

void DragonTalonFX::SetRotationOffset(double rotations)
{
	//	double newRotations = -rotations + DragonTalonFX::GetRotations();
	//	m_tickOffset += (int) (newRotations * m_calcStruc.countsPerRev / m_calcStruc.gearRatio);
}

void DragonTalonFX::SetVoltageRamping(double ramping, double rampingClosedLoop)
{
	ClosedLoopRampsConfigs configs{};
	m_talon.GetConfigurator().Refresh(configs);
	configs.VoltageClosedLoopRampPeriod = rampingClosedLoop;
	m_talon.GetConfigurator().Apply(configs);

	if (rampingClosedLoop > 0.0)
	{
		OpenLoopRampsConfigs oconfigs{};
		m_talon.GetConfigurator().Refresh(oconfigs);
		oconfigs.VoltageOpenLoopRampPeriod = ramping;
		m_talon.GetConfigurator().Apply(oconfigs);
	}
}

void DragonTalonFX::EnableBrakeMode(bool enabled)
{
	MotorOutputConfigs motorconfig{};
	m_talon.GetConfigurator().Refresh(motorconfig);
	if (enabled)
	{
		motorconfig.NeutralMode = NeutralModeValue::Brake;
	}
	else
	{
		motorconfig.NeutralMode = NeutralModeValue::Coast;
	}
	m_talon.GetConfigurator().Apply(motorconfig);
}

void DragonTalonFX::ResetToDefaults()
{
	m_talon.GetConfigurator().Apply(TalonFXConfiguration{}); // reset to factory default
	EnableBrakeMode(true);

	auto invVal = m_inverted ? InvertedValue::Clockwise_Positive : InvertedValue::CounterClockwise_Positive;
	ConfigMotorSettings(invVal, NeutralModeValue::Brake, 0.01, 1.0, -1.0);
}

void DragonTalonFX::Invert(bool inverted)
{
	m_inverted = inverted;
	m_talon.SetInverted(inverted);
}

void DragonTalonFX::SetSensorInverted(bool inverted)
{
}

RobotElementNames::MOTOR_CONTROLLER_USAGE DragonTalonFX::GetType() const
{
	return m_type;
}

int DragonTalonFX::GetID() const
{
	return m_talon.GetDeviceID();
}

void DragonTalonFX::SetCurrentLimits(bool enableStatorCurrentLimit,
									 units::current::ampere_t statorCurrentLimit,
									 bool enableSupplyCurrentLimit,
									 units::current::ampere_t supplyCurrentLimit,
									 units::current::ampere_t supplyCurrentThreshold,
									 units::time::second_t supplyTimeThreshold)
{
	CurrentLimitsConfigs currconfigs{};
	currconfigs.StatorCurrentLimit = statorCurrentLimit.to<double>();
	currconfigs.StatorCurrentLimitEnable = enableSupplyCurrentLimit;
	currconfigs.SupplyCurrentLimit = supplyCurrentLimit.to<double>();
	currconfigs.SupplyCurrentLimitEnable = enableSupplyCurrentLimit;
	m_talon.GetConfigurator().Apply(currconfigs);
}

void DragonTalonFX::EnableCurrentLimiting(bool enabled)
{
	CurrentLimitsConfigs configs{};
	m_talon.GetConfigurator().Refresh(configs);
	configs.StatorCurrentLimitEnable = enabled;
	configs.SupplyCurrentLimitEnable = enabled;
	m_talon.GetConfigurator().Apply(configs);
}

void DragonTalonFX::SetAsFollowerMotor(int masterCANID // <I> - master motor
)
{
	m_talon.SetControl(Follower(masterCANID, false));
}

/// @brief  Set the control constants (e.g. PIDF values).
/// @param [in] int             slot - hardware slot to use
/// @param [in] ControlData*    pid - the control constants
/// @return void
void DragonTalonFX::SetControlConstants(int slot, const ControlData &controlInfo)
{
	auto factory = DragonControlToCTREProAdapterFactory::GetFactory();
	delete m_controller[slot];
	m_controller[slot] = factory->CreateAdapter(m_networkTableName, slot, controlInfo, m_calcStruc, m_talon);
}

void DragonTalonFX::SetRemoteSensor(int canID,
									ctre::phoenix::motorcontrol::RemoteSensorSource deviceType)
{
	TalonFXConfiguration configs{};
	configs.Feedback.FeedbackRemoteSensorID = canID;
	configs.Feedback.FeedbackSensorSource = FeedbackSensorSourceValue::FusedCANcoder;

	m_talon.GetConfigurator().Apply(configs);
}
void DragonTalonFX::FuseCancoder(DragonCanCoder &cancoder,
								 double sensorToMechanismRatio,
								 double rotorToSensorRatio)
{
	// update cancoder definition
	cancoder.SetRange(AbsoluteSensorRangeValue::Signed_PlusMinusHalf);
	cancoder.SetDirection(SensorDirectionValue::CounterClockwise_Positive);

	TalonFXConfiguration configs{};
	m_talon.GetConfigurator().Refresh(configs);
	configs.Feedback.FeedbackRemoteSensorID = cancoder.GetCanId();
	configs.Feedback.FeedbackSensorSource = FeedbackSensorSourceValue::FusedCANcoder;
	configs.Feedback.SensorToMechanismRatio = sensorToMechanismRatio;
	configs.Feedback.RotorToSensorRatio = rotorToSensorRatio;

	m_talon.GetConfigurator().Apply(configs);
}

void DragonTalonFX::SetDiameter(double diameter)
{
	m_calcStruc.diameter = diameter;
}

void DragonTalonFX::SetVoltage(units::volt_t output)
{
	m_talon.SetVoltage(output);
}

bool DragonTalonFX::IsForwardLimitSwitchClosed()
{
	auto signal = m_talon.GetForwardLimit();
	return signal.GetValue() == ForwardLimitValue::ClosedToGround;
}

bool DragonTalonFX::IsReverseLimitSwitchClosed()
{
	auto signal = m_talon.GetReverseLimit();
	return signal.GetValue() == ReverseLimitValue::ClosedToGround;
}

void DragonTalonFX::EnableDisableLimitSwitches(bool enable)
{
	HardwareLimitSwitchConfigs hwswitch{};
	m_talon.GetConfigurator().Refresh(hwswitch);
	hwswitch.ForwardLimitEnable = enable;
	m_talon.GetConfigurator().Apply(hwswitch);
}

void DragonTalonFX::EnableVoltageCompensation(double fullvoltage)
{
	// m_talon.ConfigVoltageCompSaturation(fullvoltage);
	// m_talon.EnableVoltageCompensation(true);
}

void DragonTalonFX::SetSelectedSensorPosition(double initialPosition)
{
	m_talon.SetRotorPosition(units::angle::degree_t(initialPosition));
}

double DragonTalonFX::GetCountsPerInch() const
{
	return m_calcStruc.countsPerInch;
}
double DragonTalonFX::GetCountsPerDegree() const
{
	return m_calcStruc.countsPerDegree;
}

/**
ControlModes::CONTROL_TYPE DragonTalonFX::GetControlMode() const
{
	return m_controlMode;
}
**/

double DragonTalonFX::GetCounts()
{
	return GetRotations() * 2048;
}

IDragonMotorController::MOTOR_TYPE DragonTalonFX::GetMotorType() const
{
	return MOTOR_TYPE::FALCON500;
}
