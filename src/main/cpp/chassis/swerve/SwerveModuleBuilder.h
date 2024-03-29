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

#include "chassis/swerve/SwerveModule.h"
#include "hw/DragonCanCoder.h"
#include "hw/interfaces/IDragonMotorController.h"
#include "mechanisms/controllers/ControlData.h"

class SwerveModuleBuilder
{
public:
    SwerveModuleBuilder();
    ~SwerveModuleBuilder() = default;

    void SetModuleID(SwerveModule::ModuleID id);
    void SetMotors(IDragonMotorController &driveMotor,
                   IDragonMotorController &turnMotor);
    void SetCanCoder(DragonCanCoder &angleSensor);
    void SetTurnControlData(ControlData &pid, double encoderCountsPerAngleSensorDegree);
    void SetWheelDiameter(units::length::inch_t diameter);
    bool IsValid() const;
    SwerveModule *Commit();

private:
    SwerveModule::ModuleID m_moduleID;
    IDragonMotorController *m_driveMotor;
    IDragonMotorController *m_turnMotor;
    DragonCanCoder *m_canCoder;
    ControlData *m_turnControlData;
    double m_countsOnTurnEncoderPerDegreesOnAngleSensor;
    units::length::inch_t m_wheelDiameter;
};