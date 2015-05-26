#include "states.h"

void decide_State(States lastState, Events event)
{
    switch (event)
    {
        case eNone:
            currentState = lastState;
            break;
        case eClap:
            if (lastState == sStop)
            {
                currentState = sDrive;
            }
            else if (lastState == sDrive)
            {
                currentState = sStop;
            }
            else
            {
                currentState = sError;
            }
            break;
        case eObjectLeft:
            currentState = sDangerLeft;
            break;
        case eObjectRight:
            currentState = sDangerRight;
            break;
        default:
            currentState = sError;
            break;
    }
}

void doBehaviours(void)
{
    switch (currentState)
    {
        case sError:
            behaviour_Error();
            break;
        case sStop:
            behaviour_Stop();
            break;
        case sDrive:
            behaviour_Drive();
            break;
        case sDangerLeft:
            behaviour_Danger(leftDistance);
            break;
        case sDangerRight:
            behaviour_Danger(rightDistance);
            break;
        default:
            behaviour_Error();
            
    }
}

void behaviour_Error(void)
{
    stopMotors();
    ShowLCD("ERROR", "");
}

void behaviour_Stop(void)
{
    if (isDriving())
    {
        stopMotors();
    }
}

void behaviour_Drive(void)
{
    if (!isDriving())
    {
        setPower(default_Speed, default_Speed);
    }
}

// TODO: add constant to hardware: max_Distance
// TODO: add constant to hardware: min_Distance

void behaviour_Danger(uint8_t distance)
{
    // how strong do we have to adjust?
    float factor = distance / max_Distance
    uint8_t speed = round((factor * default_Speed)) + default_Speed;
    
    if (currentState == sDangerLeft)
    {
        setPower(speed, default_Speed);
    }
    else if (currentState == sDangerRight)
    {
        setPower(default_Speed, speed);
    }
}

int round(float myfloat)
{
  double integral;
  float fraction = (float)modf(myfloat, &integral);
 
  if (fraction >= 0.5)
    integral += 1;
  if (fraction <= -0.5)
    integral -= 1;
 
  return (int)integral;
}
