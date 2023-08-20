$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

// Team 302 Includes
#include <$$_INCLUDE_PATH_$$/$$_MECHANISM_NAME_$$Builder.h>

$$_MECHANISM_NAME_$$Builder *$$_MECHANISM_NAME_$$Builder::m_instance = nullptr;
$$_MECHANISM_NAME_$$Builder *$$_MECHANISM_NAME_$$Builder::GetBuilder()
{
    if ($$_MECHANISM_NAME_$$Builder::m_instance == nullptr)
    {
        $$_MECHANISM_NAME_$$Builder::m_instance = new $$_MECHANISM_NAME_$$Builder($$_DEFAULT_ARGS_$$);
    }
    return $$_MECHANISM_NAME_$$Builder::m_instance;
}

$$_MECHANISM_NAME_$$ *$$_MECHANISM_NAME_$$Builder::CreateNew$$_MECHANISM_NAME_$$($$_MECHANISM_ARGS_$$)
{
    return new $$_MECHANISM_NAME_$$($$_NEW_MECHANISM_ARGS_$$);
}