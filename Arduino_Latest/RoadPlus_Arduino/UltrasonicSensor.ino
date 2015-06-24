//set constants:
const int timeOutPulseRead = 5000;
const int MaxDistance = 100.0;
const int MinDistance = 15.0;
const int offsetServo1 = 20;

//set variables:
float lastobject1;
float lastobject2;
int timesRead1;
int timesRead2;
int countObjects;
int Positie = 55;
boolean Richting;

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


  //Measure Distance Sensor 1
  digitalWrite(trigPin1, HIGH);
  delayMicroseconds(100);
  digitalWrite(trigPin1, LOW);
  float distance1 = (pulseIn(echoPin1, HIGH, timeOutPulseRead) / 2) / 29.1;

  //Measure Distance Sensor 2
  digitalWrite(trigPin2, HIGH);
  delayMicroseconds(100);
  digitalWrite(trigPin2, LOW);
  float distance2 = (pulseIn(echoPin2, HIGH, timeOutPulseRead) / 2) / 29.1;
  
  //wait if the message is recieved fast
  while ((stopwatch + 20) > millis());
  
  //check sensor data for spotted objects:
  if (distance1 > MinDistance && distance1 < MaxDistance)
  {
    if (lastobject1 + 0.25 > distance1 &&  lastobject1 - 0.25 < distance1)
    {
      timesRead1++;
      if (timesRead1 == 5)
      {
        countObjects++;
        timesRead1 = 0;
      }     
    }
    else
    {
      lastobject1 = distance1;
      timesRead1 = 0;
    }
  }
  if (distance2 > MinDistance && distance2 < MaxDistance)
  {
    if (lastobject2 + 0.25 > distance2 &&  lastobject2 - 0.25 < distance2)
    {
      timesRead2++;
      if (timesRead2 == 5)
      {
        countObjects++;
        timesRead2 = 0;
      }     
    }
    else
    {
      lastobject2 = distance2;
      timesRead2 = 0;
    }
  }
}






