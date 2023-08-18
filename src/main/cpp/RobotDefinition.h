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

// C++ Includes
#include <string>
#include <vector>
#include <any>
#include <map>

// Team 302 Includes
#include <RobotDefinitions.h>

class RobotDefinition
{
public:
    /// strings will be used instead of mechanism and sensor parents classes until those are created
    // RobotDefinition(std::vector<Mechanism> mechs, std::vector<Sensor> sensors);
    RobotDefinition(std::vector<std::pair<RobotDefinitions::Component, std::string>> mechs, std::vector<std::pair<RobotDefinitions::Component, std::string>> sensors);
    ~RobotDefinition() = default;

    /// @brief Get a component (mechanism, sensor, solenoid, etc.) from a robot definition
    /// @param component Which component to get from a definition
    /// @return Returns the specfied component
    std::any GetComponent(RobotDefinitions::Component component) { return m_componentMap[component]; };

private:
    // std::vector<Mechanism> m_mechs;
    std::vector<std::string> m_mechs;
    // std::vector<Sensor> m_sensors;
    std::vector<std::string> m_sensors;
    std::map<RobotDefinitions::Component, std::any> m_componentMap;
};
