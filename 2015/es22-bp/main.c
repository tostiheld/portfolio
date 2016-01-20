#include "RP6I2CmasterTWI.h"
#include "behaviour.h"

State currentState;

int main(void)
{
	initRP6Control();
    initI2C_RP6Lib();
    
    // Enable Watchdog for Interrupt requests:
	I2CTWI_transmit3Bytes(I2C_RP6_BASE_ADR, 0, CMD_SET_WDT, true);
	// Enable timed watchdog requests:
	I2CTWI_transmit3Bytes(I2C_RP6_BASE_ADR, 0, CMD_SET_WDT_RQ, true);
    
    dischargePeakDetector();

    showScreenLCD("stopped", "");
    
	while(true)
	{
        Event event = detectEvents(&currentState);

        switch (event)
        {
            case eClap:
                moveBehaviour(&currentState);
            break;
            case eObject:
                avoidBehaviour(&currentState);
            break;
            default:
                // do nothing
            break;
        }
        
        task_checkINT0();
        task_I2CTWI();
	}
	return 0;
}
