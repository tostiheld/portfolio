#include "hardware.h"

#define 

int GetDistance(int sensor) {
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
