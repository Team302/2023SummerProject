$$COPYRIGHT$$

$$GENERATION_NOTICE$$

#include <string>

#include "configs/RobotConfig$$ROBOT_ID$$.h"
#include "configs/usages/CanSensorUsage.h"
#include "configs/usages/MotorControllerUsage.h"
#include "hw/DragonCanCoder.h"
#include "hw/DragonTalonFX.h"
#include "mechanisms/example/decoratormods/Example.h"
#include "mechanisms/example/generated/ExampleGen.h"

constexpr char canBusName[] = "Canivore";

using std::string;

void RobotConfig$$ROBOT_ID$$::DefineMotorControllers()
{
    /*
        Will be repeated for each motor, needs to create new motor and configure according to data from GUI
        Can probably use a snippet
    */
    $$MOTOR_DEFINITION$$
}

void RobotConfig$$ROBOT_ID$$::DefineSolenoids()
{
    /*
        Will be repeated for each solenoid, needs to create new solenoid and configure according to data from GUI
        Can probably use a snippet
    */
    $$SOLENOID_DEFINITION$$
}

void RobotConfig$$ROBOT_ID$$::DefineMechanisms()
{
    /*
        Will be repeated for each mechanism, need to create new generated mechanism and regular mechanism
        After, compose generated mechanism by calling appropriate functions (AddMotor(*m_motor1))
        Can probably use snippet
    */
    $$MECHANISM_DEFINITION$$
}

void RobotConfig$$ROBOT_ID$$::DefineCANSensors()
{
    /*
        Will be repeated for each cancoder, needs to create new cancoder and configure according to data from GUI
        Can probably use a snippet
    */
    $$CANCODER_DEFINITION$$
}