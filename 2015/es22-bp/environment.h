#ifndef environment_h
#define environment_h

#include <stdint.h>

/*
 * Detects a if a level is above a certain
 * threshold. Saves the current highest level in
 * previouspeak.
 * Returns 1 if a peak is detected, 0 otherwise.
 */
uint8_t detectPeak(uint8_t level, uint8_t threshold, uint8_t* previouspeak);

/*
 * Reads from all 4 distance sensors
 */
void readSensors(uint16_t output[]);

/*
 * Calculates sensor values to distances in cm
 * Expects arrays with length 'length'
 */
void calculateDistances(uint16_t values[], uint16_t distances[], uint8_t length);

/*
 * Determines which distance is in range of 'range'
 * Expects an array with length 'length'
 * 'actualvalue' is the actual value of the value in range
 * Returns: which distance is in range
 *          OR -1 if all out of range
 *          OR -100 if nullptr
 */
uint8_t whichIsInRange(uint16_t distances[], uint8_t length, uint8_t range, uint8_t* actualvalue);

#endif
