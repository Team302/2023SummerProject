$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

#pragma once

// C++ Includes
#include <map>
#include <memory>
#include <string>

// FRC includes

// Team 302 includes

// Third Party Includes

class RobotElementNames
{

public:
	// This enum contains values for each of the robot elements such as motors, solenoids, etc
	enum ROBOT_ELEMENT_NAMES
	{
		UNKNOWN_ROBOT_ELEMENT_NAMES = -1,

		$$_ROBOT_ELEMENT_NAMES_ENUMS_$$

			MAX_ROBOT_ELEMENT_NAMES
	};

private:
	RobotElementNames() = delete;
	~RobotElementNames() = delete;
};
