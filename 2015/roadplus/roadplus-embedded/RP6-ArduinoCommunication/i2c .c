#include "RP6RobotBaseLib.h" 	
#include "RP6I2CmasterTWI.h" 
#include "i2c.h"
#include "driving.h"

void sendByteAndReceiveByte(void)
{
	if(getStopwatch1() > 500) 
	{
		I2CTWI_transmitByte(ARDUINO_WRITE_ADDRESS, speed);	//writing the speed of rp6	
			
		uint8_t someByteToRead = 0;
		someByteToRead = I2CTWI_readByte(ARDUINO_READ_ADDRESS); //read the maximum speed
		writeInteger(someByteToRead, DEC);
		getSpeedLimit(someByteToRead);       
		setStopwatch1(0);	
	}
}

void I2C_transmissionError(uint8_t errorState)
{
	writeString_P("\nI2C ERROR - TWI STATE: 0x");
	writeInteger(errorState, HEX);
	writeChar('\n');
}
