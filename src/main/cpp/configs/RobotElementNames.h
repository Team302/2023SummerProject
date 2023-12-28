// clang-format off
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
// This file was automatically generated by the Team 302 code generator version 1.1.0.0
// Generated on Saturday, December 23, 2023 6:45:27 PM

#pragma once

class RobotElementNames
{

public:
	enum CANCODER_INSTANCE_USAGE
	{
		UNKNOWN_CANCODER_INSTANCE = -1,
		MAX_CANCODER_INSTANCE
	};

	enum PID_USAGE
	{
		UNKNOWN_PID = -1,
		MAX_PID
	};

	enum PDP_USAGE
	{
		UNKNOWN_PDP = -1,
		PDP,
		MAX_PDP
	};

	enum PIGEON_USAGE
	{
		UNKNOWN_PIGEON = -1,
		PIGEON_ROBOT_CENTER,
		MAX_PIGEON
	};

	enum PCM_USAGE
	{
		UNKNOWN_PCM = -1,
		PCM_1,
		MAX_PCM
	};

	enum MOTOR_CONTROLLER_USAGE
	{
		UNKNOWN_MOTOR_CONTROLLER = -1,
		INTAKE_MECHANISM_MAIN_WHEEL,
		MAX_MOTOR_CONTROLLER
	};

	enum ANALOG_INPUT_USAGE
	{
		UNKNOWN_ANALOG_INPUT = -1,
		MAX_ANALOG_INPUT
	};

	enum LIMELIGHT_USAGE
	{
		UNKNOWN_LIMELIGHT = -1,
		MAX_LIMELIGHT
	};

	enum DIGITAL_INPUT_USAGE
	{
		UNKNOWN_DIGITAL_INPUT = -1,
		MAX_DIGITAL_INPUT
	};

	enum SWERVE_MODULE_USAGE
	{
		UNKNOWN_SWERVE_MODULE = -1,
		MAX_SWERVE_MODULE
	};

	enum CANCODER_USAGE
	{
		UNKNOWN_CANCODER = -1,
		MAX_CANCODER
	};

	enum SOLENOID_USAGE
	{
		UNKNOWN_SOLENOID = -1,
		INTAKE_MECHANISM_PUSH_SOLENOID,
		EJECT_MECHANISM_EJECT_SOLENOID,
		MAX_SOLENOID
	};

	enum SERVO_USAGE
	{
		UNKNOWN_SERVO = -1,
		INTAKE_MECHANISM_ARM_SERVO,
		MAX_SERVO
	};

	enum COLOR_SENSOR_USAGE
	{
		UNKNOWN_COLOR_SENSOR = -1,
		MAX_COLOR_SENSOR
	};

	enum CAMERA_USAGE
	{
		UNKNOWN_CAMERA = -1,
		MAX_CAMERA
	};

	enum ROBORIO_USAGE
	{
		UNKNOWN_ROBORIO = -1,
		MAX_ROBORIO
	};

	enum LED_USAGE
	{
		UNKNOWN_LED = -1,
		MAX_LED
	};

	enum TALONTACH_USAGE
	{
		UNKNOWN_TALONTACH = -1,
		MAX_TALONTACH
	};

	enum MOTOR_CONTROL_DATA_USAGE
	{
		UNKNOWN_MOTOR_CONTROL_DATA = -1,
		MAX_MOTOR_CONTROL_DATA
	};

	enum STATE_USAGE
	{
		UNKNOWN_STATE = -1,
		INTAKE_MECHANISM_FORWARD,
		INTAKE_MECHANISM_MAX_TRAVEL,
		INTAKE_MECHANISM_MIN_TRAVEL,
		INTAKE_MECHANISM_REVERSE,
		INTAKE_MECHANISM_STOP,
		INTAKE_MECHANISM_PANIC,
		EJECT_MECHANISM_STATE_1,
		MAX_STATE
	};

private:
	RobotElementNames() = delete;
	~RobotElementNames() = delete;
};
