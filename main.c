#include "states.h"
#include "hardware.h"

int main(void)
{
	initRP6Control();
	initHardware();
    
	while(true)
	{
        task_I2CTWI();
        task_RP6System();
        
		Events currentEvent = detect_Event();
        decide_State(currentState, currentEvent);
        doBehaviours();
	}
	return 0;
}
