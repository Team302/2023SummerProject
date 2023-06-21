$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

#pragma once

#include <string>
//NT includes

class $$_MECHANISM_NAME_$$
{
    public:
        $$_MECHANISM_NAME_$$();
        void Initialize();
        void Cyclic();

    private:
        void CheckForTuningEnabled();
        void ReadTuningParamsFromNT();

        $$_TUNABLE_PARAMETERS_$$

        std::string m_ntName = "$$_MECHANISM_NAME_$$_Tuning";
        bool m_tuning = false;
};