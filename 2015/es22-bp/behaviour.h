#ifndef behaviour_h
#define behaviour_h

#include <stdint.h>
#include "RP6ControlLib.h"
#include "RP6Control_I2CMasterLib.h"
#include "environment.h"

/*
 * Default movement speed
 */
static const uint8_t BASE_SPEED = 80;

/*
 * Threshold for microphone peak detection
 */
static const uint8_t PEAK_THRESHOLD = 100;

/*
 * Maximum range to detect objects in
 */
static const uint8_t SENSOR_MAX_RANGE = 30;

typedef enum
{
    eNothing,
    eClap,
    eObject
} Event;

typedef struct
{
    uint8_t isDriving;
    uint8_t sensorInRange;
    uint8_t sensorValue;
    uint8_t previousPeak;
} State;

/*
 * Avoids an object
 */
void avoidBehaviour(State* state);

/*
 * Drive or stop
 */
void moveBehaviour(State* state);

/*
 * Detects events in the environment
 */
Event detectEvents(State* state);


#endif
