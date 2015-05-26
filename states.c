#include "states.h"
#include "hardware.h"

States decide_State(States lastState, Events event)
{
    switch (event)
    {
        case eNone:
            return lastState;
        case eClap:
            if (lastState == sStop)
            {
                return sDrive;
            }
            else if (lastState == sDrive)
            {
                return sStop;
            }
            return sError;
        case eObjectLeft:
            return sDangerLeft;
        case eObjectRight:
            return sDangerRight;
        default:
            return sError;
    }
    
    return sError;
}

void behaviour_Stop(void)
{
    
}

void behaviour_Drive(void)
{
    
}

void behaviour_DangerLeft(void)
{
    
}

void behaviour_DangerRight(void)
{
    
}
