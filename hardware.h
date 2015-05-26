#ifndef hardware
#define hardware

uint8_t GetDistance(uint8_t sensor);

uint8_t DetectPeak();

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

#endif
