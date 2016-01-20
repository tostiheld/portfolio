#include "xbox360pad.h"

Xbox360Pad::Xbox360Pad()
{
}

GamePadState& Xbox360Pad::getState(uint32_t playerindex)
{
    if (playerindex > hardware.getConnectedPadCount())
    {
        throw std::runtime_error("Player index too high");
    }

    Xbox360PadState state;
    if (!hardware.getState(playerindex, &state))
    {
        return oldState;
    }

    GamePadButtons buttons =
    {
        (ButtonState)((state.Buttons >> XBOX360PAD_BUTTON_A) & 1),
        (ButtonState)((state.Buttons >> XBOX360PAD_BUTTON_B) & 1),
        (ButtonState)((state.Buttons >> XBOX360PAD_BUTTON_Y) & 1),
        (ButtonState)((state.Buttons >> XBOX360PAD_BUTTON_X) & 1),

        (ButtonState)((state.Buttons >> XBOX360PAD_BUTTON_BACK) & 1),
        (ButtonState)((state.Buttons >> XBOX360PAD_BUTTON_START) & 1),

        (ButtonState)((state.Buttons >> XBOX360PAD_LEFTBUMPER) & 1),
        (ButtonState)((state.Buttons >> XBOX360PAD_RIGHTBUMPER) & 1),

        (ButtonState)((state.Buttons >> XBOX360PAD_BUTTON_LEFTSTICK) & 1),
        (ButtonState)((state.Buttons >> XBOX360PAD_BUTTON_RIGHTSTICK) & 1),

        (ButtonState)((state.Buttons >> XBOX360PAD_BUTTON_XBOX) & 1),
    };

    GamePadDPad dpad =
    {
        (ButtonState)((state.Buttons >> XBOX360PAD_DPAD_DOWN) & 1),
        (ButtonState)((state.Buttons >> XBOX360PAD_DPAD_LEFT) & 1),
        (ButtonState)((state.Buttons >> XBOX360PAD_DPAD_RIGHT) & 1),
        (ButtonState)((state.Buttons >> XBOX360PAD_DPAD_UP) & 1)
    };

    GamePadThumbsticks thumbsticks =
    {
        state.LeftStickX,
        state.LeftStickY,
        state.RightStickX,
        state.RightStickY
    };

    GamePadTriggers triggers =
    {
        state.LeftTrigger,
        state.RightTrigger
    };

    oldState = GamePadState(buttons, dpad, thumbsticks, triggers);
    return oldState;
}

void Xbox360Pad::setLeds(uint32_t playerindex, LedState state)
{
    if (playerindex > hardware.getConnectedPadCount())
    {
        throw std::runtime_error("Player index too high");
    }

    Xbox360Led leds;
    leds.LedMode = state;
    hardware.setLeds(playerindex, leds);
}

void Xbox360Pad::setRumble(uint32_t playerindex, uint8_t bigw, uint8_t smallw)
{
    if (playerindex > hardware.getConnectedPadCount())
    {
        throw std::runtime_error("Player index too high");
    }

    Xbox360Rumble rumble;
    rumble.BigWeight = bigw;
    rumble.SmallWeight = smallw;
    hardware.setRumble(playerindex, rumble);
}

