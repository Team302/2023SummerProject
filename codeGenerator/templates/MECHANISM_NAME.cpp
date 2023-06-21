$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

#include <$$_INCLUDE_PATH_$$/$$_MECHANISM_NAME_$$.h>

$$_MECHANISM_NAME_$$::$$_MECHANISM_NAME_$$()
{

}

void $$_MECHANISM_NAME_$$::Initialize()
{

}
void $$_MECHANISM_NAME_$$::Cyclic()
{
    CheckForTuningEnabled();
    if(m_tuning)
    {
        ReadTuningParamsFromNT();
    }
}
void $$_MECHANISM_NAME_$$::CheckForTuningEnabled()
{
    //set m_tuning from NT
}
void $$_MECHANISM_NAME_$$::ReadTuningParamsFromNT()
{
    //set tunable params from NT
}