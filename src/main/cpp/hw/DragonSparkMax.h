/*
 * DragonSparkMax.h
 *
 * Date Created: Jan 15, 2019
 * Author: Jonah Shader
 */

#pragma once

#include <memory>

#include "configs/RobotElementNames.h"
#include "hw/interfaces/IDragonMotorController.h"

#include "ctre/phoenix/motorcontrol/RemoteSensorSource.h" // need to remove dependency on ctre
#include "rev/CANSparkMax.h"

// namespaces
using namespace rev;

class DragonSparkMax : public IDragonMotorController
{
public:
    // note: two PIDs: 0 is position, 1 is velocity
    //  Constructors
    DragonSparkMax() = delete;
    DragonSparkMax(int id,
                   RobotElementNames::MOTOR_CONTROLLER_USAGE deviceType,
                   CANSparkMax::MotorType motorType,
                   double gearRatio);

    virtual ~DragonSparkMax() = default;

    // Getters
    double GetRotations() override;
    double GetRPS() override;
    RobotElementNames::MOTOR_CONTROLLER_USAGE GetType() const override;
    int GetID() const override;

    // Setters
    void SetControlConstants(int slot, const ControlData &controlInfo) override;

    void Set(double value) override;
    void SetRotationOffset(double rotations) override;
    void SetVoltageRamping(double ramping, double rampingClosedLoop = -1) override; // seconds 0 to full, set to 0 to disable
    void EnableCurrentLimiting(bool enabled) override;
    void EnableBrakeMode(bool enabled) override;
    void Invert(bool inverted) override;

    void InvertEncoder(bool inverted);
    void SetSmartCurrentLimiting(int limit);
    void SetSecondaryCurrentLimiting(int limit, int duration);
    // CANError Follow(DragonSparkMax* leader, bool invert = false);

    // dummy methods below
    // std::shared_ptr<frc::MotorController> GetSpeedController() override;
    double GetCurrent() override;
    IDragonMotorController::MOTOR_TYPE GetMotorType() const override;
    void SetSensorInverted(bool inverted) override;
    void SetDiameter(double diameter) override;
    void SetVoltage(units::volt_t output) override;
    double GetCounts() override;
    void SetRemoteSensor(int canID, ctre::phoenix::motorcontrol::RemoteSensorSource deviceType) override;
    bool IsForwardLimitSwitchClosed() override;
    bool IsReverseLimitSwitchClosed() override;
    void EnableVoltageCompensation(double fullvoltage) override;
    void SetSelectedSensorPosition(double initialPosition) override;
    double GetCountsPerInch() const override;
    double GetCountsPerDegree() const override;
    void EnableDisableLimitSwitches(bool enable) override;
    double GetCountsPerRev() const override { return 1.0; }
    double GetGearRatio() const override { return 1.0; }

private:
    double GetRotationsWithGearNoOffset() const;
    int m_id;
    CANSparkMax *m_spark;
    // DRAGON_CONTROL_MODE m_controlMode;
    double m_outputRotationOffset;
    double m_gearRatio;
    RobotElementNames::MOTOR_CONTROLLER_USAGE m_deviceType;

    CANSparkMax *GetSparkMax();
};