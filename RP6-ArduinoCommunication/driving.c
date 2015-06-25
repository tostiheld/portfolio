#include "RP6RobotBaseLib.h" 	
#include "RP6I2CmasterTWI.h" 
#include "i2c.h"
#include "driving.h"

void getSpeedLimit(uint8_t s)
 {
	maxSpeed = s;
 }

void receiveRC5Data(RC5data_t rc5data)
{	
	switch(rc5data.key_code)
	{

		case RC5_KEY_LEFT:
		changeDirection(LEFT);
		break;
		
		case RC5_KEY_RIGHT:
		changeDirection(RIGHT);
		break;
		
		case RC5_KEY_FORWARDS:
		changeDirection(FWD);
		break;
		
		case RC5_KEY_BACKWARDS:
		changeDirection(BWD);
		break;
		
		case RC5_KEY_STOP:
		speed = 0;
		break;
		
		case RC5_KEY_SPEED_UP:
		if (speed < 100 && speed < (maxSpeed+5))
		{
		speed += 2;
		}
		break;
		
		case RC5_KEY_SPEED_DOWN:
		if (speed > 0)
		{
			speed -= 2;
		}
		break;
	}	
}
