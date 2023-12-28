#pragma once

// C++ Includes

// FRC includes

// Team 302 includes

// Third Party Includes

//==================================================================================
/// <summary>
/// Class:          AnalogButton
/// Description:    This is treats an analog axis as a button.
/// </summary>
//==================================================================================
class theButton
{
public:
    static bool A_BUTTON_IsPressed;
    static bool B_BUTTON_IsPressed;
    static bool X_BUTTON_IsPressed;
    static bool Y_BUTTON_IsPressed;

    static void press(int b);
};