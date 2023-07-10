
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

#include "units/angle.h"

#include "hw/usages/ServoUsage.h"
#include "hw/DragonServo.h"

class ServoBuilder
{
public:
    ServoBuilder();
    ~ServoBuilder();

    void SetNetworkTableName(std::string ntName);
    void SetUsage(ServoUsage::SERVO_USAGE usage);
    void SetPwmId(int id);
    void SetMinAngle(units::angle::degree_t angle);
    void SetMaxAngle(units::angle::degree_t angle);

    DragonServo *Commit();
    void Reset();

private:
    std::string m_networkTableName;
    ServoUsage::SERVO_USAGE m_usage;
    int m_pwmID;
    units::angle::degree_t m_minAngle;
    units::angle::degree_t m_maxAngle;
};
