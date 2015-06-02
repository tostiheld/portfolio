#include "RP6ControlLib.h"
#include "RP6I2CmasterTWI.h" 
#include "RP6Control_I2CMasterLib.h"

uint8_t previousPeak;
uint8_t peakThreshold = 50;
uint8_t isDriving;

void I2C_transmissionError(uint8_t errorState)
{
	writeString_P("\nI2C ERROR - TWI STATE: 0x");
	writeInteger(errorState, HEX);
	writeChar('\n');
}

/*
 * Detects a if a level is above a certain
 * threshold. Saves the current highest level in
 * previouspeak. Only detects a peak if currenttime
 * is greater than pause value.
 * Returns 1 if a peak is detected, 0 otherwise.
 */
uint8_t detectPeak(void)
{
    uint8_t level = getMicrophonePeak();
    
    if (level > peakThreshold &&
        level > previousPeak)
    {
        previousPeak = level;
    }
    else if (previousPeak >= peakThreshold &&
             level < peakThreshold)
    {
        previousPeak = 0;
        return true;
    }

    return false;
}

int detectClosestObject(void)
{
    int readout[] = { 0, 0, 0, 0 };
    readout[0] = readADC(ADC_5); // top left
    readout[1] = readADC(ADC_2); // bottom left
    readout[2] = readADC(ADC_4); // top right
    readout[3] = readADC(ADC_3); // bottom right

    int dists[] = { 0, 0, 0, 0 };

    for (int i = 0; i < 4; i++)
    {
        dists[i] = 1/(0.4634*readout[i]-11.71)*1000;
        if (dists[i] < 3 ||
            dists[i] > 40)
        {
            dists[i] = 40;
        }
    }
    
    int smallest = 10000;
    int which = 0;

    for (int i = 0; i < 4; i++)
    {
        if (dists[i] < smallest)
        {
            smallest = dists[i];
            which = i;
        }
    }
    
    if (smallest > 20)
    {
        return -1;
    }
    
    return which;
}

int main(void)
{
	initRP6Control();
    initI2C_RP6Lib();
    I2C_setTransmissionErrorHandler(I2C_transmissionError);
    
    // Enable Watchdog for Interrupt requests:
	I2CTWI_transmit3Bytes(I2C_RP6_BASE_ADR, 0, CMD_SET_WDT, true);
	// Enable timed watchdog requests:
	I2CTWI_transmit3Bytes(I2C_RP6_BASE_ADR, 0, CMD_SET_WDT_RQ, true);
    
    dischargePeakDetector();
    startStopwatch1();
    startStopwatch2();

    showScreenLCD("", "");
    
	while(true)
	{
        if (detectPeak())
        {
            writeString_P("\n\nPeak\n");
            isDriving = !isDriving;

            setCursorPosLCD(1, 0);
            if (isDriving)
            {
                writeStringLCD("drive   ");
            }
            else
            {
                writeStringLCD("stop   ");
            }
            
            uint8_t speed = isDriving * 80;
            writeInteger(speed, DEC);
            moveAtSpeed(speed, speed);
        }

        int object = detectClosestObject();

        if (getStopwatch2() > 100)
        {
            setStopwatch2(0);
            setCursorPosLCD(0, 0);
            
            switch (object)
            {
                case 0:
                    writeStringLCD("TopLeft    ");
                    moveAtSpeed(isDriving*100, 0);
                break;
                case 1:
                    writeStringLCD("BottomLeft ");
                break;
                case 2:
                    writeStringLCD("TopRight   ");
                    moveAtSpeed(0, isDriving*100);
                break;
                case 3:
                    writeStringLCD("BottomRight");
                break;
                default:
                    writeStringLCD("nuthin     ");
					moveAtSpeed(isDriving*80, isDriving*80);
                break;
            }
        }
        //mSleep(500);
        
        task_checkINT0();
        task_I2CTWI();
	}
	return 0;
}
