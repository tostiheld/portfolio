#include "behaviour.h"

/*
 * Avoids the object given in sensorInRange
 * if isDriving is 1
 */
void avoidBehaviour(State* state)
{
    if (!state)
    {
        return;
    }
    else if (state->isDriving)
    {
        //float acceleration = sensorValue / sensorMaxRange;
        //uint8_t adjustedspeed = baseSpeed * (sensorValue / sensorMaxRange);
        
        switch (state->sensorInRange)
        {
            case 0: // top left
                setCursorPosLCD(1, 0);
                writeStringLCD("top left       ");
                moveAtSpeed(BASE_SPEED + (SENSOR_MAX_RANGE - state->sensorValue),
                            BASE_SPEED - (SENSOR_MAX_RANGE - state->sensorValue));
			break;
            case 2: // top right
                setCursorPosLCD(1, 0);
                writeStringLCD("top right      ");
                moveAtSpeed(BASE_SPEED - (SENSOR_MAX_RANGE - state->sensorValue),
                            BASE_SPEED + (SENSOR_MAX_RANGE - state->sensorValue));
            break;
            default:
                moveAtSpeed(BASE_SPEED, BASE_SPEED);
            break;
        }
    }
}

void moveBehaviour(State* state)
{
    state->isDriving = !state->isDriving;
    if (state->isDriving)
    {
        setCursorPosLCD(0, 0);
        writeStringLCD("driving");
        moveAtSpeed(BASE_SPEED, BASE_SPEED);
    }
    else
    {
        setCursorPosLCD(0, 0);
        writeStringLCD("stopped");
        moveAtSpeed(0, 0);
    }
}

Event detectEvents(State* state)
{
    if (detectPeak(getMicrophonePeak(),
                   PEAK_THRESHOLD,
                   &state->previousPeak))
    {
        return eClap;
    }
    else
    {
        uint16_t raw[] = { 0, 0, 0, 0 };
        readSensors(raw);
        
        uint16_t dists[] = { 0, 0, 0, 0 };
        calculateDistances(raw, dists, 4);        
        state->sensorInRange = whichIsInRange(dists,
                                        4,
                                        SENSOR_MAX_RANGE,
                                        &state->sensorValue);
        
        if (state->sensorInRange >= 0)
        {
            return eObject;
        }
    }

    return eNothing;
}
