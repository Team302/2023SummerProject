$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

// Team 302 Includes
#include <RobotDefinitions.h>

/*
    Logic for team number:
    We use a compiler argument to determine team number at build/deploy time
    If this argument isn't found, we will use default of comp bot
*/
#ifndef ROBOT_VARIANT
#define ROBOT_VARIANT 302
#endif

/*
    Here's what this should look like once generated:

    switch(teamNumber){
        case $$_ROBOT_ID_$$:
            return Get$$_ROBOT_ID_$$Definition();  //separate functions will make the code more readable during review
        case $$_ROBOT_ID_$$:
            return Get$$_ROBOT_ID_$$Definition();
        default:
            return Get302Defition();  //this could return comp bot or simulation bot
    }
*/
static RobotDefinition *RobotDefinitions::GetRobotDefinition(){
    $$_ROBOT_DEFINITION_CREATION_$$}

/*
    This is where all of the functions in the switch statement will be created
    Here's what one should look like once generated:

    RobotDefinition* Get302Definition()
    {
        std::vector<Mechanism> mechs = new std::vector<Mechanism>();
        std::vector<Sensor> sensors = new std::vector<Sensor>();

        Mechanism intake = IntakeBuilder::GetBuilder()->CreateNewIntake(args); //or however the builders will be called to create mechs
        mechs.emplace_back(intake);

        Mechanism shooter = ShooterBuilder::GetBuilder()->CreateNewShooter(args);
        mechs.emplace_back(shooter);

        Sensor bannerSensor = new DigitalSensor(port);
        sensors.emplace_back(bannerSensor);

        return new RobotDefinition(mechs, sensors);
    }
*/
$$_ROBOT_VARIANT_CREATION__FUNCTIONS_$$