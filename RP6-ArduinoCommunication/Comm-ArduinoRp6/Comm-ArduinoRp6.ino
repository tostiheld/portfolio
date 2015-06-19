#include <Wire.h>

void setup()
{
  Wire.begin(); // join i2c bus (address optional for master)
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
    if (x > 0)
    {
      Wire.write(x);
      Wire.endTransmission();
    }

    // stop transmitting
    delay(500);
  }

}
