#include "hardware.h"
#include "RP6ControlLib.h"

uint8_t GetDistance(uint8_t sensor) {
	/*
		ADC5 	(1 << PINA5)
		ADC4 	(1 << PINA4)
		ADC3 	(1 << PINA3)
		ADC2 	(1 << PINA2)
	*/
		
	if (sensor == 2) {
		return readADC(ADC2);	
	}
	if (sensor == 3) {
		return readADC(ADC3);
	}
}

uint8_t DetectPeak(){
	uint16_t tmp = getMicrophonePeak();
	if (tmp > 50) 
		return true;
	}
	return false;
}
