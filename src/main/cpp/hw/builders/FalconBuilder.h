
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

#include "hw/DistanceAngleCalcStruc.h"
#include "hw/interfaces/IDragonMotorController.h"

class FalconBuilder
{
public:
    FalconBuilder() = default;
    ~FalconBuilder() = default;

    void SetNetworkTableName(std::string networkTableName);
    void SetUsage(std::string usage);
    void SetIDs(int canID, int PDPID);
    void SetIDs(int canID, int PDPID, int followID);
    void SetMotorConfigs(ctre::phoenixpro::signals::InvertedValue inverted,
                         ctre::phoenixpro::signals::NeutralModeValue mode,
                         double deadbandPercent,
                         double peakMin,
                         double peakMax);

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

    void ResetToDefaults();

private:
    std::string m_networkTableName;
    std::string m_usage;
    int m_canID;
    int m_pdpID;
    int m_followId = -1;
    bool m_brakeMode = true;
    bool m_inverted = false;

    DistanceAngleCalcStruc m_calcStruc;

    bool m_enableCurrentLimiting;
    int m_peakCurrentLimit;
    int m_peakCurrnetDuration;
    int m_continuousCurrnetDuration;

    bool m_forwardLimitSwitch = false;
    bool m_forwardLimitSwitchNormallyOpen = false;

    bool m_reverseLimitSwitch = false;
    bool m_reverseLimitSwitchNormallyOpen = false;

    double m_voltageCompensationDuration = 12.0;
    bool m_enableVoltagCompensation = false;

    IDragonMotorController::MOTOR_TYPE m_motorType = IDragonMotorController::FALCON500;
    std::string m_canBusName = std::string("");
};
