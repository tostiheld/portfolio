//set constants:
//const int timeOutPulseRead = 15000;
const int MaxDistance = 40;
const int MinDistance = 5;
const int offsetServo1 = 20;

//set variables:
float lastobject1;
float lastobject2;
int timesRead1;
int timesRead2;
int countObjects;
int Positie = 55;

boolean Richting;

void TestConnection()
{
  float distance = 0;
  int error = 0;
  while(distance == 0 && error < 10)
  {
    delayMicroseconds(100);
    digitalWrite(trigPin1, HIGH);
    delayMicroseconds(100);
    digitalWrite(trigPin1, LOW);
    distance = (pulseIn(echoPin1, HIGH, 100000) / 2) / 29.1;    
    error++;
  }
  if (error > 8)
  {
     Serial.print(">Ultrasonic:ErrorNotConnected:;");
     sonarOn = false;
  }
  else
  {
    sonarOn = true;
  }
}
void GetDensity()
{
  //turn servo motors:
  if (Richting) 
  {
    Positie += 3;
  }
  else 
  {
    Positie -= 3;
  }
  
  //if the servo is at the start, invert direction and adjust data for easier reading:
  if (Positie <= 0) 
  {
    Richting = true;
    
    density = constrain(countObjects,0,10);    
    countObjects = 0;
  }
  else if (Positie >= 90) 
  {
    Richting = false;
  }
  
  //start stopwatch:
  unsigned long stopwatch = millis();
  
  //turn the servo`s to the appropiate direction:
  Servo1.write(Positie + offsetServo1);


  //Measure Distance Sensor
  digitalWrite(trigPin1, HIGH);
  delayMicroseconds(100);
  digitalWrite(trigPin1, LOW);
  float distance1 = (pulseIn(echoPin1, HIGH, timeOutPulseRead) / 2) / 29.1;

  //wait if the message is recieved fast
  while ((stopwatch + 20) > millis());
  
  //check sensor data for spotted objects:
  if (distance1 > MinDistance && distance1 < MaxDistance)
  {
    if (lastobject1 + 0.25 > distance1 &&  lastobject1 - 0.25 < distance1)
    {
      timesRead1++;
      if (timesRead1 == 3)
      {
        countObjects++;
      }     
    }
    else
    {
      lastobject1 = distance1;
      timesRead1 = 0;
    }
  }
}






