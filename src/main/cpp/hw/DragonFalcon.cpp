
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
#include <hw/DragonFalcon.h>
#include <hw/factories/PDPFactory.h>
#include "hw/factories/DragonControlToCTREAdapterFactory.h"
#include "configs/usages/MotorControllerUsage.h"
#include "utils/logging/Logger.h"
#include "utils/ConversionUtils.h"
#include "hw/ctreadapters/DragonControlToCTREAdapter.h"

// Third Party Includes
#include "ctre/phoenixpro/TalonFX.hpp"
#include "ctre/phoenixpro/controls/Follower.hpp"
#include "ctre/phoenixpro/configs/Configs.hpp"
// #include <ctre/phoenix/motorcontrol/can/TalonFX.h>
// #include <ctre/phoenix/motorcontrol/SupplyCurrentLimitConfiguration.h>

using namespace frc;
using ctre::phoenixpro::configs::CurrentLimitsConfigs;

// using namespace std;
// using namespace ctre::phoenix;
// using namespace ctre::phoenix::motorcontrol;
// using namespace ctre::phoenix::motorcontrol::can;

using ctre::phoenixpro::configs::CANcoderConfiguration;
using ctre::phoenixpro::configs::ClosedLoopRampsConfigs;
using ctre::phoenixpro::configs::HardwareLimitSwitchConfigs;
using ctre::phoenixpro::configs::MotorOutputConfigs;
using ctre::phoenixpro::configs::OpenLoopRampsConfigs;
using ctre::phoenixpro::configs::Slot0Configs;
using ctre::phoenixpro::configs::Slot1Configs;
using ctre::phoenixpro::configs::Slot2Configs;
using ctre::phoenixpro::configs::VoltageConfigs;
using ctre::phoenixpro::controls::Follower;
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

DragonFalcon::DragonFalcon(string networkTableName,
						   MotorControllerUsage::MOTOR_CONTROLLER_USAGE deviceType,
						   int deviceID,
						   string canBusName,
						   int pdpID) : m_networkTableName(networkTableName),
										m_talon(new TalonFX(deviceID, canBusName)),
										m_controller(),
										m_type(deviceType),
										m_id(deviceID),
										m_pdp(pdpID),
										m_calcStruc(),
										m_motorType(MOTOR_TYPE::FALCON500),
										m_inverted(false)
{
	m_talon->GetConfigurator().Apply(TalonFXConfiguration{}); // reset to factory default
}

void DragonFalcon::ConfigHWLimitSW(bool enableForward,
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
	if (m_talon != nullptr)
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
		m_talon->GetConfigurator().Apply(hwswitch);
	}
}

void DragonFalcon::ConfigMotorSettings(InvertedValue inverted,
									   NeutralModeValue mode,
									   double deadbandPercent,
									   double peakForwardDutyCycle,
									   double peakReverseDutyCycle)
{
	if (m_talon != nullptr)
	{
		MotorOutputConfigs motorconfig{};
		motorconfig.Inverted = inverted;
		motorconfig.NeutralMode = mode;
		motorconfig.PeakForwardDutyCycle = peakForwardDutyCycle;
		motorconfig.PeakReverseDutyCycle = peakReverseDutyCycle;
		motorconfig.DutyCycleNeutralDeadband = deadbandPercent;
		m_talon->GetConfigurator().Apply(motorconfig);
	}
}
void DragonFalcon::SetPIDConstants(int slot, double p, double i, double d, double f)
{
	if (m_talon != nullptr)
	{
		if (slot == 0)
		{
			Slot0Configs slot0;
			slot0.kP = p;
			slot0.kI = i;
			slot0.kD = d;
			slot0.kV = f;
			m_talon->GetConfigurator().Apply(slot0);
		}
		else if (slot == 1)
		{
			Slot1Configs slot1;
			slot1.kP = p;
			slot1.kI = i;
			slot1.kD = d;
			slot1.kV = f;
			m_talon->GetConfigurator().Apply(slot1);
		}
		else if (slot == 2)
		{
			Slot2Configs slot2;
			slot2.kP = p;
			slot2.kI = i;
			slot2.kD = d;
			slot2.kV = f;
			m_talon->GetConfigurator().Apply(slot2);
		}
	}
}
double DragonFalcon::GetRotations() const
{
	auto &possig = m_talon->GetPosition();
	possig.Refresh();

	auto &velsig = m_talon->GetVelocity();
	auto turnsPerSec = velsig.GetValue();

	auto rotations = possig.GetValue() + turnsPerSec * possig.GetTimestamp().GetLatency();
	return rotations.to<double>();
}

double DragonFalcon::GetRPS() const
{
	auto &velsig = m_talon->GetVelocity();
	auto turnsPerSec = velsig.GetValue();
	return turnsPerSec.to<double>();
}

MotorController *DragonFalcon::GetSpeedController() const
{
	return m_talon;
}

