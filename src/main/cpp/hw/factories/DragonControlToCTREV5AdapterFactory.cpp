
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

// C++ Includes
#include <string>

// FRC includes

// Team 302 includes
#include "hw/DistanceAngleCalcStruc.h"
#include "hw/ctreadapters/DragonControlToCtreV5Adapter.h"
#include "hw/factories/DragonControlToCtreV5AdapterFactory.h"
#include "mechanisms/controllers/ControlData.h"
#include "utils/logging/Logger.h"

#include "hw/ctreadapters/DragonControlToCtreV5Adapter.h"
#include <hw/ctreadapters/DragonPercentOutputToCtreV5Adapter.h>
#include <hw/ctreadapters/DragonPositionDegreeToCtreV5Adapter.h>
#include <hw/ctreadapters/DragonPositionInchToCtreV5Adapter.h>
#include <hw/ctreadapters/DragonTicksToCtreV5Adapter.h>
#include <hw/ctreadapters/DragonTrapezoidToCtreV5Adapter.h>
#include <hw/ctreadapters/DragonVelocityDegreeToCtreV5Adapter.h>
#include <hw/ctreadapters/DragonVelocityInchToCtreV5Adapter.h>
#include <hw/ctreadapters/DragonVelocityRPSToCtreV5Adapter.h>
#include <hw/ctreadapters/DragonVoltageToCtreV5Adapter.h>

// Third Party Includes
#include <ctre/phoenix/motorcontrol/can/WPI_BaseMotorController.h>

using namespace std;

DragonControlToCtreV5AdapterFactory *DragonControlToCtreV5AdapterFactory::m_factory = nullptr;

//=======================================================================================
/// Method: GetInstance
/// @brief  Get the factory singleton
/// @return DragonServoFactory*    pointer to the factory
//=======================================================================================
DragonControlToCtreV5AdapterFactory *DragonControlToCtreV5AdapterFactory::GetFactory()
{
    if (DragonControlToCtreV5AdapterFactory::m_factory == nullptr)
    {
        DragonControlToCtreV5AdapterFactory::m_factory = new DragonControlToCtreV5AdapterFactory();
    }
    return DragonControlToCtreV5AdapterFactory::m_factory;
}

DragonControlToCtreV5Adapter *DragonControlToCtreV5AdapterFactory::CreatePercentOuptutAdapter(std::string networkTableName,
                                                                                              ctre::phoenix::motorcontrol::can::WPI_BaseMotorController *controller)
{

    ControlData controlInfo;
    DistanceAngleCalcStruc calcStruc;
    return CreateAdapter(networkTableName, 0, &controlInfo, calcStruc, controller);
}
DragonControlToCtreV5Adapter *DragonControlToCtreV5AdapterFactory::CreateAdapter(std::string networkTableName,
                                                                                 int controllerSlot,
                                                                                 ControlData *controlInfo,
                                                                                 DistanceAngleCalcStruc calcStruc,
                                                                                 ctre::phoenix::motorcontrol::can::WPI_BaseMotorController *controller)
{
    if (controlInfo != nullptr && controller != nullptr)
    {
        switch (controlInfo->GetMode())
        {
        case ControlModes::CONTROL_TYPE::PERCENT_OUTPUT:
            return new DragonPercentOutputToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
            break;

        case ControlModes::CONTROL_TYPE::POSITION_ABS_TICKS:
            return new DragonTicksToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
            break;

        case ControlModes::CONTROL_TYPE::POSITION_DEGREES:
            return new DragonPositionDegreeToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
            break;

        case ControlModes::CONTROL_TYPE::POSITION_INCH:
            return new DragonPositionInchToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
            break;

        case ControlModes::CONTROL_TYPE::POSITION_DEGREES_ABSOLUTE:
            return new DragonPercentOutputToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
            break;

        case ControlModes::CONTROL_TYPE::TRAPEZOID:
            return new DragonTrapezoidToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
            break;

        case ControlModes::CONTROL_TYPE::VELOCITY_DEGREES:
            return new DragonVelocityDegreeToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
            break;

        case ControlModes::CONTROL_TYPE::VELOCITY_INCH:
            return new DragonVelocityInchToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
            break;

        case ControlModes::CONTROL_TYPE::VELOCITY_RPS:
            return new DragonVelocityRPSToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
            break;

        case ControlModes::CONTROL_TYPE::CURRENT:
            break;

        case ControlModes::CONTROL_TYPE::MOTION_PROFILE:
            break;

        case ControlModes::CONTROL_TYPE::MOTION_PROFILE_ARC:
            break;

        case ControlModes::CONTROL_TYPE::VOLTAGE:
            return new DragonVoltageToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
            break;

        default:
            string msg{"Invalid control data "};
            msg += to_string(controller->GetDeviceID());
            Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, string("DragonControlToCtreV5AdapterFactory"), string("CreateAdapter"), msg);
            return new DragonPercentOutputToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
            break;
        }
    }
    string msg{"Invalid contrrol information "};
    Logger::GetLogger()->LogData(LOGGER_LEVEL::ERROR_ONCE, string("DragonControlToCtreV5AdapterFactory"), string("CreateAdapter"), msg);
    return new DragonPercentOutputToCtreV5Adapter(networkTableName, controllerSlot, controlInfo, calcStruc, controller);
}
