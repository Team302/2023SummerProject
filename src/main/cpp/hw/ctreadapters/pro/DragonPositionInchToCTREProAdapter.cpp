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
#include <numbers>
#include <string>

// FRC includes

// Team 302 includes
#include "hw/DistanceAngleCalcStruc.h"
#include "hw/ctreadapters/pro/DragonControlToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonPositionInchToCTREProAdapter.h"
#include "mechanisms/controllers/ControlData.h"
#include "mechanisms/controllers/ControlModes.h"
#include "utils/ConversionUtils.h"

// Third Party Includes
#include "units/length.h"
#include "ctre/phoenixpro/controls/PositionDutyCycle.hpp"
#include "ctre/phoenixpro/controls/PositionTorqueCurrentFOC.hpp"
#include "ctre/phoenixpro/controls/PositionVoltage.hpp"

using ctre::phoenixpro::controls::PositionDutyCycle;
using ctre::phoenixpro::controls::PositionTorqueCurrentFOC;
using ctre::phoenixpro::controls::PositionVoltage;
using ctre::phoenixpro::hardware::TalonFX;
using std::string;

DragonPositionInchToCTREProAdapter::DragonPositionInchToCTREProAdapter(string networkTableName,
                                                                       int controllerSlot,
                                                                       const ControlData &controlInfo,
                                                                       const DistanceAngleCalcStruc &calcStruc,
                                                                       ctre::phoenixpro::hardware::TalonFX &controller) : DragonControlToCTREProAdapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller)
{
}

void DragonPositionInchToCTREProAdapter::Set(double value)
{
    units::angle::degree_t target = (m_calcStruc.countsPerInch > DistanceAngleCalcStruc::countsPerTolerance) ? units::angle::turn_t(value / m_calcStruc.countsPerInch) : units::angle::turn_t(value / (m_calcStruc.diameter * std::numbers::pi));
    if (m_isVoltage)
    {
        PositionVoltage out{target, m_enableFOC, m_voltageFeedForward, m_controllerSlot, false};
        m_controller.SetControl(out);
    }
    else if (m_isTorque)
    {
        PositionTorqueCurrentFOC out{target, m_torqueCurrentFeedForward, m_controllerSlot, false};
        m_controller.SetControl(out);
    }
    else
    {
        PositionDutyCycle out{target, m_enableFOC, m_dutyFeedForward, m_controllerSlot, false};
        m_controller.SetControl(out);
    }
}

void DragonPositionInchToCTREProAdapter::SetControlConstants(int controlSlot,
                                                             const ControlData &controlInfo)
{
    SetPeakAndNominalValues(m_networkTableName, controlInfo);
    SetPIDConstants(m_networkTableName, m_controllerSlot, controlInfo);
}
