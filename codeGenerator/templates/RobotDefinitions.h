$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

#pragma once

// C++ Includes
#include <map>
#include <vector>

// Team 302 Includes
#include <RobotDefinition.h>

/*
    Robot variants will be tied to team number
    For instance, RobotVariants::CompBot = 302 would be one entry in enum
*/
enum RobotVariants
{
    $$_ROBOT_VARIANTS_$$
}

class RobotDefinitions
{
public:
    static RobotDefintion *GetRobotDefinition(); // may not even need this argument, can find team number in this class as well

private:
    /*
        This tag will just be a bunch of functions that return a robot definition for the specified robot
        For example:
        RobotDefintion *Get302Definition();
        RobotDefintion *Get3Definition();
        etc...
    */
    $$_ROBOT_VARIANT_CREATION_$$
};