#include "hardware.h"
#include "RP6ControlLib.h"
#include "RP6Control_I2CMasterLib.h"

uint8_t getDistance(uint8_t sensor) {
	if (sensor == 2) {
		readADC(ADC2) = leftDistanceRaw;	
		leftDistance = 1/(0.4634*leftDistanceRaw-11.71)*1000;
	}
	if (sensor == 3) {
		readADC(ADC3) = rightDistanceRaw;
		rightDistance = 1/(0.4634*rightDistanceRaw-11.71)*1000;
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
	if ((GetDistance(3) < 8) && (GetDistance(2) > GetDistance(3))) {
		return eObjectLeft;
	}
	if ((GetDistance(2) < 8) && (GetDistance(3) > GetDistance(2))) {
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

