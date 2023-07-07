
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


// FRC Includes
#include <networktables/NetworkTableInstance.h>


#include <mechanisms/SomeMech/SomeMech.h>

SomeMech::SomeMech()
{
}

void SomeMech::Initialize()
{
    m_table = nt::NetworkTableInstance::GetDefault().GetTable(m_ntName);
    m_table.get()->PutBoolean("Enable Tuning for SomeMech?", m_tuning);
}

void SomeMech::Cyclic()
{
    CheckForTuningEnabled();
    if(m_tuning)
    {
        ReadTuningParamsFromNT();
    }
}

void SomeMech::CheckForTuningEnabled()
{
    bool pastTuning = m_tuning;
    m_tuning = m_table.get()->GetBoolean("Enable Tuning for SomeMech?", false);
    if(pastTuning != m_tuning && m_tuning == true)
    {
        PushTuningParamsToNT();
    }
}

void SomeMech::ReadTuningParamsFromNT()
{
    FirstGains_pGain = m_table.get()->GetNumber("FirstGains_pGain", 0);
	FirstGains_iGain = m_table.get()->GetNumber("FirstGains_iGain", 0);
	FirstGains_dGain = m_table.get()->GetNumber("FirstGains_dGain", 0);
	FirstGains_fGain = m_table.get()->GetNumber("FirstGains_fGain", 0);
	FirstGains_iZone = m_table.get()->GetNumber("FirstGains_iZone", 0);
	SecondGains_pGain = m_table.get()->GetNumber("SecondGains_pGain", 0);
	SecondGains_iGain = m_table.get()->GetNumber("SecondGains_iGain", 0);
	SecondGains_dGain = m_table.get()->GetNumber("SecondGains_dGain", 0);
	SecondGains_fGain = m_table.get()->GetNumber("SecondGains_fGain", 0);
	SecondGains_iZone = m_table.get()->GetNumber("SecondGains_iZone", 0);
}

void SomeMech::PushTuningParamsToNT()
{
    FirstGains_pGain = m_table.get()->PutNumber("FirstGains_pGain", FirstGains_pGain);
	FirstGains_iGain = m_table.get()->PutNumber("FirstGains_iGain", FirstGains_iGain);
	FirstGains_dGain = m_table.get()->PutNumber("FirstGains_dGain", FirstGains_dGain);
	FirstGains_fGain = m_table.get()->PutNumber("FirstGains_fGain", FirstGains_fGain);
	FirstGains_iZone = m_table.get()->PutNumber("FirstGains_iZone", FirstGains_iZone);
	SecondGains_pGain = m_table.get()->PutNumber("SecondGains_pGain", SecondGains_pGain);
	SecondGains_iGain = m_table.get()->PutNumber("SecondGains_iGain", SecondGains_iGain);
	SecondGains_dGain = m_table.get()->PutNumber("SecondGains_dGain", SecondGains_dGain);
	SecondGains_fGain = m_table.get()->PutNumber("SecondGains_fGain", SecondGains_fGain);
	SecondGains_iZone = m_table.get()->PutNumber("SecondGains_iZone", SecondGains_iZone);
}