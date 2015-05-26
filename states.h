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
 * sError: Something went wrong
 * sStop: Doing nothing, waiting for clap
 * sDrive: Driving, listening for clap, detecting objects
 * sDangerLeft: steering right until no object is detected
 * sDangerRight: steering left until no object is detected
 */
typedef enum
{
    sError,
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
States decide_State(Events event);

/*
 * Behaviours for different states
 */
void behaviour_Stop(void);
void behaviour_Drive(void);
void behaviour_DangerLeft(void);
void behaviour_DangerRight(void);

#endif
