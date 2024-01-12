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
#include <memory>
#include <string>

// FRC includes

// Team 302 includes
#include "mechanisms/base/BaseMechSolenoid.h"
#include "hw/DragonSolenoid.h"
#include "utils/logging/LoggableItem.h"
#include "utils/logging/Logger.h"

// Third Party Includes

using namespace std;

/// @brief Create a generic mechanism wiht 1 solenoid
/// @param [in] MechanismTypes::MECHANISM_TYPE the type of mechansim
/// @param [in] std::string the name of the file that will set control parameters for this mechanism
/// @param [in] std::string the name of the network table for logging information
/// @param [in] std::shared_ptr<DragonSolenoid> solenoid used by this mechanism
BaseMechSolenoid::BaseMechSolenoid(string networkTableName, DragonSolenoid &solenoid) : LoggableItem(), m_networkTableName(networkTableName), m_solenoid(solenoid)
{
}

/// @brief      Activate/deactivate pneumatic solenoid
/// @param [in] bool - true == extend, false == retract
void BaseMechSolenoid::ActivateSolenoid(bool activate)
{
    m_solenoid.Set(activate);
}

/// @brief      Check if the pneumatic solenoid is activated
/// @return     bool - true == extended, false == retracted
bool BaseMechSolenoid::IsSolenoidActivated() const
{
    return m_solenoid.Get();
}

/// @brief log data to the network table if it is activated and time period has past
void BaseMechSolenoid::LogInformation()
{
    // Logger::GetLogger()->LogData(LOGGER_LEVEL::PRINT, m_networkTableName, "Solenoid", IsSolenoidActivated());
}
