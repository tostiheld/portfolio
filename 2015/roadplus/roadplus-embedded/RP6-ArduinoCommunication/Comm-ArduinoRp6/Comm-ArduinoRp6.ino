
#include <Wire.h>
#include <SoftwareSerial.h>
// software serial #1: TX = digital pin 10, RX = digital pin 11
SoftwareSerial portOne(10, 11);

char incomingByte = 0;
String bericht;

byte byteToSend;
void setup()
{
  Wire.begin(42);                // join i2c bus with address #8
  Wire.onRequest(requestEvent); 
   Wire.onReceive(receiveEvent);// register event
  Serial.begin(38400);
  // Start each software serial port
  portOne.begin(38400);
}

void loop()
{
  portOne.listen();
  while (portOne.available() > 0) {
    byteToSend = portOne.read();
    Serial.println(byteToSend);
  }

  delay(500);

}

// function that executes whenever data is requested by master
// this function is registered as an event, see setup()
void requestEvent()
{
   if (byteToSend <= 100 || byteToSend > 0)
  {
    Wire.write(byteToSend); // respond with message of 6 bytes
  }
  // as expected by master
}

void receiveEvent(int howMany)
{
  while (1 < Wire.available()) // loop through all but the last
  {
    char c = Wire.read(); // receive byte as a character
    Serial.println(c);         // print the character
  }
  int x = Wire.read();    // receive byte as an integer
  Serial.println(x);
}
