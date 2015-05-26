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
                showScreenLCD("Drive", "");
            }
            else
            {
                currentState = sStop;
                showScreenLCD("Stop", "");
                //mSleep(1000);
            }
            break;
        case eObjectLeft:
            if (currentState == sDrive)
            {
                currentState = sDangerLeft;
                showScreenLCD("Avoid left", "");
            }
            break;
        case eObjectRight:
            if (currentState == sDrive)
            {
                currentState = sDangerRight;
                showScreenLCD("Avoid right", "");
            }
            break;
        default:
            currentState = sError;
            showScreenLCD("Error", "");
            break;
    }
}

void doBehaviours(void)
{
    setCursorPosLCD(1, 0);
    writeIntegerLengthLCD(currentState, DEC, 1);
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
}

void behaviour_Stop(void)
{
	stopMotors();
}

void behaviour_Drive(void)
{
    if (!isDriving())
    {
        setPower(default_Speed, default_Speed);
    }
}

// TODO: add constant to hardware: max_Distance

void behaviour_Danger(uint8_t distance)
{
    if (distance < 5 ||
        distance > 30)
    {
        return;
    }
    
    // how strong do we have to adjust?
    float factor = distance / max_Distance;
    uint8_t speed = newRound((factor * default_Speed)) + default_Speed;
    
    setCursorPosLCD(1, 3);
    writeIntegerLengthLCD(speed, DEC, 2);
    
    if (currentState == sDangerLeft)
    {
        setPower(speed, default_Speed);
    }
    else if (currentState == sDangerRight)
    {
        setPower(default_Speed, speed);
    }
}

uint8_t newRound(float myfloat)
{
  double integral;
  float fraction = (float)modf(myfloat, &integral);
 
  if (fraction >= 0.5)
    integral += 1;
  if (fraction <= -0.5)
    integral -= 1;
 
  return (uint8_t)integral;
}
