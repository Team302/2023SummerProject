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
//==============================================================
// This file is auto generated by FRCrobotCodeGen302.exe Version $CODE_GENERATOR_VERSION$
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated, unless the changes are delimited by:
//  //========= Hand modified code start section x ========
//                    Your hand written code goes here
//	//========= Hand modified code end section x ========
//==============================================================

#pragma once

// C++ Includes
#include <string>

// FRC includes

// Team 302 includes
#include <mechanisms/base/StateMgr.h>
#include <mechanisms/arm/arm.h>
#include <mechanisms/StateStruc.h>

//========= Hand modified code start section 0 ========
#include <mechanisms/intake/IntakeStateMgr.h>
#include <robotstate/IRobotStateChangeSubscriber.h>
#include <robotstate/RobotStateChanges.h>
//========= Hand modified code end section 0 ========

// Third Party Includes

//========= Hand modified code start section 1 ========
class TeleopControl;
//========= Hand modified code end section 1 ========

class ArmStateMgr : public StateMgr
    //========= Hand modified code start section 2 ========
    ,
                    public IRobotStateChangeSubscriber
//========= Hand modified code end section 2 ========
{
public:
    /// @enum the various states of the Intake
    enum ARM_STATE
    {
        HOLD_POSITION_ROTATE,
        MANUAL_ROTATE,
        CUBE_BACKROW_ROTATE,
        CONE_BACKROW_ROTATE,
        CUBE_MIDROW_ROTATE_UP,
        CUBE_MIDROW_ROTATE_DOWN,
        CONE_MIDROW_ROTATE_UP,
        CONE_MIDROW_ROTATE_DOWN,
        HUMAN_PLAYER_STATION_ROTATE,
        STARTING_POSITION_ROTATE,
        FLOOR_POSITION_ROTATE,
        FLOOR_POSITION_ROTATE_AUTON,
        FLOOR_POSITION_ROTATE_AUTON_HOLD
    };

    const std::map<const std::string, ARM_STATE> m_armXmlStringToStateEnumMap{
        {"HOLD_POSITION_ROTATE", ARM_STATE::HOLD_POSITION_ROTATE},
        {"MANUAL_ROTATE", ARM_STATE::MANUAL_ROTATE},
        {"CUBE_BACKROW_ROTATE", ARM_STATE::CUBE_BACKROW_ROTATE},
        {"CONE_BACKROW_ROTATE", ARM_STATE::CONE_BACKROW_ROTATE},
        {"CUBE_MIDROW_ROTATE_UP", ARM_STATE::CUBE_MIDROW_ROTATE_UP},
        {"CUBE_MIDROW_ROTATE_DOWN", ARM_STATE::CUBE_MIDROW_ROTATE_DOWN},
        {"CONE_MIDROW_ROTATE_UP", ARM_STATE::CONE_MIDROW_ROTATE_UP},
        {"CONE_MIDROW_ROTATE_DOWN", ARM_STATE::CONE_MIDROW_ROTATE_DOWN},
        {"HUMAN_PLAYER_STATION_ROTATE", ARM_STATE::HUMAN_PLAYER_STATION_ROTATE},
        {"STARTING_POSITION_ROTATE", ARM_STATE::STARTING_POSITION_ROTATE},
        {"FLOOR_POSITION_ROTATE", ARM_STATE::FLOOR_POSITION_ROTATE},
        {"FLOOR_POSITION_ROTATE_AUTON", ARM_STATE::FLOOR_POSITION_ROTATE_AUTON},
        {"FLOOR_POSITION_ROTATE_AUTON_HOLD", ARM_STATE::FLOOR_POSITION_ROTATE_AUTON_HOLD}};

    /// @brief  Find or create the state manmanager
    static ArmStateMgr *GetInstance();

    /// @brief  Get the current Parameter parm value for the state of this mechanism
    /// @param PrimitiveParams* currentParams current set of primitive parameters
    /// @returns int state id - -1 indicates that there is not a state to set
    int GetCurrentStateParam(
        PrimitiveParams *currentParams) override;

    void CheckForStateTransition() override;
    //========= Hand modified code start section 3 ========
    void CheckForSensorTransitions() override;
    void CheckForGamepadTransitions() override;

    void Update(RobotStateChanges::StateChange change, int state) override;

    //========= Hand modified code end section 3 ========

private:
    ArmStateMgr();
    ~ArmStateMgr() = default;

    //========= Hand modified code start section 4 ========
    void CheckForConeGamepadTransitions(TeleopControl *controller);
    void CheckForCubeGamepadTransitions(TeleopControl *controller);
    //========= Hand modified code end section 4 ========

    Arm *m_arm;

    //========= Hand modified code start section 5 ========
    ARM_STATE m_prevState;
    ARM_STATE m_currentState;
    ARM_STATE m_targetState;

    RobotStateChanges::GamePiece m_gamepieceMode;
    //========= Hand modified code end section 5 ========

    IntakeStateMgr::INTAKE_STATE m_intakeState;

    static ArmStateMgr *m_instance;

    const StateStruc m_hold_position_rotateState = {ARM_STATE::HOLD_POSITION_ROTATE, "HOLD_POSITION_ROTATE", StateType::ARM_STATE, false};
    const StateStruc m_manual_rotateState = {ARM_STATE::MANUAL_ROTATE, "MANUAL_ROTATE", StateType::MANUAL_ARM_STATE, false};
    const StateStruc m_cube_backrow_rotateState = {ARM_STATE::CUBE_BACKROW_ROTATE, "CUBE_BACKROW_ROTATE", StateType::ARM_STATE, false};
    const StateStruc m_cone_backrow_rotateState = {ARM_STATE::CONE_BACKROW_ROTATE, "CONE_BACKROW_ROTATE", StateType::ARM_STATE, false};
    const StateStruc m_cube_midrow_rotate_upState = {ARM_STATE::CUBE_MIDROW_ROTATE_UP, "CUBE_MIDROW_ROTATE", StateType::ARM_STATE, false};
    const StateStruc m_cube_midrow_rotate_downState = {ARM_STATE::CUBE_MIDROW_ROTATE_DOWN, "CUBE_MIDROW_ROTATE", StateType::ARM_STATE, false};
    const StateStruc m_cone_midrow_rotate_upState = {ARM_STATE::CONE_MIDROW_ROTATE_UP, "CONE_MIDROW_ROTATE", StateType::ARM_STATE, false};
    const StateStruc m_cone_midrow_rotate_downState = {ARM_STATE::CONE_MIDROW_ROTATE_DOWN, "CONE_MIDROW_ROTATE", StateType::ARM_STATE, false};
    const StateStruc m_human_player_station_rotateState = {ARM_STATE::HUMAN_PLAYER_STATION_ROTATE, "HUMAN_PLAYER_STATION_ROTATE", StateType::ARM_STATE, false};
    const StateStruc m_starting_position_rotateState = {ARM_STATE::STARTING_POSITION_ROTATE, "STARTING_POSITION_ROTATE", StateType::ARM_STATE, true};
    const StateStruc m_floor_position_rotateState = {ARM_STATE::FLOOR_POSITION_ROTATE, "FLOOR_POSITION_ROTATE", StateType::ARM_STATE, false};
    const StateStruc m_floor_position_rotate_autonState = {ARM_STATE::FLOOR_POSITION_ROTATE_AUTON, "FLOOR_POSITION_ROTATE_AUTON", StateType::ARM_STATE, false};
    const StateStruc m_floor_position_rotate_auton_holdState = {ARM_STATE::FLOOR_POSITION_ROTATE_AUTON_HOLD, "FLOOR_POSITION_ROTATE_AUTON_HOLD", StateType::ARM_STATE, false};
};
