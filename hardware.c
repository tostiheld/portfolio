#include "hardware.h"

const uint8_t max_Distance = 30;


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
    uint16_t tmp = getMicrophonePeak();
    if (tmp > 50)
    {
        writeString_P("Peak\n\n");
        return true;
    }
    return false;
}

Events detect_Event(void)
{
    if (detectPeak())
    {
        return eClap;
    }
    else if ((getDistance(3) < 8) && (getDistance(2) > getDistance(3)))
    {
        return eObjectLeft;
    }
    else if ((getDistance(2) < 8) && (getDistance(3) > getDistance(2)))
    {
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

