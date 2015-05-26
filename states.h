/*
 * State behaviour for BP1 program
 */

#ifndef "states_h"
#define "states_h"

/*
 * States
 * ======
 * 
 * Definition of different program states
 * 
 * sStop: Doing nothing, waiting for clap
 * sDrive: Driving, listening for clap, detecting objects
 * sDangerLeft: steering right until no object is detected
 * sDangerRight: steering left until no object is detected
 */
typedef enum
{
    sStop,
    sDrive,
    sDangerLeft,
    sDangerRight
} States;

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
/*
 * Decide which state to use
 */
States detect_State(Events event);

/*
 * Behaviours for different states
 */
void state_Stop(void);
void state_Drive(void);
void state_DangerLeft(void);
void state_DangerRight(void);

#endif
