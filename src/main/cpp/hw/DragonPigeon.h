
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
#include <memory>
#include <ctre/phoenix/sensors/WPI_PigeonIMU.h>
#include <ctre/phoenix/sensors/WPI_Pigeon2.h>
#include <ctre/Phoenix.h>

#include "configs/RobotElementNames.h"
#include "hw/interfaces/IDragonPigeon.h"

class DragonPigeon : public IDragonPigeon
{
public:
    DragonPigeon(int canID,
                 std::string canBusName,
                 RobotElementNames::CANCODER_USAGE usage,
                 double rotation);
    DragonPigeon() = delete;
    virtual ~DragonPigeon() = default;

    units::angle::degree_t GetPitch() override;
    units::angle::degree_t GetRoll() override;
    units::angle::degree_t GetYaw() override;
    void ReZeroPigeon(units::angle::degree_t angle, int timeoutMs = 0) override;

private:
    ctre::phoenix::sensors::WPI_PigeonIMU m_pigeon;
    RobotElementNames::CANCODER_USAGE m_usage;

    double m_initialYaw;
    double m_initialPitch;
    double m_initialRoll;

    // these methods correct orientation, but do not apply the initial offsets
    double GetRawYaw();
    double GetRawRoll();
    double GetRawPitch();
};
