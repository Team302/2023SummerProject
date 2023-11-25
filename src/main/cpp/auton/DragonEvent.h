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
#include <functional>

#include <frc2/command/CommandPtr.h>
#include <frc2/command/RunCommand.h>

#include <chassis/ChassisOptionEnums.h>
#include <mechanisms/arm/ArmStateMgr.h>
#include <mechanisms/extender/ExtenderStateMgr.h>
#include <mechanisms/Intake/IntakeStateMgr.h>
#include <DragonVision/DragonLimelight.h>

class DragonEvent
{
public:
    DragonEvent(std::string name,
                ChassisOptionEnums::HeadingOption headingOption,
                float heading,
                // mechanism params
                ArmStateMgr::ARM_STATE armState,
                ExtenderStateMgr::EXTENDER_STATE extenderState,
                IntakeStateMgr::INTAKE_STATE intakeState,
                DragonLimelight::PIPELINE_MODE pipelineMode);

    DragonEvent() = delete;
    ~DragonEvent() = default;

    static std::function<void()> GetEventRunner(DragonEvent *event);

    std::string GetName() const { return m_name; };
    ChassisOptionEnums::HeadingOption GetHeadingOption() const { return m_headingOption; };
    float GetHeading() const { return m_heading; };

    // Mechanism getters
    ArmStateMgr::ARM_STATE GetArmState() const { return m_armState; };
    ExtenderStateMgr::EXTENDER_STATE GetExtenderState() const { return m_extenderState; };
    IntakeStateMgr::INTAKE_STATE GetIntakeState() const { return m_intakeState; }

    DragonLimelight::PIPELINE_MODE GetPipelineMode() const { return m_pipelineMode; }

private:
    std::string m_name;
    ChassisOptionEnums::HeadingOption m_headingOption;
    float m_heading;

    // Mechanism state variables
    ArmStateMgr::ARM_STATE m_armState;
    ExtenderStateMgr::EXTENDER_STATE m_extenderState;
    IntakeStateMgr::INTAKE_STATE m_intakeState;

    DragonLimelight::PIPELINE_MODE m_pipelineMode;
};