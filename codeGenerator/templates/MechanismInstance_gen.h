$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

#pragma once

#include <string>
#include <memory>

// FRC Includes
#include <networktables/NetworkTable.h>

#include "mechanisms/base/StateMgr.h"
#include "mechanisms/$$_MECHANISM_NAME_$$/generated/$$_MECHANISM_NAME_$$.h"

class $$_MECHANISM_INSTANCE_NAME_$$_gen : public $$_MECHANISM_NAME_$$, public StateMgr
{
public:
    $$_MECHANISM_INSTANCE_NAME_$$_gen();
    void Initialize(RobotConfigMgr::RobotIdentifier robotFullName);
};