double DragonFalcon::GetCurrent() const
{
	auto pdp = PDPFactory::GetFactory()->GetPDP();
	if (pdp != nullptr)
	{
		return pdp->GetCurrent(m_pdp);
	}
	return 0.0;
}

/**
void DragonFalcon::UpdateFramePeriods
(
	ctre::phoenix::motorcontrol::StatusFrameEnhanced	frame,
	uint8_t												milliseconds
)
{
	m_talon->SetStatusFramePeriod( frame, milliseconds, 0 );
}
**/
void DragonFalcon::SetFramePeriodPriority(MOTOR_PRIORITY priority)
{
	return;
	/**
	switch ( priority )
	{
		case HIGH:
			UpdateFramePeriods( StatusFrameEnhanced::Status_1_General, 10 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_2_Feedback0, 20 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_3_Quadrature, 100 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_4_AinTempVbat, 150 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_8_PulseWidth, 120 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_10_Targets, 120 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_11_UartGadgeteer, 120 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_12_Feedback1, 120 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_13_Base_PIDF0, 120 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_14_Turn_PIDF1, 120 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_15_FirmareApiStatus, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_Brushless_Current, 200 );
			break;

		case MEDIUM:
			UpdateFramePeriods( StatusFrameEnhanced::Status_1_General, 60 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_2_Feedback0, 120 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_3_Quadrature, 150 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_4_AinTempVbat, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_8_PulseWidth, 150 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_10_Targets, 150 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_11_UartGadgeteer, 150 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_12_Feedback1, 150 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_13_Base_PIDF0, 150 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_14_Turn_PIDF1, 150 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_15_FirmareApiStatus, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_Brushless_Current, 200 );
			break;

		case LOW:
			UpdateFramePeriods( StatusFrameEnhanced::Status_1_General, 120 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_2_Feedback0, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_3_Quadrature, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_4_AinTempVbat, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_8_PulseWidth, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_10_Targets, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_11_UartGadgeteer, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_12_Feedback1, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_13_Base_PIDF0, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_14_Turn_PIDF1, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_15_FirmareApiStatus, 200 );
			UpdateFramePeriods( StatusFrameEnhanced::Status_Brushless_Current, 200 );
			break;

		default:
		break;

	}
	**/
}

void DragonFalcon::Set(double value)
{
	m_controller[0]->Set(value);
}

void DragonFalcon::SetRotationOffset(double rotations)
{
	//	double newRotations = -rotations + DragonFalcon::GetRotations();
	//	m_tickOffset += (int) (newRotations * m_calcStruc.countsPerRev / m_calcStruc.gearRatio);
}

void DragonFalcon::SetVoltageRamping(double ramping, double rampingClosedLoop)
{
	if (m_talon != nullptr)
	{
		ClosedLoopRampsConfigs configs{};
		m_talon->GetConfigurator().Refresh(configs);
		configs.VoltageClosedLoopRampPeriod = rampingClosedLoop;
		m_talon->GetConfigurator().Apply(configs);

		if (rampingClosedLoop > 0.0)
		{
			OpenLoopRampsConfigs oconfigs{};
			m_talon->GetConfigurator().Refresh(oconfigs);
			oconfigs.VoltageOpenLoopRampPeriod = ramping;
			m_talon->GetConfigurator().Apply(oconfigs);
		}
	}
}

void DragonFalcon::EnableBrakeMode(bool enabled)
{
	if (m_talon != nullptr)
	{
		MotorOutputConfigs motorconfig{};
		m_talon->GetConfigurator().Refresh(motorconfig);
		if (enabled)
		{
			motorconfig.NeutralMode = NeutralModeValue::Brake;
		}
		else
		{
			motorconfig.NeutralMode = NeutralModeValue::Coast;
		}
		m_talon->GetConfigurator().Apply(motorconfig);
	}
}

void DragonFalcon::Invert(bool inverted)
{
	m_inverted = inverted;
	m_talon->SetInverted(inverted);
}

void DragonFalcon::SetSensorInverted(bool inverted)
{
}

MotorControllerUsage::MOTOR_CONTROLLER_USAGE DragonFalcon::GetType() const
{
	return m_type;
}

int DragonFalcon::GetID() const
{
	return m_id;
}

void DragonFalcon::SetCurrentLimits(bool enableStatorCurrentLimit,
									units::current::ampere_t statorCurrentLimit,
									bool enableSupplyCurrentLimit,
									units::current::ampere_t supplyCurrentLimit,
									units::current::ampere_t supplyCurrentThreshold,
									units::time::second_t supplyTimeThreshold)
{
	if (m_talon != nullptr)
	{
		CurrentLimitsConfigs currconfigs{};
		currconfigs.StatorCurrentLimit = statorCurrentLimit.to<double>();
		currconfigs.StatorCurrentLimitEnable = enableSupplyCurrentLimit;
		currconfigs.SupplyCurrentLimit = supplyCurrentLimit.to<double>();
		currconfigs.SupplyCurrentLimitEnable = enableSupplyCurrentLimit;
		m_talon->GetConfigurator().Apply(currconfigs);
	}
}

