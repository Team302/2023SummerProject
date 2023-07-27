
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

// FRC includes

// Team 302 includes
// #include <gamepad/IDragonGamePad.h>
#include <teleopcontrol/TeleopControlAxis.h>
#include <teleopcontrol/TeleopControlButton.h>
#include <teleopcontrol/TeleopControlFunctions.h>

#include <RobinHood/robin_hood.h>

const TeleopControlButton driverAButton = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::A_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverBButton = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::B_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverXButton = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::X_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverYButton = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::Y_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverLeftBumper = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::LEFT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverRightBumper = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::RIGHT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverSelectButton = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::SELECT_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverStartButton = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::START_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverLeftStickPressed = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::LEFT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverRightStickPressed = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::RIGHT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverLeftTriggerPressed = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::LEFT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverRightTriggerPressed = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::RIGHT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverDPad0 = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::POV_0, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverDPad45 = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::POV_45, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverDPad90 = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::POV_90, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverDPad135 = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::POV_135, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverDPad180 = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::POV_180, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverDPad225 = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::POV_225, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverDPad270 = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::POV_270, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton driverDPad315 = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::POV_315, TeleopControlMappingEnums::STANDARD};

const TeleopControlButton copilotAButton = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::A_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotBButton = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::B_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotXButton = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::X_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotYButton = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::Y_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotLeftBumper = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::LEFT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotRightBumper = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::RIGHT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotSelectButton = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::SELECT_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotStartButton = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::START_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotLeftStickPressed = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::LEFT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotRightStickPressed = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::RIGHT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotLeftTriggerPressed = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::LEFT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotRightTriggerPressed = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::RIGHT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotDPad0 = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::POV_0, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotDPad45 = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::POV_45, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotDPad90 = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::POV_90, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotDPad135 = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::POV_135, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotDPad180 = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::POV_180, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotDPad225 = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::POV_225, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotDPad270 = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::POV_270, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton copilotDPad315 = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::POV_315, TeleopControlMappingEnums::STANDARD};

const TeleopControlButton extra1AButton = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::A_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1BButton = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::B_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1XButton = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::X_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1YButton = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::Y_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1LeftBumper = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::LEFT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1RightBumper = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::RIGHT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1SelectButton = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::SELECT_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1StartButton = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::START_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1LeftStickPressed = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::LEFT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1RightStickPressed = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::RIGHT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1LeftTriggerPressed = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::LEFT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1RightTriggerPressed = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::RIGHT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1DPad0 = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::POV_0, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1DPad45 = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::POV_45, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1DPad90 = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::POV_90, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1DPad135 = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::POV_135, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1DPad180 = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::POV_180, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1DPad225 = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::POV_225, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1DPad270 = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::POV_270, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra1DPad315 = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::POV_315, TeleopControlMappingEnums::STANDARD};

const TeleopControlButton extra2AButton = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::A_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2BButton = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::B_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2XButton = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::X_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2YButton = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::Y_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2LeftBumper = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::LEFT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2RightBumper = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::RIGHT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2SelectButton = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::SELECT_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2StartButton = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::START_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2LeftStickPressed = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::LEFT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2RightStickPressed = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::RIGHT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2LeftTriggerPressed = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::LEFT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2RightTriggerPressed = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::RIGHT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2DPad0 = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::POV_0, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2DPad45 = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::POV_45, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2DPad90 = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::POV_90, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2DPad135 = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::POV_135, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2DPad180 = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::POV_180, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2DPad225 = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::POV_225, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2DPad270 = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::POV_270, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra2DPad315 = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::POV_315, TeleopControlMappingEnums::STANDARD};

const TeleopControlButton extra3AButton = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::A_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3BButton = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::B_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3XButton = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::X_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3YButton = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::Y_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3LeftBumper = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::LEFT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3RightBumper = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::RIGHT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3SelectButton = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::SELECT_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3StartButton = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::START_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3LeftStickPressed = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::LEFT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3RightStickPressed = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::RIGHT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3LeftTriggerPressed = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::LEFT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3RightTriggerPressed = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::RIGHT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3DPad0 = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::POV_0, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3DPad45 = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::POV_45, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3DPad90 = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::POV_90, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3DPad135 = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::POV_135, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3DPad180 = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::POV_180, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3DPad225 = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::POV_225, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3DPad270 = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::POV_270, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra3DPad315 = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::POV_315, TeleopControlMappingEnums::STANDARD};

