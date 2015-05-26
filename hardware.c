#include "hardware.h"
#include "RP6ControlLib.h"

uint8_t getDistance(uint8_t sensor) {
	/*
		ADC5 	(1 << PINA5)
		ADC4 	(1 << PINA4)
		ADC3 	(1 << PINA3)
		ADC2 	(1 << PINA2)
	*/
	
	if (sensor == 2) {
		readADC(ADC2) = leftDistance;	
	}
	if (sensor == 3) {
		readADC(ADC3) = rightDistance;
	}
}

uint8_t detectPeak(void){
	uint16_t tmp = getMicrophonePeak();
	if (tmp > 50) 
		return true;
	}
	return false;
}

Events detect_Event(void) {
	if ((GetDistance(3) < 50) && (GetDistance(2) > GetDistance(3))) {
		return eObjectLeft;
	}
	if ((GetDistance(2) < 50) && (GetDistance(3) > GetDistance(2))) {
		return eObjectRight;
	}
	if (DetectPeak) {
		return eClap;
	}
	return eNone;
}

uint8_t isDriving(void){
	if (getLeftSpeed() > 0 || getRightSpeed() > 0) {
		return true;
	}
	return false;
}


void setPower(uint8_t left, uint8_t right){
	leftSpeed = left;
	rightSpeed = right;
	moveAtSpeed(leftSpeed, rightSpeed);
}

void stopMotors(void){
	stop();
}

