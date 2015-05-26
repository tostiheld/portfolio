#include "hardware.h"

const uint8_t max_Distance = 30;
uint8_t peakCounter = 0;
uint8_t previousPeak = 0;

void initHardware(void)
{
    initLCD();
    showScreenLCD("################", "################");
    
    setLEDs(0b1111);
    mSleep(200);
    setLEDs(0b0000);
    mSleep(1000);
    
    dischargePeakDetector();
    initI2C_RP6Lib();
    startStopwatch1();
	
    clearLCD();
}

uint8_t getDistance(uint8_t sensor) {
    if (sensor == 2)
    {
        leftDistance = 1/(0.4634*readADC(ADC2)-11.71)*1000;
        return leftDistance;
    } else if (sensor == 3)
    {
        rightDistance = 1/(0.4634*readADC(ADC3)-11.71)*1000;
        return rightDistance;
    }
    return -1;
}

uint8_t detectPeak(void){
	uint8_t detected = 0;
    uint16_t tmp = getMicrophonePeak();
    if (tmp > 10 && getStopwatch1() > 100)
    {
		if (tmp > previousPeak)
		{
			previousPeak = tmp;
			peakCounter = 0;
		}
		if (previousPeak > 100)
		{				
			writeString_P("Peak\n\n");
			detected = 1;
		}
		setStopwatch1(0);
    }
    else if (peakCounter > 20)
    {
		peakCounter = 0;
		previousPeak = tmp;
	}
	else {
		peakCounter++;
	}
    previousPeak = tmp;
    return detected;
}

Events detect_Event(void)
{
	getDistance(2);
	getDistance(3);
    if (detectPeak())
    {
        return eClap;
    }
    
    //else if ((getDistance(3) < 8) && (getDistance(2) > getDistance(3)))
    //{
    else if (leftDistance < 8 && leftDistance < rightDistance)
    {
		writeString_P("Left object\n\n");
        return eObjectLeft;
    }
    //else if ((getDistance(2) < 8) && (getDistance(3) > getDistance(2)))
    //{
    else if (rightDistance < 8 && rightDistance < leftDistance)
    {
		writeString_P("Right object\n\n");
        return eObjectRight;
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

