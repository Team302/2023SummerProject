$$COPYRIGHT$$
$$GENERATION_NOTICE$$

#pragma once
#include "configs/RobotConfig.h"
#include "hw/DragonTalonFX.h"
$$MECH_INCLUDES$$

class RobotConfig$$ROBOT_ID$$ : public RobotConfig
{
public:
    RobotConfig$$ROBOT_ID$$() = default;
    ~RobotConfig$$ROBOT_ID$$() = default;

protected:
    // actuators
    void DefineMotorControllers() override;
    void DefineSolenoids() override;

    // sensors
    void DefineCANSensors() override;

    // mechanisms
    void DefineMechanisms() override;

private:
    $$MOTOR_INIT$$
    $$SOLENOID_INIT$$
    $$CANCODER_INIT$$
    $$MECHANISM_INIT$$
};