const TeleopControlButton extra4AButton = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::A_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4BButton = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::B_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4XButton = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::X_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4YButton = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::Y_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4LeftBumper = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::LEFT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4RightBumper = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::RIGHT_BUMPER, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4SelectButton = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::SELECT_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4StartButton = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::START_BUTTON, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4LeftStickPressed = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::LEFT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4RightStickPressed = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::RIGHT_STICK_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4LeftTriggerPressed = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::LEFT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4RightTriggerPressed = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::RIGHT_TRIGGER_PRESSED, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4DPad0 = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::POV_0, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4DPad45 = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::POV_45, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4DPad90 = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::POV_90, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4DPad135 = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::POV_135, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4DPad180 = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::POV_180, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4DPad225 = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::POV_225, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4DPad270 = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::POV_270, TeleopControlMappingEnums::STANDARD};
const TeleopControlButton extra4DPad315 = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::POV_315, TeleopControlMappingEnums::STANDARD};

robin_hood::unordered_map<TeleopControlFunctions::FUNCTION, const TeleopControlButton> teleopControlMapButtonMap{
    $$BUTTON_BINDINGS_HERE$$};

const TeleopControlAxis driverLeftJoystickX = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::LEFT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis driverLeftJoystickY = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::LEFT_JOYSTICK_Y, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis driverRightJoystickX = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::RIGHT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 0.5};
const TeleopControlAxis driverRightJoystickY = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::RIGHT_JOYSTICK_Y, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis driverLeftTrigger = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::LEFT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis driverRightTrigger = {TeleopControlMappingEnums::DRIVER, TeleopControlMappingEnums::RIGHT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};

const TeleopControlAxis copilotLeftJoystickX = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::LEFT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis copilotLeftJoystickY = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::LEFT_JOYSTICK_Y, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 0.5};
const TeleopControlAxis copilotRightJoystickX = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::RIGHT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis copilotRightJoystickY = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::RIGHT_JOYSTICK_Y, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis copilotLeftTrigger = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::LEFT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis copilotRightTrigger = {TeleopControlMappingEnums::CO_PILOT, TeleopControlMappingEnums::RIGHT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};

const TeleopControlAxis extra1LeftJoystickX = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::LEFT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra1LeftJoystickY = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::LEFT_JOYSTICK_Y, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis extra1RightJoystickX = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::RIGHT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra1RightJoystickY = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::RIGHT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis extra1LeftTrigger = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::LEFT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra1RightTrigger = {TeleopControlMappingEnums::EXTRA1, TeleopControlMappingEnums::RIGHT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};

const TeleopControlAxis extra2LeftJoystickX = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::LEFT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra2LeftJoystickY = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::LEFT_JOYSTICK_Y, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis extra2RightJoystickX = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::RIGHT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra2RightJoystickY = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::RIGHT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis extra2LeftTrigger = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::LEFT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra2RightTrigger = {TeleopControlMappingEnums::EXTRA2, TeleopControlMappingEnums::RIGHT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};

const TeleopControlAxis extra3LeftJoystickX = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::LEFT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra3LeftJoystickY = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::LEFT_JOYSTICK_Y, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis extra3RightJoystickX = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::RIGHT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra3RightJoystickY = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::RIGHT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis extra3LeftTrigger = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::LEFT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra3RightTrigger = {TeleopControlMappingEnums::EXTRA3, TeleopControlMappingEnums::RIGHT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};

const TeleopControlAxis extra4LeftJoystickX = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::LEFT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra4LeftJoystickY = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::LEFT_JOYSTICK_Y, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis extra4RightJoystickX = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::RIGHT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra4RightJoystickY = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::RIGHT_JOYSTICK_X, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::REVERSED, 1.0};
const TeleopControlAxis extra4LeftTrigger = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::LEFT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};
const TeleopControlAxis extra4RightTrigger = {TeleopControlMappingEnums::EXTRA4, TeleopControlMappingEnums::RIGHT_TRIGGER, TeleopControlMappingEnums::APPLY_STANDARD_DEADBAND, TeleopControlMappingEnums::CUBED, TeleopControlMappingEnums::SYNCED, 1.0};

robin_hood::unordered_map<TeleopControlFunctions::FUNCTION, const TeleopControlAxis> teleopControlMapAxisMap{
    $$AXIS_BINDINGS_HERE$$};