void DragonFalcon::EnableCurrentLimiting(bool enabled)
{
	if (m_talon != nullptr)
	{
		CurrentLimitsConfigs configs{};
		m_talon->GetConfigurator().Refresh(configs);
		configs.StatorCurrentLimitEnable = enabled;
		configs.SupplyCurrentLimitEnable = enabled;
		m_talon->GetConfigurator().Apply(configs);
	}
}

void DragonFalcon::SetAsFollowerMotor(int masterCANID // <I> - master motor
)
{
	if (m_talon != nullptr)
	{
		m_talon->SetControl(Follower(masterCANID, false));
	}
}

/// @brief  Set the control constants (e.g. PIDF values).
/// @param [in] int             slot - hardware slot to use
/// @param [in] ControlData*    pid - the control constants
/// @return void
void DragonFalcon::SetControlConstants(int slot, ControlData *controlInfo)
{
	if (m_talon != nullptr)
	{
		// SetPeakAndNominalValues(m_networkTableName, controlInfo);
		// SetPIDConstants(m_networkTableName, m_controllerSlot, controlInfo);
	}
	// delete m_controller[slot];
	// m_controller[slot] = DragonControlToCTREAdapterFactory::GetFactory()->CreateAdapter(m_networkTableName, slot, controlInfo, m_calcStruc, m_talon);
}

void DragonFalcon::SetRemoteSensor(int canID,
								   ctre::phoenix::motorcontrol::RemoteSensorSource deviceType)
{
	if (m_talon != nullptr)
	{
		TalonFXConfiguration configs{};
		configs.Feedback.FeedbackRemoteSensorID = canID;
		configs.Feedback.FeedbackSensorSource = FeedbackSensorSourceValue::FusedCANcoder;

		m_talon->GetConfigurator().Apply(configs);
	}
}
void DragonFalcon::FuseCancoder(DragonCanCoder &cancoder,
								double sensorToMechanismRatio,
								double rotorToSensorRatio)
{
	if (m_talon != nullptr)
	{
		// update cancoder definition
		cancoder.SetRange(AbsoluteSensorRangeValue::Signed_PlusMinusHalf);
		cancoder.SetDirection(SensorDirectionValue::CounterClockwise_Positive);

		TalonFXConfiguration configs{};
		m_talon->GetConfigurator().Refresh(configs);
		configs.Feedback.FeedbackRemoteSensorID = cancoder.GetCanId();
		configs.Feedback.FeedbackSensorSource = FeedbackSensorSourceValue::FusedCANcoder;
		configs.Feedback.SensorToMechanismRatio = sensorToMechanismRatio;
		configs.Feedback.RotorToSensorRatio = rotorToSensorRatio;

		m_talon->GetConfigurator().Apply(configs);
	}
}

void DragonFalcon::SetDiameter(double diameter)
{
	m_calcStruc.diameter = diameter;
}

void DragonFalcon::SetVoltage(units::volt_t output)
{
	if (m_talon != nullptr)
	{
		m_talon->SetVoltage(output);
	}
}

bool DragonFalcon::IsForwardLimitSwitchClosed() const
{
	if (m_talon != nullptr)
	{
		auto signal = m_talon->GetForwardLimit();
		return signal.GetValue() == ForwardLimitValue::ClosedToGround;
	}
	return false;
}

bool DragonFalcon::IsReverseLimitSwitchClosed() const
{
	if (m_talon != nullptr)
	{
		auto signal = m_talon->GetReverseLimit();
		return signal.GetValue() == ReverseLimitValue::ClosedToGround;
	}
	return false;
}

void DragonFalcon::EnableDisableLimitSwitches(bool enable)
{
	if (m_talon != nullptr)
	{
		HardwareLimitSwitchConfigs hwswitch{};
		m_talon->GetConfigurator().Refresh(hwswitch);
		hwswitch.ForwardLimitEnable = enable;
		m_talon->GetConfigurator().Apply(hwswitch);
	}
}

void DragonFalcon::EnableVoltageCompensation(double fullvoltage)
{
	// m_talon->ConfigVoltageCompSaturation(fullvoltage);
	// m_talon->EnableVoltageCompensation(true);
}

void DragonFalcon::SetSelectedSensorPosition(double initialPosition)
{
	if (m_talon != nullptr)
	{
		m_talon->SetRotorPosition(units::angle::degree_t(initialPosition));
	}
}

double DragonFalcon::GetCountsPerInch() const
{
	return m_calcStruc.countsPerInch;
}
double DragonFalcon::GetCountsPerDegree() const
{
	return m_calcStruc.countsPerDegree;
}

/**
ControlModes::CONTROL_TYPE DragonFalcon::GetControlMode() const
{
	return m_controlMode;
}
**/

double DragonFalcon::GetCounts() const
{
	if (m_talon != nullptr)
	{
		return GetRotations() * 2048;
	}
	return 0.0;
}

IDragonMotorController::MOTOR_TYPE DragonFalcon::GetMotorType() const
{
	return m_motorType;
}
