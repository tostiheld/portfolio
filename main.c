#include "RP6ControlLib.h"
#include "RP6I2CmasterTWI.h" 
#include "RP6Control_I2CMasterLib.h"

uint8_t peakThreshold = 50;
uint8_t sensorMaxRange = 20;
uint8_t baseSpeed = 80;

uint8_t previousPeak;
uint8_t isDriving;
uint8_t sensorInRange;
uint8_t sensorValue;

typedef enum
{
    eNothing,
    eClap,
    eObject
} Event;

/*
 * Detects a if a level is above a certain
 * threshold. Saves the current highest level in
 * previouspeak.
 * Returns 1 if a peak is detected, 0 otherwise.
 */
uint8_t detectPeak(uint8_t level,
                   uint8_t threshold,
                   uint8_t *previouspeak)
{
    if (level > threshold &&
        level > *previouspeak)
    {
        *previouspeak = level;
    }
    else if (*previouspeak >= threshold &&
             level < threshold)
    {
        *previouspeak = 0;
        return true;
    }

    return false;
}

/*
 * Reads from all 4 distance sensors
 */
void readSensors(uint16_t* output)
{
    if (output == NULL)
    {
        return;
    }
    
    output[0] = readADC(ADC_5); // top left
    output[1] = readADC(ADC_2); // bottom left
    output[2] = readADC(ADC_4); // top right
    output[3] = readADC(ADC_3); // bottom right
}

/*
 * Calculates sensor values to distances in cm
 */
void calculateDistances(uint16_t* values,
                        uint16_t* distances)
{
    if (values == NULL ||
        distances == NULL)
    {
        return;
    }
    
    for (int i = 0; i < 4; i++)
    {
        distances[i] = 1 / (0.4634 * values[i] - 11.71) * 1000;
        
        // if we calculate an invalid value, set to upper
        // bound
        if (distances[i] < 3 ||
            distances[i] > 40)
        {
            distances[i] = 40;
        }
    }
}

/*
 * Returns which array member is in range
 * actualvalue is the actual value of the array member
 * Or -1 if all are out of range
 * Or -100 if distances is NULL
 */
uint8_t whichIsInRange(uint16_t* distances,
                       uint8_t range,
                       uint8_t* actualvalue)
{
    if (distances == NULL)
    {
        return -100;
    }
    
    int smallest = 10000;
    uint8_t which = 0;

    for (uint8_t i = 0; i < 4; i++)
    {
        if (distances[i] < smallest)
        {
            smallest = distances[i];
            which = i;
        }
    }

    *actualvalue = smallest;
    
    if (smallest > range)
    {
        return -1;
    }
    
    return which;
}

void avoidBehaviour(void)
{
    if (isDriving)
    {
        float acceleration = sensorValue / sensorMaxRange;
        uint8_t adjustedspeed = sensorValue * baseSpeed;
        
        switch (sensorInRange)
        {
            case 0: // top left
                setCursorPosLCD(1, 0);
                writeStringLCD("top left       ");
            case 1: // bottom left
                setCursorPosLCD(1, 0);
                writeStringLCD("bottom left    ");
                moveAtSpeed(baseSpeed + adjustedspeed,
                            baseSpeed - adjustedspeed);
            break;
            case 2: // top right
                setCursorPosLCD(1, 0);
                writeStringLCD("top right      ");
            case 3: // bottom right
                setCursorPosLCD(1, 0);
                writeStringLCD("bottom right   ");
                moveAtSpeed(baseSpeed - adjustedspeed,
                            baseSpeed + adjustedspeed);
            break;
            default:
                // do nothing
            break;
        }
    }
}

void moveBehaviour(void)
{
    isDriving = !isDriving;
    if (isDriving)
    {
        setCursorPosLCD(0, 0);
        writeStringLCD("driving");
    }
    else
    {
        setCursorPosLCD(0, 0);
        writeStringLCD("stopped");
    }
    uint8_t speed = isDriving * baseSpeed;
    moveAtSpeed(speed, speed);
}

Event detectEvents(void)
{
    if (detectPeak(getMicrophonePeak(),
                   peakThreshold,
                   &previousPeak))
    {
        return eClap;
    }
    else
    {
        uint16_t raw[] = { 0, 0, 0, 0 };
        readSensors(&raw);
        
        uint16_t dists[] = { 0, 0, 0, 0 };
        calculateDistances(&raw, &dists);        
        sensorInRange = whichIsInRange(dists,
                                       sensorMaxRange,
                                       &sensorValue);
        
        if (sensorInRange >= 0)
        {
            return eObject;
        }
    }

    return eNothing;
}

int main(void)
{
	initRP6Control();
    initI2C_RP6Lib();
    
    // Enable Watchdog for Interrupt requests:
	I2CTWI_transmit3Bytes(I2C_RP6_BASE_ADR, 0, CMD_SET_WDT, true);
	// Enable timed watchdog requests:
	I2CTWI_transmit3Bytes(I2C_RP6_BASE_ADR, 0, CMD_SET_WDT_RQ, true);
    
    dischargePeakDetector();
    startStopwatch1();
    startStopwatch2();

    showScreenLCD("", "");
    
	while(true)
	{
        Event event = detectEvents();

        switch (event)
        {
            case eClap:
                moveBehaviour();
            break;
            case eObject:
                avoidBehaviour();
            break;
            default:
                // do nothing
            break;
        }
        
        task_checkINT0();
        task_I2CTWI();
	}
	return 0;
}
