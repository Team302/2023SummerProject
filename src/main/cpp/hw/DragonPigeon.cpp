
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

#include <ctre/phoenix/Sensors/PigeonIMU.h>
#include <hw/DragonPigeon.h>
#include <memory>

using namespace std;
using namespace ctre::phoenix::sensors;

DragonPigeon::DragonPigeon(
    int canID,
    string canBusName,
    DragonPigeon::PIGEON_USAGE usage,
    DragonPigeon::PIGEON_TYPE type,
    double rotation) : m_pigeon(nullptr),
                       m_pigeon2(nullptr),
                       m_initialYaw(rotation),
                       m_initialPitch(0.0),
                       m_initialRoll(0.0)
{
    if (type == DragonPigeon::PIGEON_TYPE::PIGEON1)
    {
        m_pigeon = new WPI_PigeonIMU(canID);
        m_pigeon->ConfigFactoryDefault();
        m_pigeon->SetYaw(rotation, 0);
        m_pigeon->SetFusedHeading(rotation, 0);

        m_pigeon->SetStatusFramePeriod(PigeonIMU_StatusFrame::PigeonIMU_BiasedStatus_4_Mag, 120, 0);
        m_pigeon->SetStatusFramePeriod(PigeonIMU_StatusFrame::PigeonIMU_CondStatus_11_GyroAccum, 120, 0);
        m_pigeon->SetStatusFramePeriod(PigeonIMU_StatusFrame::PigeonIMU_BiasedStatus_6_Accel, 120, 0); // using fused heading not yaw
    }
    else
    {
        m_pigeon2 = new WPI_Pigeon2(canID, canBusName);
        m_pigeon2->ConfigFactoryDefault();
        m_pigeon2->SetYaw(rotation);

        m_pigeon2->SetStatusFramePeriod(PigeonIMU_StatusFrame::PigeonIMU_BiasedStatus_4_Mag, 120, 0);
        m_pigeon2->SetStatusFramePeriod(PigeonIMU_StatusFrame::PigeonIMU_CondStatus_11_GyroAccum, 120, 0);
        m_pigeon2->SetStatusFramePeriod(PigeonIMU_StatusFrame::PigeonIMU_BiasedStatus_6_Accel, 120, 0); // using fused heading not yaw
    }
}

double DragonPigeon::GetPitch()
{
    return -(GetRawPitch() - m_initialPitch); // TODO: add inversions into code
}

double DragonPigeon::GetRoll()
{
    return GetRawRoll() - m_initialRoll;
}

double DragonPigeon::GetYaw()
{
    return GetRawYaw(); // reset should have taken care of this
    // return GetRawYaw() - m_initialYaw;
}

void DragonPigeon::ReZeroPigeon(double angleDeg, int timeoutMs)
{
    if (m_pigeon != nullptr)
    {
        m_pigeon->SetYaw(angleDeg, timeoutMs);
    }
    else if (m_pigeon2 != nullptr)
    {
        m_pigeon2->SetYaw(angleDeg, timeoutMs);
    }
}

double DragonPigeon::GetRawPitch()
{
    auto pitch = 0.0;
    if (m_pigeon != nullptr)
    {
        pitch = -m_pigeon->GetPitch();
    }
    else if (m_pigeon2 != nullptr)
    {
        pitch = -m_pigeon2->GetPitch();
    }
    return remainder(pitch, 360.0);
}

double DragonPigeon::GetRawRoll()
{
    auto roll = 0.0;
    if (m_pigeon != nullptr)
    {
        roll = -m_pigeon->GetRoll();
    }
    else if (m_pigeon2 != nullptr)
    {
        roll = -m_pigeon2->GetRoll();
    }
    return remainder(roll, 360.0);
}

double DragonPigeon::GetRawYaw()
{
    double yaw = 0.0;
    if (m_pigeon != nullptr)
    {
        yaw = m_pigeon->GetYaw();
    }
    else if (m_pigeon2 != nullptr)
    {
        yaw = m_pigeon2->GetYaw();
    }
    yaw = remainder(yaw, 360.0);

    // normalize it to be between -180 and + 180
    if (yaw > 180)
    {
        yaw -= 360.0;
    }
    else if (yaw < -180)
    {
        yaw += 360.0;
    }
    return yaw;
}
