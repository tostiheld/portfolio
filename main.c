#include "RP6ControlLib.h"
#include "RP6I2CmasterTWI.h" 
#include "states.h"
//#include "RP6Control_I2CMasterLib.h"
#include "hardware.h"

const uint8_t default_Speed = 60;

int main(void)
{
	initRP6Control();
	initHardware();
    
	while(true)
	{
        task_I2CTWI();
        
		Events currentEvent = detect_Event();
        decide_State(currentState, currentEvent);
        doBehaviours();
	}
	return 0;
}
