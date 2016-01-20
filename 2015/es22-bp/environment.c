#include "behaviour.h"

/*
 * Detects a if a level is above a certain
 * threshold. Saves the current highest level in
 * previouspeak.
 * Returns 1 if a peak is detected, 0 otherwise.
 */
uint8_t detectPeak(uint8_t level,
                   uint8_t threshold, 
                   uint8_t* previouspeak)
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
        return 1;
    }

    return 0;
}

/*
 * Reads from all 4 distance sensors
 */
void readSensors(uint16_t output[])
{
    if (!output)
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
 * Expects arrays with length 'length'
 */
void calculateDistances(uint16_t values[],
                        uint16_t distances[],
                        uint8_t length)
{
    if (!values ||
        !distances)
    {
        return;
    }
    
    for (int i = 0; i < length; i++)
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
 * Determines which distance is in range of 'range'
 * Expects an array with length 'length'
 * 'actualvalue' is the actual value of the value in range
 * Returns: which distance is in range
 *          OR -1 if all out of range
 *          OR -100 if nullptr
 */
uint8_t whichIsInRange(uint16_t distances[],
                       uint8_t length,
                       uint8_t range,
                       uint8_t* actualvalue)
{
    if (!distances)
    {
        return -100;
    }
    
    int smallest = 10000;
    uint8_t which = 0;

    for (uint8_t i = 0; i < length; i++)
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
