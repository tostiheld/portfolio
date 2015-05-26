#include "RP6ControlLib.h"
#include "states.h"

int main(void)
{
	initRP6Control();
	
	initLCD(); // Initialize the LC-Display (LCD)
			   // Always call this before using the LCD!
	showScreenLCD("################", "################");
	
	setLEDs(0b1111);
	mSleep(50);
	setLEDs(0b0000);
	dischargePeakDetector();
	mSleep(1000);
	
	uint8_t ledpos = 0b0001;
	setLEDs(ledpos);
			
	uint16_t last_peak = 0;
	uint8_t peak_counter = 0;
	
    clearLCD();
    
	while(true)
	{
		uint16_t tmp = getMicrophonePeak();
		setCursorPosLCD(0, 0);
		writeIntegerLengthLCD(tmp, DEC, 4);
		
		if (tmp > 20)
		{
            ledpos <<= 0b0001;
            
            setCursorPosLCD(1, 0);
            writeIntegerLengthLCD(ledpos, BIN, 4);
            
            if (ledpos == 0b10000)
            {
                ledpos = 0b0001;
            }
		}
		setLEDs(ledpos);
        
        mSleep(40);
	}
	return 0;
}
