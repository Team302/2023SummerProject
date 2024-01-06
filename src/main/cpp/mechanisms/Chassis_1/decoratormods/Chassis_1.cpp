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
// Generated on Saturday, January 6, 2024 12:11:53 AM

// C++ Includes
#include <string>

// FRC Includes

// Team 302 includes
#include "PeriodicLooper.h"
#include "mechanisms/Chassis_1/generated/Chassis_1_gen.h"
#include "mechanisms/Chassis_1/decoratormods/Chassis_1.h"

using std::string;

/// @brief  This method constructs the mechanism using composition with its various actuators and sensors.
/// @param controlFileName The control file with the PID constants and Targets for each state
/// @param networkTableName Location for logging information
/// @param motor  Motor in the mechanims - code generator should probably use the usage for the variable name
/// @param otherMotor Same as previous
/// @param solenoid Solenoid in the mechanism - code generator should probably use the usage for the variable name
/// Additional actuators and sensors are also in this list.
Chassis_1::Chassis_1 ( Chassis_1_gen *base ) : Chassis_1_gen(),
	m_Chassis_1 ( base )
{
	// PeriodicLooper::GetInstance()->RegisterAll(*this);
}

// todo not sure what to do with this
/*
bool Chassis_1::IsAtMinPosition(RobotElementNames::ROBOT_ELEMENT_NAMES identifier) const
{
    return m_Chassis_1->IsAtMinPosition(identifier);
}
bool Chassis_1::IsAtMinPosition(RobotElementNames::ROBOT_ELEMENT_NAMES identifier) const
{
    return m_Chassis_1->IsAtMinPosition(identifier);
}
bool Chassis_1::IsAtMaxPosition(RobotElementNames::ROBOT_ELEMENT_NAMES identifier) const
{
    return m_Chassis_1->IsAtMaxPosition(identifier);
}
bool Chassis_1::IsAtMaxPosition(RobotElementNames::ROBOT_ELEMENT_NAMES identifier) const
{
    return m_Chassis_1->IsAtMaxPosition(identifier);
}
*/
