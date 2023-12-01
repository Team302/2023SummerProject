$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

#include "configs/RobotConfigMgr.h"
#include "configs/RobotConfig.h"
$$_ROBOT_CONFIG_INCLUDES_$$

RobotConfigMgr *RobotConfigMgr::m_instance = nullptr;
RobotConfigMgr *RobotConfigMgr::GetInstance()
{
    if (RobotConfigMgr::m_instance == nullptr)
    {
        RobotConfigMgr::m_instance = new RobotConfigMgr();
    }
    return RobotConfigMgr::m_instance;
}

RobotConfigMgr::RobotConfigMgr() : m_config(nullptr)
{
}

void RobotConfigMgr::InitRobot(RobotIdentifier id)
{
    switch (id)
    {
        $$_ROBOT_CONFIGURATION_CREATION_$$

    default:
        break;
    }

    if (m_config != nullptr)
    {
        m_config->BuildRobot();
    }
}
