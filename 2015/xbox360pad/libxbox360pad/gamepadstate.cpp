#include "gamepadstate.h"

GamePadState::GamePadState(GamePadButtons buttons, GamePadDPad dpad,
                           GamePadThumbsticks sticks, GamePadTriggers triggers)
{
    Buttons = buttons;
    DPad = dpad;
    Thumbsticks = sticks;
    Triggers = triggers;
}

GamePadButtons GamePadState::getButtons()
{
    return Buttons;
}

GamePadDPad GamePadState::getDPad()
{
    return DPad;
}

GamePadThumbsticks GamePadState::getThumbsticks()
{
    return Thumbsticks;
}

GamePadTriggers GamePadState::getTriggers()
{
    return Triggers;
}
