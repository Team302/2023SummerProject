
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

// C++ Includes

// Team 302 includes
#include "hw/factories/DragonControlToCTREProAdapterFactory.h"
#include "hw/ctreadapters/pro/DragonControlToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonPercentOutputToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonPositionDegreeToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonPositionInchToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonTicksToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonTrapezoidToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonVelocityDegreeToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonVelocityInchToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonVelocityRPSToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonVelocityToCTREProAdapter.h"
#include "hw/ctreadapters/pro/DragonVoltageToCTREProAdapter.h"
#include "utils/logging/Logger.h"

using namespace std;

DragonControlToCTREProAdapterFactory *DragonControlToCTREProAdapterFactory::m_factory = nullptr;

//=======================================================================================
/// Method: GetInstance
/// @brief  Get the factory singleton
/// @return DragonServoFactory*    pointer to the factory
//=======================================================================================
DragonControlToCTREProAdapterFactory *DragonControlToCTREProAdapterFactory::GetFactory()
{
    if (DragonControlToCTREProAdapterFactory::m_factory == nullptr)
    {
        DragonControlToCTREProAdapterFactory::m_factory = new DragonControlToCTREProAdapterFactory();
    }
    return DragonControlToCTREProAdapterFactory::m_factory;
}

DragonControlToCTREProAdapter *DragonControlToCTREProAdapterFactory::CreateAdapter(std::string networkTableName,
                                                                                   int controllerSlot,
                                                                                   const ControlData &controlInfo,
                                                                                   const DistanceAngleCalcStruc &calcStruc,
                                                                                   ctre::phoenixpro::hardware::TalonFX &controller)
{
    switch (controlInfo.GetMode())
    {
    case ControlModes::CONTROL_TYPE::PERCENT_OUTPUT:
        return new DragonPercentOutputToCTREProAdapter(networkTableName,
                                                       controllerSlot,
                                                       controlInfo,
                                                       calcStruc,
                                                       controller);
        break;
    case ControlModes::CONTROL_TYPE::POSITION_ABS_TICKS:
        return new DragonTicksToCTREProAdapter(networkTableName,
                                               controllerSlot,
                                               controlInfo,
                                               calcStruc,
                                               controller);
        break;

    case ControlModes::CONTROL_TYPE::POSITION_DEGREES:
        return new DragonPositionDegreeToCTREProAdapter(networkTableName,
                                                        controllerSlot,
                                                        controlInfo,
                                                        calcStruc,
                                                        controller);
        break;

    case ControlModes::CONTROL_TYPE::POSITION_INCH:
        return new DragonPositionInchToCTREProAdapter(networkTableName,
                                                      controllerSlot,
                                                      controlInfo,
                                                      calcStruc,
                                                      controller);
        break;

    case ControlModes::CONTROL_TYPE::POSITION_DEGREES_ABSOLUTE:
        return new DragonPercentOutputToCTREProAdapter(networkTableName,
                                                       controllerSlot,
                                                       controlInfo,
                                                       calcStruc,
                                                       controller);
        break;

    case ControlModes::CONTROL_TYPE::TRAPEZOID:
        return new DragonTrapezoidToCTREProAdapter(networkTableName,
                                                   controllerSlot,
                                                   controlInfo,
                                                   calcStruc,
                                                   controller);
        break;

    case ControlModes::CONTROL_TYPE::VELOCITY_DEGREES:
        return new DragonVelocityDegreeToCTREProAdapter(networkTableName,
                                                        controllerSlot,
                                                        controlInfo,
                                                        calcStruc,
                                                        controller);
        break;

    case ControlModes::CONTROL_TYPE::VELOCITY_INCH:
        return new DragonVelocityInchToCTREProAdapter(networkTableName,
                                                      controllerSlot,
                                                      controlInfo,
                                                      calcStruc,
                                                      controller);
        break;

    case ControlModes::CONTROL_TYPE::VELOCITY_RPS:
        return new DragonVelocityRPSToCTREProAdapter(networkTableName,
                                                     controllerSlot,
                                                     controlInfo,
                                                     calcStruc,
                                                     controller);
        break;

    case ControlModes::CONTROL_TYPE::VOLTAGE:
        return new DragonVoltageToCTREProAdapter(networkTableName,
                                                 controllerSlot,
                                                 controlInfo,
                                                 calcStruc,
                                                 controller);
        break;

    default:
        string msg{"Invalid control data "};
        msg += to_string(controller.GetDeviceID());
        Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, string("DragonControlToCTREProAdapterFactory"), string("CreateAdapter"), msg);
        return new DragonPercentOutputToCTREProAdapter(networkTableName,
                                                       controllerSlot,
                                                       controlInfo,
                                                       calcStruc,
                                                       controller);
        break;
    }
    return nullptr;
}
