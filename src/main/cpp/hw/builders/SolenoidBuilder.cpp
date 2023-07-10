
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

#include "hw/builders/SolenoidBuilder.h"

using frc::PneumaticsModuleType;
using std::string;

SolenoidBuilder::SolenoidBuilder(PneumaticsModuleType pneumaticType, int pcmID) : m_networkTableName(string("defaultSolenoid")),
                                                                                  m_pneumaticType(pneumaticType),
                                                                                  m_pcmID(pcmID),
                                                                                  m_usage(SolenoidUsage::SOLENOID_USAGE::UNKNOWN_SOLENOID_USAGE),
                                                                                  m_channel(0),
                                                                                  m_secondChannel(0),
                                                                                  m_isDouble(false),
                                                                                  m_reversed(false)
{
    Reset();
}

SolenoidBuilder::~SolenoidBuilder()
{
}

DragonSolenoid *SolenoidBuilder::Commit()
{
    if (m_isDouble)
    {
        return new DragonSolenoid(m_networkTableName, m_usage, m_pcmID, m_pneumaticType, m_channel, m_secondChannel, m_reversed);
    }
    return new DragonSolenoid(m_networkTableName, m_usage, m_pcmID, m_pneumaticType, m_channel, m_reversed);
}

void SolenoidBuilder::Reset()
{
    m_networkTableName = string("");
    m_usage = SolenoidUsage::SOLENOID_USAGE::UNKNOWN_SOLENOID_USAGE;
    m_channel = 0;
    m_secondChannel = 0;
    m_isDouble = false;
    m_reversed = false;
}

void SolenoidBuilder::SetNetworkTableName(std::string ntName)
{
    m_networkTableName = ntName;
}

void SolenoidBuilder::SetUsage(SolenoidUsage::SOLENOID_USAGE usage)
{
    m_usage = usage;
}

void SolenoidBuilder::SetChannel(int chan)
{
    // TODO:  add validation
    m_channel = chan;
}

void SolenoidBuilder::SetSecondChannel(int chan)
{
    // TODO:  add validation
    m_isDouble = true;
    m_secondChannel = chan;
}

void SolenoidBuilder::SetReversed(bool isRev)
{
    m_reversed = isRev;
}