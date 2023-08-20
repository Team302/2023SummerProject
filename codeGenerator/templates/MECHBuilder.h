$$_COPYRIGHT_$$
$$_GEN_NOTICE_$$

#pragma once

#include <$$_INCLUDE_PATH_$$/$$_MECHANISM_NAME_$$.h>

class $$_MECHANISM_NAME_$$Builder
{
public:
    $$_MECHANISM_NAME_$$Builder($$_CONSTRUCTOR_ARGS_$$);
    ~$$_MECHANISM_NAME_$$Builder() = default;

    $$_MECHANISM_NAME_$$Builder *GetBuilder();
    $$_MECHANISM_NAME_$$ *CreateNew$$_MECHANISM_NAME_$$($$_MECHANISM_ARGS_$$);

private:
    static $$_MECHANISM_NAME_$$Builder *m_instance;

    $$_DEFAULT_VALUES_$$
};