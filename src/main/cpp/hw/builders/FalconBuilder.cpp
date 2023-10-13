
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

#include "hw/builders/FalconBuilder.h"
#include "hw/DragonTalonFX.h"

#include "ctre/phoenixpro/TalonFX.hpp"

using ctre::phoenix::motorcontrol::FeedbackDevice;
using ctre::phoenix::motorcontrol::RemoteFeedbackDevice;
using ctre::phoenixpro::configs::CurrentLimitsConfigs;
using ctre::phoenixpro::configs::MotorOutputConfigs;
using ctre::phoenixpro::configs::TalonFXConfiguration;
using ctre::phoenixpro::hardware::TalonFX;
using ctre::phoenixpro::signals::FeedbackSensorSourceValue;
using ctre::phoenixpro::signals::InvertedValue;
using ctre::phoenixpro::signals::NeutralModeValue;
using std::string;

// FalconBuilder::FalconBuilder(DragonTalonFX* existingFalcon)
//{
//     m_networkTableName =
// }
void FalconBuilder::SetNetworkTableName(string networkTableName)
{
    m_networkTableName = networkTableName;
}
void FalconBuilder::SetUsage(MotorControllerUsage::MOTOR_CONTROLLER_USAGE usage)
{
    m_usage = usage;
}
void FalconBuilder::SetIDs(int canID, int pdpID)
{
    m_canID = canID;
    m_pdpID = pdpID;
}
void FalconBuilder::SetIDs(int canID, int pdpID, int followID)
{
    m_canID = canID;
    m_pdpID = pdpID;
    m_followId = followID;
}

void FalconBuilder::SetCANBusName(string canBusName)
{
    m_canBusName = canBusName;
}
void FalconBuilder::SetMotorConfigs(InvertedValue inverted,
                                    NeutralModeValue mode,
                                    double deadbandPercent,
                                    double peakForwardDutyCycle,
                                    double peakReverseDutyCycle)
{
    m_inverted = inverted;
    m_brakeMode = mode;
    m_deadbandPercent = deadbandPercent;
    m_peakReverseDutyCycle = peakReverseDutyCycle;
    m_peakForwardDutyCycle = peakForwardDutyCycle;
    m_motorConfigSet = true;
}

void FalconBuilder::SetCurrentLimits(bool enableStatorCurrentLimit,
                                     units::current::ampere_t statorCurrentLimit,
                                     bool enableSupplyCurrentLimit,
                                     units::current::ampere_t supplyCurrentLimit,
                                     units::current::ampere_t supplyCurrentThreshold,
                                     units::time::second_t supplyTimeThreshold)
{
    m_enableStatorCurrentLimiting = enableStatorCurrentLimit;
    m_statorCurrentLimit = statorCurrentLimit;
    m_enableSupplyCurrentLimiting = enableSupplyCurrentLimit;
    m_supplyContinuousCurrentLimit = supplyCurrentLimit;
    m_supplyPeakCurrentLimit = supplyCurrentThreshold;
    m_supplyCurrentDuration = supplyTimeThreshold;
    m_currentSet = true;
}

void FalconBuilder::SetVoltageConfigs(units::voltage::volt_t peakForwardVoltage,
                                      units::voltage::volt_t peakReverseVoltage,
                                      units::time::second_t supplyVoltageTime)
{
    m_forwardVoltage = peakForwardVoltage;
    m_reverseVoltage = peakReverseVoltage;
    m_voltageTime = supplyVoltageTime;
    m_voltageSet = true;
}

void FalconBuilder::SetTorqueConfigs(units::current::ampere_t peakForwardTorqueCurrent,
                                     units::current::ampere_t peakReverseTorqueCurrent,
                                     units::current::ampere_t torqueNeutralDeadband)
{
    m_peakForwardTorqueCurrent = peakForwardTorqueCurrent;
    m_peakReverseTorqueCurrent = peakReverseTorqueCurrent;
    m_torqueNeutralDeadband = torqueNeutralDeadband;
    m_torqueSet = true;
}

