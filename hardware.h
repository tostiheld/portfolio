#ifndef hardware_h
#define hardware_h

#include "RP6ControlLib.h"
#include "RP6I2CmasterTWI.h" 
#include "RP6Control_I2CMasterLib.h"

extern const uint8_t max_Distance;

uint8_t leftDistance;
uint8_t rightDistance;
uint8_t leftSpeed;
uint8_t rightSpeed;
uint8_t previousPeak;

void initHardware(void);

uint8_t getDistance(uint8_t sensor);

uint8_t detectPeak(void);

/*
 * Events
 * ======
 * 
 * Events that lead to state change
 * 
 * eNone: nothing to do
 * eClap: clap detected through mic
 * eObjectLeft: object left detected
 * eObjectLeft: object right detected
 */

typedef enum
{
    eNone,
    eClap,
    eObjectLeft,
    eObjectRight
} Events;

/*
 * Detect an event using the hardware functions
 */
Events detect_Event(void);
uint8_t isDriving(void);
void setPower(uint8_t left, uint8_t right);
void stopMotors(void);


#endif
