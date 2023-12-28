#pragma once

// C++ Includes

// FRC includes
#include "David.h"
// Team 302 includes

// Third Party Includes

//==================================================================================
/// <summary>
/// Class:          AnalogButton
/// Description:    This is treats an analog axis as a button.
/// </summary>
//==================================================================================

bool theButton::A_BUTTON_IsPressed = false;
bool theButton::B_BUTTON_IsPressed = false;
bool theButton::X_BUTTON_IsPressed = false;
bool theButton::Y_BUTTON_IsPressed = false;

void theButton::press(int b)
{
    A_BUTTON_IsPressed = false;
    B_BUTTON_IsPressed = false;
    X_BUTTON_IsPressed = false;
    Y_BUTTON_IsPressed = false;

    static int counter = 0;
    if (b == 0)
    {
        A_BUTTON_IsPressed = true;
        B_BUTTON_IsPressed = false;
        X_BUTTON_IsPressed = false;
        Y_BUTTON_IsPressed = false;
    }
    else if (b == 1)
    {
        A_BUTTON_IsPressed = false;
        B_BUTTON_IsPressed = true;
        X_BUTTON_IsPressed = false;
        Y_BUTTON_IsPressed = false;
    }
    else if (b == 2)
    {
        A_BUTTON_IsPressed = false;
        B_BUTTON_IsPressed = false;
        X_BUTTON_IsPressed = true;
        Y_BUTTON_IsPressed = false;
    }
    else if (b == 3)
    {
        A_BUTTON_IsPressed = false;
        B_BUTTON_IsPressed = false;
        X_BUTTON_IsPressed = false;
        Y_BUTTON_IsPressed = true;
    }
}
