#include "RP6ControlLib.h"
#include "RP6I2CmasterTWI.h" 
#include "states.h"
//#include "RP6Control_I2CMasterLib.h"
#include "hardware.h"

const uint8_t default_Speed = 60;

void I2C_transmissionError(uint8_t errorState)
{
	writeString_P("\nI2C ERROR - TWI STATE: 0x");
	writeInteger(errorState, HEX);
	writeChar('\n');
}

int main(void)
{
	initRP6Control();
	initHardware();
    
    I2C_setTransmissionErrorHandler(I2C_transmissionError);
    
    // Enable Watchdog for Interrupt requests:
	I2CTWI_transmit3Bytes(I2C_RP6_BASE_ADR, 0, CMD_SET_WDT, true);
	// Enable timed watchdog requests:
	I2CTWI_transmit3Bytes(I2C_RP6_BASE_ADR, 0, CMD_SET_WDT_RQ, true);
    
    currentState = sStop;
    
	while(true)
	{
        task_checkINT0();
        task_I2CTWI();
        
		Events currentEvent = detect_Event();
        decide_State(currentState, currentEvent);
        doBehaviours();
	}
	return 0;
}
