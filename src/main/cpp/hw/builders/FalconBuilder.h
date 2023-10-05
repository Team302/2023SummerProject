
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

#pragma once
#include <string>

#include "units/angle.h"
#include "units/current.h"
#include "units/time.h"
#include "units/voltage.h"
#include "ctre/phoenixpro/signals/SpnEnums.hpp"
#include "ctre/phoenix/motorcontrol/FeedbackDevice.h"
#include "ctre/phoenix/motorcontrol/RemoteSensorSource.h"

#include "configs/usages/MotorControllerUsage.h"
#include "hw/DistanceAngleCalcStruc.h"
#include "hw/DragonFalcon.h"
#include "hw/interfaces/IDragonMotorController.h"

class FalconBuilder
{
public:
    FalconBuilder() = default;
    // FalconBuilder(DragonFalcon* existingFalcon);
    ~FalconBuilder() = default;

    void SetNetworkTableName(std::string networkTableName);
    void SetUsage(MotorControllerUsage::MOTOR_CONTROLLER_USAGE usage);
    void SetIDs(int canID, int PDPID);
    void SetIDs(int canID, int PDPID, int followID);
    void SetMotorConfigs(ctre::phoenixpro::signals::InvertedValue inverted,
                         ctre::phoenixpro::signals::NeutralModeValue mode,
                         double deadbandPercent,
                         double peakForwardDutyCycle,
                         double peakReverseDutyCycle);

    void SetCurrentLimits(bool enableStatorCurrentLimit,
                          units::current::ampere_t statorCurrentLimit,
                          bool enableSupplyCurrentLimit,
                          units::current::ampere_t supplyCurrentLimit,
                          units::current::ampere_t supplyCurrentThreshold,
                          units::time::second_t supplyTimeThreshold);

    void SetVoltageConfigs(units::voltage::volt_t peakForwardVoltage,
                           units::voltage::volt_t peakReverseVoltage,
                           units::time::second_t supplyVoltageTime);

    void SetTorqueConfigs(units::current::ampere_t peakForwardTorqueCurrent,
                          units::current::ampere_t peakReverseTorqueCurrent,
                          units::current::ampere_t torqueNeutralDeadband);

    void SetFeedbackConfigs(units::angle::turn_t feedbackRotorOffset,
                            ctre::phoenixpro::signals::FeedbackSensorSourceValue feedbackSensor,
                            int remoteSensorID);
    void SetCANBusName(std::string canbus);

    void ResetToDefaults();

    bool IsValid() const;

    DragonFalcon *Commit();

private:
    bool m_voltageSet = false;
    bool m_torqueSet = false;
    bool m_feedbackSet = false;

    std::string m_networkTableName;
    MotorControllerUsage::MOTOR_CONTROLLER_USAGE m_usage;
    int m_canID = -1;
    int m_pdpID = -1;
    int m_followId = -1;

    bool m_motorConfigSet = false;
    ctre::phoenixpro::signals::NeutralModeValue m_brakeMode = ctre::phoenixpro::signals::NeutralModeValue::Brake;
    ctre::phoenixpro::signals::InvertedValue m_inverted = ctre::phoenixpro::signals::InvertedValue::CounterClockwise_Positive;
    double m_peakForwardDutyCycle;
    double m_peakReverseDutyCycle;
    double m_deadbandPercent;

    DistanceAngleCalcStruc m_calcStruc;

    bool m_currentSet = false;
    bool m_enableStatorCurrentLimiting = false;
    units::current::ampere_t m_statorCurrentLimit = units::current::ampere_t(0.0);
    bool m_enableSupplyCurrentLimiting = false;
    units::current::ampere_t m_supplyContinuousCurrentLimit = units::current::ampere_t(0.0);
    units::current::ampere_t m_supplyPeakCurrentLimit = units::current::ampere_t(0.0);
    units::time::second_t m_supplyCurrentDuration = units::time::second_t(0.0);

    units::voltage::volt_t m_reverseVoltage = units::voltage::volt_t(-12.0);
    units::voltage::volt_t m_forwardVoltage = units::voltage::volt_t(12.0);
    units::time::second_t m_voltageTime = units::time::second_t(0.0);

    units::current::ampere_t m_peakForwardTorqueCurrent = units::current::ampere_t(0.0);
    units::current::ampere_t m_peakReverseTorqueCurrent = units::current::ampere_t(0.0);
    units::current::ampere_t m_torqueNeutralDeadband = units::current::ampere_t(0.0);

    units::angle::turn_t m_feedbackRotorOffset = units::angle::turn_t(0.0);
    ctre::phoenixpro::signals::FeedbackSensorSourceValue m_feedbackSensor = ctre::phoenixpro::signals::FeedbackSensorSourceValue::FusedCANcoder;
    int m_remoteSensorID = -1;

    bool m_forwardLimitSwitch = false;
    bool m_forwardLimitSwitchNormallyOpen = false;

    bool m_reverseLimitSwitch = false;
    bool m_reverseLimitSwitchNormallyOpen = false;

    double m_voltageCompensationDuration = 12.0;
    bool m_enableVoltagCompensation = false;

    IDragonMotorController::MOTOR_TYPE m_motorType = IDragonMotorController::FALCON500;
    std::string m_canBusName = std::string("");
};