void FalconBuilder::SetFeedbackConfigs(units::angle::turn_t feedbackRotorOffset,
                                       FeedbackSensorSourceValue feedbackSensor,
                                       int remoteSensorID)
{
    m_feedbackRotorOffset = feedbackRotorOffset;
    m_feedbackSensor = feedbackSensor;
    m_remoteSensorID = remoteSensorID;
    m_feedbackSet = true;
}

void FalconBuilder::ResetToDefaults()
{

    m_motorConfigSet = false;
    m_currentSet = false;
    m_voltageSet = false;
    m_torqueSet = false;
    m_feedbackSet = false;
    m_networkTableName = std::string("");
    m_usage = MotorControllerUsage::MOTOR_CONTROLLER_USAGE::UNKNOWN_MOTOR_CONTROLLER_USAGE;
    m_canID = -1;
    m_pdpID = -1;
    m_followId = -1;
    m_brakeMode = ctre::phoenixpro::signals::NeutralModeValue::Brake;
    m_inverted = ctre::phoenixpro::signals::InvertedValue::CounterClockwise_Positive;
    m_peakForwardDutyCycle = 12;
    m_peakReverseDutyCycle = -12;
    m_deadbandPercent = 0;

    m_calcStruc.countsPerRev = 2048;
    m_calcStruc.gearRatio = 1.0;
    m_calcStruc.diameter = 1.0;
    m_calcStruc.countsPerInch = 1.0;
    m_calcStruc.countsPerDegree = 1.0;

    m_enableStatorCurrentLimiting = false;
    m_statorCurrentLimit = units::current::ampere_t(0.0);
    m_enableSupplyCurrentLimiting = false;
    m_supplyContinuousCurrentLimit = units::current::ampere_t(0.0);
    m_supplyPeakCurrentLimit = units::current::ampere_t(0.0);
    m_supplyCurrentDuration = units::time::second_t(0.0);

    m_reverseVoltage = units::voltage::volt_t(-12.0);
    m_forwardVoltage = units::voltage::volt_t(12.0);
    m_voltageTime = units::time::second_t(0.0);

    m_peakForwardTorqueCurrent = units::current::ampere_t(0.0);
    m_peakReverseTorqueCurrent = units::current::ampere_t(0.0);
    m_torqueNeutralDeadband = units::current::ampere_t(0.0);

    m_forwardLimitSwitch = false;
    m_forwardLimitSwitchNormallyOpen = false;

    m_reverseLimitSwitch = false;
    m_reverseLimitSwitchNormallyOpen = false;

    m_voltageCompensationDuration = 12.0;
    m_enableVoltagCompensation = false;
    m_canBusName = std::string("");
}

bool FalconBuilder::IsValid() const
{

    return true;
}

DragonTalonFX *FalconBuilder::Commit()
{
    if (IsValid())
    {
        // auto falcon = new ctre::phoenixpro::hardware::TalonFX(m_canID, m_canBusName);
        auto falcon = new DragonTalonFX(m_networkTableName,
                                       m_usage,
                                       m_canID,
                                       m_canBusName,
                                       m_pdpID);
        if (falcon != nullptr)
        {
            if (m_motorConfigSet)
            {
                falcon->ConfigMotorSettings(m_inverted,
                                            m_brakeMode,
                                            m_deadbandPercent,
                                            m_peakForwardDutyCycle,
                                            m_peakReverseDutyCycle);
            }

            if (m_currentSet)
            {
                falcon->SetCurrentLimits(m_enableStatorCurrentLimiting,
                                         m_statorCurrentLimit,
                                         m_enableSupplyCurrentLimiting,
                                         m_supplyContinuousCurrentLimit,
                                         m_supplyPeakCurrentLimit,
                                         m_supplyCurrentDuration);
            }
        }
        return falcon;
    }
    return nullptr;
}