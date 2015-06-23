/*****************************************************************************/
// Includes:

#include "RP6RobotBaseLib.h" 	
#include "RP6I2CmasterTWI.h"     // Include the I2C-Bus Slave Library

#define ARDUINO_DEVICE_ADDRESS (42)
#define ARDUINO_WRITE_ADDRESS (ARDUINO_DEVICE_ADDRESS << 1)
#define ARDUINO_READ_ADDRESS (ARDUINO_WRITE_ADDRESS + 1)
int speed = 0;
#define RC_YOUR_OWN

#ifdef RC_YOUR_OWN		// Your own RC!
	#define RC5_KEY_LEFT 				4		
	#define RC5_KEY_RIGHT 				6		
	#define RC5_KEY_FORWARDS 			2  	
	#define RC5_KEY_BACKWARDS 			8		
	#define RC5_KEY_STOP 				5		
	#define RC5_KEY_CURVE_LEFT 			1	
	#define RC5_KEY_CURVE_RIGHT 		3	
	#define RC5_KEY_CURVE_BACK_LEFT 	7	
	#define RC5_KEY_CURVE_BACK_RIGHT 	9		
	#define RC5_KEY_LEFT_MOTOR_FWD		10
	#define RC5_KEY_LEFT_MOTOR_BWD 		11
	#define RC5_KEY_RIGHT_MOTOR_FWD 	12
	#define RC5_KEY_RIGHT_MOTOR_BWD 	13
#endif

 int getSpeedLimit(uint8_t s)
 {
	speed = s;
	 return speed;
 }
 
 
void receiveRC5Data(RC5data_t rc5data)
{

	
#ifndef DO_NOT_MOVE    	// used to disable movement if you want to 
						// look at the received RC5 codes only (s. above).
	
 // used to store if we have received 
						  // a movement command.							  // Any other key is ignored!
	// Check which key is pressed:
	switch(rc5data.key_code)
	{
		case RC5_KEY_LEFT:
		rotate(speed*2, LEFT, DIST_MM(3), false);
		break;
		case RC5_KEY_RIGHT:
		rotate(speed*2, RIGHT, DIST_MM(3), false);
		break;
		case RC5_KEY_FORWARDS:
		move(speed*2, FWD, DIST_MM(3000), false);
		break;
		case RC5_KEY_BACKWARDS:
		move(speed*2, BWD, DIST_MM(3000), false);
		break;
		case RC5_KEY_STOP:
		move(0, FWD, DIST_MM(0), false);
		break;
	}
	
#endif

}


 
static uint8_t someByteToSend = 0; 

/**
 * This function gets called automatically if there was an I2C Error like
 * the slave sent a "not acknowledge" (NACK, error codes e.g. 0x20 or 0x30).
 * The most common mistakes are: 
 *   - using the wrong address for the slave
 *   - slave not active or not connected to the I2C-Bus
 *   - too fast requests for a slower slave
 * Be sure to check this if you get I2C errors!
 */
void I2C_transmissionError(uint8_t errorState)
{
	writeString_P("\nI2C ERROR - TWI STATE: 0x");
	writeInteger(errorState, HEX);
	writeChar('\n');
}

void sendByteAndReceiveByte(void)
{
	if(getStopwatch1() > 500) {
	
                writeString_P("\nSend: ");
		
		//TODO: send someByteToSend to the Arduino
		I2CTWI_transmitByte(ARDUINO_WRITE_ADDRESS, someByteToSend);
		someByteToSend += 5;		
	
		if(someByteToSend > 255) 
		{
			someByteToSend = 0;
		}
		
		uint8_t someByteToRead = 0;

		//TODO: read someByteToSend from the Arduino
		someByteToRead = I2CTWI_readByte(ARDUINO_READ_ADDRESS);
		writeString_P("\nReceived: ");
                writeInteger(someByteToRead,DEC);
		writeChar('\n');
		getSpeedLimit(someByteToRead);
		writeInteger(someByteToRead, DEC);        
		setStopwatch1(0);	
	}
}

/*****************************************************************************/
// Main - The program starts here:

int main(void)
{
	initRobotBase();
	
	I2CTWI_initMaster(100); // Initialize the TWI Module for Master operation
				// with 100kHz SCL Frequency
				// PCF8574 and PCF8591 are only specified for
				// up to 100kHz SCL freq - not 400kHz HighSpeed mode!
	
	I2CTWI_setTransmissionErrorHandler(I2C_transmissionError);

	setLEDs(0b111111);
	mSleep(500);	   
	setLEDs(0b000000);
	
	powerON();

        writeString_P("\nReady for I2C master mode:\n");
	startStopwatch1();
	
	while(true)  
	{
		/*if (speed == 0)
		{
			speed = 30;
		}	*/
		moveAtSpeed(speed*2, speed*2);
		sendByteAndReceiveByte();
		task_I2CTWI(); // Call I2C Management routine
		task_RP6System();
	}
	return 0;
}
