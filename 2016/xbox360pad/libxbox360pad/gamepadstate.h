#ifndef GAMEPADSTATE_H
#define GAMEPADSTATE_H

#include "gamepadtypes.h"

class GamePadState
{
private:
    GamePadButtons     Buttons;
    GamePadDPad        DPad;
    GamePadThumbsticks Thumbsticks;
    GamePadTriggers    Triggers;

public:
    GamePadState(){}
    GamePadState(GamePadButtons buttons, GamePadDPad dpad,
                 GamePadThumbsticks sticks, GamePadTriggers triggers);

    GamePadButtons     getButtons();
    GamePadDPad        getDPad();
    GamePadThumbsticks getThumbsticks();
    GamePadTriggers    getTriggers();
};

#endif // GAMEPADSTATE_H
