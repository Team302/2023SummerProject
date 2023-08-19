$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

#pragma once

#include <string>
#include <memory>

// FRC Includes
#include <networktables/NetworkTable.h>

// Team 302 Includes
$$_INCLUDES_$$

class $$_MECHANISM_NAME_$$
{
public:
    $$_MECHANISM_NAME_$$($$_CONSTRUCTOR_ARGS_$$);
    void Initialize();
    void Cyclic();

private:
    void CheckForTuningEnabled();
    void ReadTuningParamsFromNT();
    void PushTuningParamsToNT();

    $$_COMPONENTS_$$

    $$_TUNABLE_PARAMETERS_$$

    std::string m_ntName = "$$_MECHANISM_NAME_$$";
    bool m_tuning = false;
    std::shared_ptr<nt::NetworkTable> m_table;
};