$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

#pragma once

#include <string>
#include <memory>

// FRC Includes
#include <networktables/NetworkTable.h>
#include "configs/RobotConfigMgr.h"

$$_INCLUDE_FILES_$$

class $$_MECHANISM_NAME_$$
{
public:
    $$_MECHANISM_NAME_$$();
    ~$$_MECHANISM_NAME_$$() = default;
    virtual void Initialize(RobotConfigMgr::RobotIdentifier robotFullName) = 0;
    void Cyclic();

    $$_MECHANISM_ELEMENTS_$$

protected:
    std::string m_ntName;
    bool m_tuning = false;
    std::shared_ptr<nt::NetworkTable> m_table;

private:
    void CheckForTuningEnabled();
    void ReadTuningParamsFromNT();
    void PushTuningParamsToNT();

    $$_TUNABLE_PARAMETERS_$$
};