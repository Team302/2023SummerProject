
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


#pragma once

#include <string>
#include <memory>

// FRC Includes
#include <networktables/NetworkTable.h>

// Team 302 Includes
#include <hw/DragonTalonSRX.h>

class Turret
{
public:
    Turret(DragonTalonSRX *zAxis, double mainPID_pGain, double mainPID_iGain, double mainPID_dGain, double mainPID_fGain, double mainPID_iZone);
    void Initialize();
    void Cyclic();

private:
    void CheckForTuningEnabled();
    void ReadTuningParamsFromNT();
    void PushTuningParamsToNT();

    DragonTalonSRX *m_zAxis;

    double m_mainPID_pGain;
	double m_mainPID_iGain;
	double m_mainPID_dGain;
	double m_mainPID_fGain;
	double m_mainPID_iZone;

    std::string m_ntName = "Turret";
    bool m_tuning = false;
    std::shared_ptr<nt::NetworkTable> m_table;
};