
//====================================================================================================================================================
/// Copyright 2023 Lake Orion Robotics FIRST Team 302
///
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
/// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
/// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
/// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
/// OR OTHER DEALINGS IN THE SOFTWARE.
//====================================================================================================================================================

#pragma once

// C++ Includes
#include <string>

// Team 302 includes
#include "hw/DistanceAngleCalcStruc.h"
#include "hw/DragonTalonFX.h"

#include "ctre/phoenixpro/controls/ControlRequest.hpp"
#include "ctre/phoenixpro/TalonFX.hpp"

class DragonControlToCTREProAdapter;

class DragonControlToCTREProAdapterFactory
{
public:
    static DragonControlToCTREProAdapterFactory *GetFactory();
    DragonControlToCTREProAdapter *CreateAdapter(std::string networkTableName,
                                                 int controlSlot,
                                                 const ControlData &controlInfo,
                                                 const DistanceAngleCalcStruc &calcStruc,
                                                 ctre::phoenixpro::hardware::TalonFX &controller);
    ctre::phoenixpro::controls::ControlRequest *CreateControlRequest(int controllerSlot,
                                                                     const ControlData &controlInfo,
                                                                     const DistanceAngleCalcStruc &calcStruc,
                                                                     ctre::phoenixpro::hardware::TalonFX *controller);

private:
    DragonControlToCTREProAdapterFactory() = default;
    ~DragonControlToCTREProAdapterFactory() = default;

    static DragonControlToCTREProAdapterFactory *m_factory;
};