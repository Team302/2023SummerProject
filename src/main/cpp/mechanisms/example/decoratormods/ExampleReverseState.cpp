
//====================================================================================================================================================
/// Copyright 2023 Lake Orion Robotics FIRST Team 302
///
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
/// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
/// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
/// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
/// OR OTHER DEALINGS IN THE SOFTWARE.
//====================================================================================================================================================

// C++ Includes
#include <string>

// FRC includes

// Team 302 includes
#include "State.h"
#include "mechanisms/example/generated/ExampleReverseStateGen.h"
#include "mechanisms/example/decoratormods/ExampleReverseState.h"
#include "mechanisms/controllers/ControlData.h"
#include "mechanisms/base/BaseMech.h"
#include "teleopcontrol/TeleopControl.h"
#include "teleopcontrol/TeleopControlFunctions.h"
#include "utils/logging/Logger.h"

// Third Party Includes

using namespace std;

/// @class ExampleReverseState
/// @brief information about the control (open loop, closed loop position, closed loop velocity, etc.) for a mechanism state
ExampleReverseState::ExampleReverseState(std::string stateName,
                                         int stateId,
                                         ExampleReverseStateGen *generatedState) : State(stateName, stateId), m_genState(generatedState)
{
}

void ExampleReverseState::Init()
{
    m_genState->Init();
}

void ExampleReverseState::Run()
{
    m_genState->Run();
}

void ExampleReverseState::Exit()
{
    m_genState->Exit();
}

bool ExampleReverseState::AtTarget()
{
    auto attarget = m_genState->AtTarget();
    return attarget;
}

bool ExampleReverseState::IsTransitionCondition(bool considerGamepadTransitions) const
{
    // auto transition = m_genState->IsTransitionCondition(considerGamepadTransitions);
    // return transition;
    return (considerGamepadTransitions && TeleopControl::GetInstance()->IsButtonPressed(TeleopControlFunctions::EXAMPLE_MECH_REVERSE));
}