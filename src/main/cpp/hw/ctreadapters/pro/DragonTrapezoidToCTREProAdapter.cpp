
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
#include <string>

// FRC includes

// Team 302 includes
#include "hw/DistanceAngleCalcStruc.h"
#include "hw/ctreadapters/pro/DragonControlToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonTrapezoidToCTREProAdapter.h"
#include "mechanisms/controllers/ControlData.h"
#include "mechanisms/controllers/ControlModes.h"
#include "utils/ConversionUtils.h"

// Third Party Includes
#include "ctre/phoenixpro/controls/MotionMagicDutyCycle.hpp"
#include "ctre/phoenixpro/controls/MotionMagicTorqueCurrentFOC.hpp"
#include "ctre/phoenixpro/controls/MotionMagicVoltage.hpp"
#include "units/angle.h"

using ctre::phoenixpro::controls::MotionMagicDutyCycle;
using ctre::phoenixpro::controls::MotionMagicTorqueCurrentFOC;
using ctre::phoenixpro::controls::MotionMagicVoltage;
using ctre::phoenixpro::hardware::TalonFX;
using std::string;

DragonTrapezoidToCTREProAdapter::DragonTrapezoidToCTREProAdapter(string networkTableName,
                                                                 int controllerSlot,
                                                                 const ControlData &controlInfo,
                                                                 const DistanceAngleCalcStruc &calcStruc,
                                                                 ctre::phoenixpro::hardware::TalonFX &controller) : DragonControlToCTREProAdapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller)
{
}

void DragonTrapezoidToCTREProAdapter::Set(double value)
{
    units::angle::turn_t target = units::angle::degree_t(value);
    if (m_isVoltage)
    {
        MotionMagicVoltage out{target};
        out.WithEnableFOC(m_enableFOC);
        m_controller.SetControl(out);
    }
    else if (m_isTorque)
    {
        MotionMagicTorqueCurrentFOC out{target};
        m_controller.SetControl(out);
    }
    else
    {
        MotionMagicDutyCycle out{target};
        out.WithEnableFOC(m_enableFOC);
        m_controller.SetControl(out);
    }
}

void DragonTrapezoidToCTREProAdapter::SetControlConstants(int controlSlot,
                                                          const ControlData &controlInfo)
{
    SetPeakAndNominalValues(m_networkTableName, controlInfo);
    SetPIDConstants(m_networkTableName, m_controllerSlot, controlInfo);
    SetMaxVelocityAcceleration(m_networkTableName, m_controlData);
}
