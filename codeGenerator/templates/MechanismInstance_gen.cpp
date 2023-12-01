$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

// FRC Includes
#include <networktables/NetworkTableInstance.h>
#include "hw/interfaces/IDragonMotorController.h"

#include "$$_MECHANISM_INSTANCE_NAME_$$_gen.h"

$$_MECHANISM_INSTANCE_NAME_$$_gen::$$_MECHANISM_INSTANCE_NAME_$$_gen()
{
    m_ntName = "$$_MECHANISM_INSTANCE_NAME_$$";
    $$_OBJECT_CREATION_$$
}

void $$_MECHANISM_INSTANCE_NAME_$$_gen::Initialize(RobotConfigMgr::RobotIdentifier robotFullName)
{
    m_table = nt::NetworkTableInstance::GetDefault().GetTable(m_ntName);
    m_table.get()->PutBoolean("Enable Tuning for $$_MECHANISM_INSTANCE_NAME_$$?", m_tuning);

    $$_ELEMENT_INITIALIZATION_$$
}