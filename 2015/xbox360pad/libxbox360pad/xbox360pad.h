#ifndef XBOX360PAD_H
#define XBOX360PAD_H

#include <stdexcept>

#include "gamepadstate.h"
#include "xbox360padservice.h"

class Xbox360Pad
{
private:
    Xbox360PadService hardware;
    GamePadState oldState;

public:
    Xbox360Pad();

    GamePadState& getState(uint32_t playerindex);
    void setLeds(uint32_t playerindex, LedState state);
    void setRumble(uint32_t playerindex, uint8_t bigw, uint8_t smallw);
};

#endif // XBOX360PAD_H
