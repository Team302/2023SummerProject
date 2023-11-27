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

#include "auton/DragonEvent.h"
#include "mechanisms/StateMgrHelper.h"
#include "chassis/ChassisFactory.h"
#include "DragonVision/LimelightFactory.h"

DragonEvent::DragonEvent(std::string name,
                         ChassisOptionEnums::HeadingOption headingOption,
                         float heading,
                         // mechanism params
                         ArmStateMgr::ARM_STATE armState,
                         ExtenderStateMgr::EXTENDER_STATE extenderState,
                         IntakeStateMgr::INTAKE_STATE intakeState,
                         DragonLimelight::PIPELINE_MODE pipelineMode) : m_name(name),
                                                                        m_headingOption(headingOption),
                                                                        m_heading(heading),
                                                                        m_armState(armState),
                                                                        m_extenderState(extenderState),
                                                                        m_intakeState(intakeState),
                                                                        m_pipelineMode(pipelineMode)
{
}

std::function<void()> DragonEvent::RegisterEventRunner(DragonEvent *event)
{
    // register chassis movement for later use
    ChassisMovement chassisMovement = ChassisMovement();

    chassisMovement.headingOption = event->GetHeadingOption();
    chassisMovement.yawAngle = units::angle::degree_t(event->GetHeading());
    m_registeredEventsMap[event->GetName()] = chassisMovement;

    // return lambda function for FRC command
    return [event]()
    {
        StateMgrHelper::SetMechanismStateFromEvent(event);

        SwerveChassis *chassis = ChassisFactory::GetChassisFactory()->GetSwerveChassis();

        DragonLimelight *m_limelight = LimelightFactory::GetLimelightFactory()->GetLimelight(LimelightUsages::PRIMARY);

        if (event->GetPipelineMode() != DragonLimelight::PIPELINE_MODE::UNKNOWN)
            m_limelight->SetPipeline(event->GetPipelineMode());
    };
}