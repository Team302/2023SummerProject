
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
#include "hw/DragonCanCoder.h"
#include "hw/DistanceAngleCalcStruc.h"
#include "hw/interfaces/IDragonMotorController.h"
#include "configs/usages/MotorControllerUsage.h"

// Third Party Includes
#include "ctre/phoenixpro/TalonFX.hpp"
#include "ctre/phoenixpro/controls/ControlRequest.hpp"

class IDragonControlToVendorControlAdapter;

class DragonTalonFX : public IDragonMotorController
{
public:
    // Constructors
    DragonTalonFX() = delete;
    DragonTalonFX(std::string networkTableName,
                  MotorControllerUsage::MOTOR_CONTROLLER_USAGE deviceType,
                  int deviceID,
                  std::string canBusName);
    // DragonTalonFX(const DragonTalonFX &other);
    virtual ~DragonTalonFX() = default;

    // Getters (override)
    double GetRotations() override;
    double GetRPS() override;
    MotorControllerUsage::MOTOR_CONTROLLER_USAGE GetType() const override;
    int GetID() const override;
    double GetCurrent() override;
    IDragonMotorController::MOTOR_TYPE GetMotorType() const override;

    // Setters (override)
    // void SetControlMode(ControlModes::CONTROL_TYPE mode) override; //:D
    void Set(double value) override;
    void Set(int slot, double value);
    void SetRotationOffset(double rotations) override;
    void SetVoltageRamping(double ramping, double rampingClosedLoop = -1) override; // seconds 0 to full, set to 0 to disable
    void EnableCurrentLimiting(bool enabled) override;
    void EnableBrakeMode(bool enabled) override;
    void Invert(bool inverted) override;
    void SetSensorInverted(bool inverted) override;
    void ResetToDefaults();

    /// @brief  Set the control constants (e.g. PIDF values).
    /// @param [in] int             slot - hardware slot to use
    /// @param [in] ControlData*    pid - the control constants
    /// @return void
    void SetControlConstants(int slot, const ControlData &controlInfo) override;

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

    void FuseCancoder(DragonCanCoder &cancoder,
                      double sensorToMechanismRatio,
                      double rotorToSensorRatio);

    void SetDiameter(double diameter) override;

    void SetVoltage(units::volt_t output) override;

    /**
    void UpdateFramePeriods
    (
        ctre::phoenix::motorcontrol::StatusFrameEnhanced	frame,
        uint8_t			                                    milliseconds
    ) override;
    **/

    void SetFramePeriodPriority(MOTOR_PRIORITY priority) override;

    double GetCountsPerRev() const override { return m_calcStruc.countsPerRev; }
    double GetGearRatio() const override { return m_calcStruc.gearRatio; }
    bool IsMotorInverted() const override { return m_inverted; };
    bool IsForwardLimitSwitchClosed() override;
    bool IsReverseLimitSwitchClosed() override;
    void EnableVoltageCompensation(double fullvoltage) override;
    void SetSelectedSensorPosition(double initialPosition) override;

    double GetCountsPerInch() const override;
    double GetCountsPerDegree() const override;
    double GetCounts() override;
    void EnableDisableLimitSwitches(bool enable) override;

private:
    std::string m_networkTableName;
    MotorControllerUsage::MOTOR_CONTROLLER_USAGE m_type;
    ctre::phoenixpro::hardware::TalonFX m_talon;
    ctre::phoenixpro::controls::ControlRequest *slot0Control;
    ctre::phoenixpro::controls::ControlRequest *slot1Control;
    ctre::phoenixpro::controls::ControlRequest *slot2Control;
    ctre::phoenixpro::controls::ControlRequest *slot3Control;
    // IDragonControlToVendorControlAdapter *m_controller[4];
    DistanceAngleCalcStruc m_calcStruc;
    bool m_inverted;
};
