/*****************************************************************************/
// Includes:
#include "RP6RobotBaseLib.h" 	  //Base Library
#include "RP6I2CmasterTWI.h"     //set I2CMaster 
#include "i2c.h"
#include "driving.h"				 
#define ARDUINO_DEVICE_ADDRESS (42)
#define ARDUINO_WRITE_ADDRESS (ARDUINO_DEVICE_ADDRESS << 1)
#define ARDUINO_READ_ADDRESS (ARDUINO_WRITE_ADDRESS + 1)
#define RC_YOUR_OWN //Tv-Remote

#ifdef RC_YOUR_OWN		//buttons for Tv-Remote
	#define RC5_KEY_LEFT 				4		
	#define RC5_KEY_RIGHT 				6		
	#define RC5_KEY_FORWARDS 			2  	
	#define RC5_KEY_BACKWARDS 			8		
	#define RC5_KEY_STOP 				5		
	#define RC5_KEY_SPEED_UP		    32	
	#define RC5_KEY_SPEED_DOWN  		33	
#endif

//global variables
int speed = 0;
int maxSpeed = 50;
 
int main(void)
{
	initRobotBase();
	I2CTWI_initMaster(100);
	I2CTWI_setTransmissionErrorHandler(I2C_transmissionError);
	
	setLEDs(0b111111);
	mSleep(500);	   
	setLEDs(0b000000);
	powerON();
	changeDirection(FWD); //set driving direction default to FWD
	IRCOMM_setRC5DataReadyHandler(receiveRC5Data);
    writeString_P("\nT22 Proftaak!!\n");
	startStopwatch1();
	
	while(true)  
	{
		if (speed > (maxSpeed+5))
		{
			speed = maxSpeed;
		}
		moveAtSpeed(speed*2, speed*2);
		sendByteAndReceiveByte();
		task_I2CTWI();
		task_RP6System();
	}
	return 0;
}
/*****************************************************************************/
