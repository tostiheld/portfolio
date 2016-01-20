#include <Wire.h>

void setup()
{
  Wire.begin(10); // join i2c bus (address optional for master)
  Serial.begin(9600);
}

byte x = 0;
void loop()
{
  if (Serial.available() > 0)
  {
    Wire.beginTransmission(10); // transmit to device #4
    x = Serial.read();
    Serial.write(x);
    Wire.write(x);
    Wire.endTransmission();    // stop transmitting
    delay(500);
  }

}
