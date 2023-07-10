
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

#include <string>
#include "hw/builders/ServoBuilder.h"

using std::string;

ServoBuilder::ServoBuilder() : m_networkTableName(string("DefaultServo")),
                               m_usage(ServoUsage::SERVO_USAGE::UNKNOWN_SERVO_USAGE),
                               m_pwmID(0),
                               m_minAngle(units::angle::degree_t(0.0)),
                               m_maxAngle(units::angle::degree_t(360.0))
{
    Reset();
}

ServoBuilder::~ServoBuilder()
{
}

DragonServo *ServoBuilder::Commit()
{
    return new DragonServo(m_usage, m_pwmID, m_minAngle, m_maxAngle);
}

void ServoBuilder::Reset()
{
    m_networkTableName = string("DefaultServo");
    m_usage = ServoUsage::SERVO_USAGE::UNKNOWN_SERVO_USAGE;
    m_pwmID = 0;
    m_minAngle = units::angle::degree_t(0.0);
    m_maxAngle = units::angle::degree_t(360.0);
}
void ServoBuilder::SetNetworkTableName(std::string ntName)
{
    // TODO:  add validation
    m_networkTableName = ntName;
}

void ServoBuilder::SetPwmId(int id)
{
    // TODO:  add validation
    m_pwmID = id;
}

void ServoBuilder::SetUsage(ServoUsage::SERVO_USAGE usage)
{
    m_usage = usage;
}

void ServoBuilder::SetMinAngle(units::angle::degree_t angle)
{
    // TODO:  add validation
    m_minAngle = angle;
}

void ServoBuilder::SetMaxAngle(units::angle::degree_t angle)
{
    // TODO:  add validation
    m_maxAngle = angle;
}
