
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

#include <memory>
#include <vector>

#include <frc/motorcontrol/MotorController.h>

#include <mechanisms/controllers/ControlModes.h>
#include "hw/interfaces/IDragonMotorController.h"
#include "configs/RobotElementNames.h"

// Third Party Includes
#include <ctre/phoenix/motorcontrol/RemoteSensorSource.h>
#include <ctre/phoenix/ErrorCode.h>
#include <ctre/phoenix/motorcontrol/can/WPI_TalonSRX.h>

class DragonTalonBase : public IDragonMotorController
{
public:
    // Constructors
    DragonTalonBase() = delete;
    DragonTalonBase(
        RobotElementNames::MOTOR_CONTROLLER_USAGE deviceType,
        int deviceID,
        int pdpID,
        int countsPerRev,
        double gearRatio,
        double countsPerInch,
        double countsPerDegree,
        IDragonMotorController::MOTOR_TYPE motortype

    );
    virtual ~DragonTalonBase() = default;

    // Getters (override)
    double GetRotations() const override;
    double GetRPS() const override;
    RobotElementNames::MOTOR_CONTROLLER_USAGE GetType() const override;
    int GetID() const override;
    double GetCurrent() const override;
    IDragonMotorController::MOTOR_TYPE GetMotorType() const override;

    // Setters (override)
    void SetControlMode(ControlModes::CONTROL_TYPE mode) override; //: D
    void Set(double value) override;
    void Set(std::shared_ptr<nt::NetworkTable> nt, double value) override;
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
    void SetControlConstants(int slot, const ControlData &controlInfo) override;
    // Method:		SelectClosedLoopProfile
    // Description:	Selects which profile slot to use for closed-loop control
    // Returns:		void
    void SelectClosedLoopProfile(int slot, int pidIndex); // <I> - 0 for primary closed loop, 1 for cascaded closed-loop

    int ConfigSelectedFeedbackSensor(
        ctre::phoenix::motorcontrol::FeedbackDevice feedbackDevice,
        int pidIdx,
        int timeoutMs);
    int ConfigSelectedFeedbackSensor(
        ctre::phoenix::motorcontrol::RemoteFeedbackDevice feedbackDevice,
        int pidIdx,
        int timeoutMs);
    int ConfigPeakCurrentLimit(int amps, int timeoutMs);
    int ConfigPeakCurrentDuration(int milliseconds, int timeoutMs);
    int ConfigContinuousCurrentLimit(int amps, int timeoutMs);

    void SetAsFollowerMotor(int masterCANID);

    void SetForwardLimitSwitch(
        bool normallyOpen);

    void SetReverseLimitSwitch(
        bool normallyOpen);

    void SetRemoteSensor(
        int canID,
        ctre::phoenix::motorcontrol::RemoteSensorSource deviceType) override;

    void SetDiameter(double diameter) override;

    double GetCountsPerRev() const override { return m_countsPerRev; }
    /**
        void UpdateFramePeriods
        (
	        ctre::phoenix::motorcontrol::StatusFrameEnhanced	frame,
            uint8_t			                                    milliseconds
        ) override;
        &&/
        
        void SetVoltage
        (
            units::volt_t output
        ) override;

        double GetGearRatio() const override { return m_gearRatio;}
        bool IsForwardLimitSwitchClosed()  override;
        bool IsReverseLimitSwitchClosed() override;
        void EnableVoltageCompensation( double fullvoltage) override;
        void SetSelectedSensorPosition
        (
            double  initialPosition
        ) override;
        
        double GetCountsPerInch() const override;
        double GetCountsPerDegree() const override;
        ControlModes::CONTROL_TYPE GetControlMode() const override;
        double GetCounts() override;
        void EnableDisableLimitSwitches
        (
            bool enable
        ) override;


    private:
        std::shared_ptr<ctre::phoenix::motorcontrol::can::WPI_TalonSRX>  m_talon;
        ControlModes::CONTROL_TYPE m_controlMode;
        RobotElementNames::MOTOR_CONTROLLER_USAGE m_type;

        int m_id;
        int m_pdp;
        int m_countsPerRev;
        int m_tickOffset;
        double m_gearRatio;
        double m_diameter;
        double m_countsPerInch;
        double m_countsPerDegree;
        IDragonMotorController::MOTOR_TYPE m_motorType;
};

typedef std::vector<DragonTalonBase*> DragonTalonBaseVector;