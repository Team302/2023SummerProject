
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

// Team 302 includes
#include <mechanisms/base/Mech.h>
#include <mechanisms/base/Mech2IndMotors.h>
#include <mechanisms/base/Mech1Solenoid.h>
#include <mechanisms/MechanismTypes.h>

// forward declares
class ControlData;
class IDragonMotorController;

class Mech2Motors1Solenoid : public Mech
{
public:
    /// @brief Create a generic mechanism wiht 2 independent motors
    /// @param [in] MechanismTypes::MECHANISM_TYPE the type of mechansim
    /// @param [in] std::string the name of the file that will set control parameters for this mechanism
    /// @param [in] std::string the name of the network table for logging information
    /// @param [in] std::shared_ptr<IDragonMotorController> primary motor used by this mechanism
    /// @param [in] std::shared_ptr<IDragonMotorController> secondary motor used by this mechanism
    Mech2Motors1Solenoid(
        MechanismTypes::MECHANISM_TYPE type,
        std::string controlFileName,
        std::string networkTableName,
        std::shared_ptr<IDragonMotorController> primaryMotor,
        std::shared_ptr<IDragonMotorController> secondaryMotor,
        std::shared_ptr<DragonSolenoid> solenoid);
    Mech2Motors1Solenoid() = delete;
    ~Mech2Motors1Solenoid() = default;

    /// @brief log data to the network table if it is activated and time period has past
    void LogInformation() const override;

    /// @brief update the output to the mechanism using the current controller and target value(s)
    /// @return void
    void Update();

    void UpdateTargets(double primary, double secondary);

    /// @brief  Return the current position of the primary motor in the mechanism.  The value is in inches or degrees.
    /// @return double	position in inches (translating mechanisms) or degrees (rotating mechanisms)
    double GetPrimaryPosition() const;

    /// @brief  Return the current position of the secondary motor in the mechanism.  The value is in inches or degrees.
    /// @return double	position in inches (translating mechanisms) or degrees (rotating mechanisms)
    double GetSecondaryPosition() const;

    /// @brief  Get the current speed of the primary motor in the mechanism.  The value is in inches per second or degrees per second.
    /// @return double	speed in inches/second (translating mechanisms) or degrees/second (rotating mechanisms)
    double GetPrimarySpeed() const;

    /// @brief  Get the current speed of the secondary motor in the mechanism.  The value is in inches per second or degrees per second.
    /// @return double	speed in inches/second (translating mechanisms) or degrees/second (rotating mechanisms)
    double GetSecondarySpeed() const;

    /// @brief  Set the control constants (e.g. PIDF values).
    /// @param [in] ControlData*                                   pid:  the control constants
    /// @return void
    void SetControlConstants(int slot, ControlData *pid);
    void SetSecondaryControlConstants(int slot, ControlData *pid);

    /// @brief      Activate/deactivate pneumatic solenoid
    /// @param [in] bool - true == extend, false == retract
    /// @return     void
    void ActivateSolenoid(bool activate);

    /// @brief      Check if the pneumatic solenoid is activated
    /// @return     bool - true == extended, false == retracted
    bool IsSolenoidActivated() const;

    Mech2IndMotors *GetMotorMech() const { return m_motorMech; }
    Mech1Solenoid *GetSolenoidMech() const { return m_solenoidMech; }

private:
    Mech2IndMotors *m_motorMech;
    Mech1Solenoid *m_solenoidMech;
};
