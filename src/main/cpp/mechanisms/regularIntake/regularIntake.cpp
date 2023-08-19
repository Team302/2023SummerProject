
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
//==============================================================


// FRC Includes
#include <networktables/NetworkTableInstance.h>

#include <mechanisms/regularIntake/regularIntake.h>

regularIntake::regularIntake(double UNKNOWN_pGain, double UNKNOWN_iGain, double UNKNOWN_dGain, double UNKNOWN_fGain, double UNKNOWN_iZone)
{
    m_UNKNOWN_pGain = UNKNOWN_pGain;
	m_UNKNOWN_iGain = UNKNOWN_iGain;
	m_UNKNOWN_dGain = UNKNOWN_dGain;
	m_UNKNOWN_fGain = UNKNOWN_fGain;
	m_UNKNOWN_iZone = UNKNOWN_iZone;
}

void regularIntake::Initialize()
{
    m_table = nt::NetworkTableInstance::GetDefault().GetTable(m_ntName);
    m_table.get()->PutBoolean("Enable Tuning for regularIntake?", m_tuning);
}

void regularIntake::Cyclic()
{
    CheckForTuningEnabled();
    if (m_tuning)
    {
        ReadTuningParamsFromNT();
    }
}

void regularIntake::CheckForTuningEnabled()
{
    bool pastTuning = m_tuning;
    m_tuning = m_table.get()->GetBoolean("Enable Tuning for regularIntake?", false);
    if (pastTuning != m_tuning && m_tuning == true)
    {
        PushTuningParamsToNT();
    }
}

void regularIntake::ReadTuningParamsFromNT()
{
    m_UNKNOWN_pGain = m_table.get()->GetNumber("UNKNOWN_pGain", m_UNKNOWN_pGain);
	m_UNKNOWN_iGain = m_table.get()->GetNumber("UNKNOWN_iGain", m_UNKNOWN_iGain);
	m_UNKNOWN_dGain = m_table.get()->GetNumber("UNKNOWN_dGain", m_UNKNOWN_dGain);
	m_UNKNOWN_fGain = m_table.get()->GetNumber("UNKNOWN_fGain", m_UNKNOWN_fGain);
	m_UNKNOWN_iZone = m_table.get()->GetNumber("UNKNOWN_iZone", m_UNKNOWN_iZone);
}

void regularIntake::PushTuningParamsToNT()
{
    m_table.get()->PutNumber("UNKNOWN_pGain", m_UNKNOWN_pGain);
	m_table.get()->PutNumber("UNKNOWN_iGain", m_UNKNOWN_iGain);
	m_table.get()->PutNumber("UNKNOWN_dGain", m_UNKNOWN_dGain);
	m_table.get()->PutNumber("UNKNOWN_fGain", m_UNKNOWN_fGain);
	m_table.get()->PutNumber("UNKNOWN_iZone", m_UNKNOWN_iZone);
}