/*
 * State behaviour for BP1 program
 */

#ifndef "states_h"
#define "states_h"

const uint8_t default_Speed = 60;

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
 * Decide which state to use
 */
void decide_State(Events event);

States currentState;

/*
 * Behaviours for different states
 */
void behaviour_Error(void);
void behaviour_Stop(void);
void behaviour_Drive(void);
void behaviour_Danger(uint8_t distance);

#endif
