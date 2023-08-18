$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

// Team 302 Includes
#include <RobotDefinitions.h>

/*
    Will need to include all mechanism and sensor h files
    Will also need to include their builders
*/
$$_INCLUDES_$$

/*
    Here's what this should look like once generated: (teamNumber is passed in from BuildDetailsReader where GetRobotDefinition is called)

    switch(teamNumber){
        case $$_ROBOT_ID_$$:
            return Get$$_ROBOT_ID_$$Definition();  //separate functions will make the code more readable during review
        case $$_ROBOT_ID_$$:
            return Get$$_ROBOT_ID_$$Definition();
        default:
            return Get302Defition();  //this could return comp bot or simulation bot
    }
*/
static RobotDefinition *RobotDefinitions::GetRobotDefinition(int teamNumber){
    $$_ROBOT_DEFINITION_SWITCH_$$}

/*
    This is where all of the functions in the switch statement will be created
    Here's what one should look like once generated:

    RobotDefinition* Get302Definition()
    {
        std::vector<std::pair<Component, Mechanism>> mechs = new std::vector<Mechanism>();
        std::vector<std::pair<Component, Sensor>> sensors = new std::vector<Sensor>();

        Mechanism intake = IntakeBuilder::GetBuilder()->CreateNewIntake(args); //or however the builders will be called to create mechs
        mechs.emplace_back(std::make_pair(Component::Intake, intake));

        Mechanism shooter = ShooterBuilder::GetBuilder()->CreateNewShooter(args);
        mechs.emplace_back(std::make_pair(Component::Shooter, shooter));

        Sensor intakeSensor = new BannerSensor(port);
        sensors.emplace_back(std::make_pair(Component::IntakeSensor, intakeSensor));

        return new RobotDefinition(mechs, sensors);
    }
*/
$$_ROBOT_VARIANT_CREATION_FUNCTIONS_$$