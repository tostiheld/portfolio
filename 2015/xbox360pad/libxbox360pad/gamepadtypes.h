#ifndef GAMEPADTYPES_H
#define GAMEPADTYPES_H

#include <stdint.h>

enum ButtonState : uint8_t
{
    ButtonReleased = 0,
    ButtonPressed  = 1
};

enum LedState
{
    LedAllOff        = 0x00,
    LedAllBlink      = 0x01,
    LedFirstOnFlash  = 0x02,
    LedSecondOnFlash = 0x03,
    LedThirdOnFlash  = 0x04,
    LedFourthOnFlash = 0x05,
    LedFirstOn       = 0x06,
    LedSecondOn      = 0x07,
    LedThirdOn       = 0x08,
    LedFourthOn      = 0x09,
    LedRotate        = 0x0A,
    LedBlink         = 0x0B,
    LedSlowBlink     = 0x0C,
    LedAlternate     = 0x0D
};

struct GamePadButtons
{
    ButtonState A;
    ButtonState B;
    ButtonState Y;
    ButtonState X;

    ButtonState Back;
    ButtonState Start;

    ButtonState LeftShoulder;
    ButtonState RightShoulder;

    ButtonState LeftStick;
    ButtonState RightStick;

    ButtonState XboxButton;
};

struct GamePadDPad
{
    ButtonState Down;
    ButtonState Left;
    ButtonState Right;
    ButtonState Up;
};

struct GamePadThumbsticks
{
    int16_t LeftX;
    int16_t LeftY;
    int16_t RightX;
    int16_t RightY;
};

struct GamePadTriggers
{
    uint8_t Left;
    uint8_t Right;
};

#endif // GAMEPADTYPES_H
