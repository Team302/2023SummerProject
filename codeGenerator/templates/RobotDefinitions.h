$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

#pragma once

// C++ Includes
#include <map>
#include <vector>

// Team 302 Includes
#include <RobotDefinition.h>

/*
    This enum will hold everything that can be in a robot (mechanisms, sensors, etc.)
    Once generated it may look like this:

    enum Components
    {
        Intake,
        Shooter,
        IntakeSensor,
        PDH,
        SomeSolenoid
    }
*/
enum Components
{
    $$_COMPONENTS_ENUM_$$
}

class RobotDefinitions
{
public:
    static RobotDefinition *GetRobotDefinition(int teamNumber); // may not even need this argument, can find team number in this class as well

private:
    /*
        This tag will just be a bunch of functions that return a robot definition for the specified robot
        For example:
        RobotDefintion *Get302Definition();
        RobotDefintion *Get3Definition();
        etc...f
    */
    $$_ROBOT_VARIANT_CREATION_$$
};