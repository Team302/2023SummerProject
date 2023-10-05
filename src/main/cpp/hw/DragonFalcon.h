
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

// C++ Includes
#include <memory>
#include <string>

// FRC includes
#include <frc/motorcontrol/MotorController.h>

// Team 302 includes
#include "hw/DistanceAngleCalcStruc.h"
#include "hw/interfaces/IDragonMotorController.h"
#include "configs/usages/MotorControllerUsage.h"

// Third Party Includes
#include "ctre/phoenixpro/TalonFX.hpp"
// #include <ctre/phoenix/motorcontrol/can/WPI_TalonFX.h>
// #include <ctre/phoenix/motorcontrol/RemoteSensorSource.h>

class IDragonControlToVendorControlAdapter;

class DragonFalcon : public IDragonMotorController
{
public:
    // Constructors
    DragonFalcon() = delete;
    DragonFalcon(std::string networkTableName,
                 MotorControllerUsage::MOTOR_CONTROLLER_USAGE deviceType,
                 int deviceID,
                 std::string canBusName,
                 int pdpID);
    virtual ~DragonFalcon() = default;
    ctre::phoenixpro::hardware::TalonFX *GetTalonFX() const { return m_talon; }

    // Getters (override)
    double GetRotations() const override;
    double GetRPS() const override;
    MotorControllerUsage::MOTOR_CONTROLLER_USAGE GetType() const override;
    int GetID() const override;
    frc::MotorController *GetSpeedController() const override;
    double GetCurrent() const override;
    IDragonMotorController::MOTOR_TYPE GetMotorType() const override;

    // Setters (override)
    // void SetControlMode(ControlModes::CONTROL_TYPE mode) override; //:D
    void Set(double value) override;
    void SetRotationOffset(double rotations) override;
    void SetVoltageRamping(double ramping, double rampingClosedLoop = -1) override; // seconds 0 to full, set to 0 to disable
    void EnableCurrentLimiting(bool enabled) override;
    void EnableBrakeMode(bool enabled) override;
    void Invert(bool inverted) override;
    void SetSensorInverted(bool inverted) override;

    /// @brief  Set the control constants (e.g. PIDF values).
    /// @param [in] int             slot - hardware slot to use
    /// @param [in] ControlData*    pid - the control constants
    /// @return void
    void SetControlConstants(int slot, ControlData *controlInfo) override;

    /**
    int ConfigSelectedFeedbackSensor(
        ctre::phoenix::motorcontrol::FeedbackDevice feedbackDevice,
        int pidIdx,
        int timeoutMs);
    int ConfigSelectedFeedbackSensor(
        ctre::phoenix::motorcontrol::RemoteFeedbackDevice feedbackDevice,
        int pidIdx,
        int timeoutMs);
        **/
    void SetCurrentLimits(bool enableStatorCurrentLimit,
                          units::current::ampere_t statorCurrentLimit,
                          bool enableSupplyCurrentLimit,
                          units::current::ampere_t supplyCurrentLimit,
                          units::current::ampere_t supplyCurrentThreshold,
                          units::time::second_t supplyTimeThreshold);
    void SetPIDConstants(int slot, double kP, double kI, double kD, double kF);
    void ConfigHWLimitSW(bool enableForward,
                         int remoteForwardSensorID,
                         bool forwardResetPosition,
                         double forwardPosition,
                         ctre::phoenixpro::signals::ForwardLimitSourceValue forwardType,
                         ctre::phoenixpro::signals::ForwardLimitTypeValue forwardOpenClose,
                         bool enableReverse,
                         int remoteReverseSensorID,
                         bool reverseResetPosition,
                         double reversePosition,
                         ctre::phoenixpro::signals::ReverseLimitSourceValue revType,
                         ctre::phoenixpro::signals::ReverseLimitTypeValue revOpenClose);
    void ConfigMotorSettings(ctre::phoenixpro::signals::InvertedValue inverted,
                             ctre::phoenixpro::signals::NeutralModeValue mode,
                             double deadbandPercent,
                             double peakForwardDutyCycle,
                             double peakReverseDutyCycle);

    void SetAsFollowerMotor(int masterCANID);

    void SetRemoteSensor(int canID,
                         ctre::phoenix::motorcontrol::RemoteSensorSource deviceType) override;

    void SetDiameter(double diameter) override;

    void SetVoltage(units::volt_t output) override;

    /**
    void UpdateFramePeriods
    (
        ctre::phoenix::motorcontrol::StatusFrameEnhanced	frame,
        uint8_t			                                    milliseconds
    ) override;
    **/

    void SetFramePeriodPriority(
        MOTOR_PRIORITY priority) override;

    double GetCountsPerRev() const override { return m_calcStruc.countsPerRev; }
    double GetGearRatio() const override { return m_calcStruc.gearRatio; }
    bool IsMotorInverted() const override { return m_inverted; };
    bool IsForwardLimitSwitchClosed() const override;
    bool IsReverseLimitSwitchClosed() const override;
    void EnableVoltageCompensation(double fullvoltage) override;
    void SetSelectedSensorPosition(
        double initialPosition) override;

    double GetCountsPerInch() const override;
    double GetCountsPerDegree() const override;
    double GetCounts() const override;
    void EnableDisableLimitSwitches(
        bool enable) override;

private:
    std::string m_networkTableName;
    ctre::phoenixpro::hardware::TalonFX *m_talon;
    IDragonControlToVendorControlAdapter *m_controller[4];
    MotorControllerUsage::MOTOR_CONTROLLER_USAGE m_type;
    int m_id;
    int m_pdp;
    DistanceAngleCalcStruc m_calcStruc;
    IDragonMotorController::MOTOR_TYPE m_motorType;
    bool m_inverted;
};
