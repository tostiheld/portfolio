bool dir;
int sweep;
long total;
int average;
unsigned long lastTime;
const int timeOutPulseRead = 15000;

void getDensity()
{
  if (millis() > (lastTime + 50))
  {
    lastTime = millis();
    if (sweep >= 90 ||
        sweep <= 0)
    {
      dir = !dir;
      average = total / 90;
      total = 0;
      
      if (average > 50)
      {
        average = 50;
      }
      else if (average < 10)
      {
        average = 10;
      }
      
      float percentage = ((average - 10.0) / 40.0) * 100.0;
      density = 100 - percentage;
    }
    
    if (dir)
    {
      sweep += 1;
    }
    else
    {
      sweep -= 1;
    }
    
    pinMode(trigPin1, OUTPUT);
    digitalWrite(trigPin1, LOW);
    delayMicroseconds(2);
    digitalWrite(trigPin1, HIGH);
    delayMicroseconds(5);
    digitalWrite(trigPin1, LOW);
    
    pinMode(echoPin1, INPUT);
    float duration = pulseIn(echoPin1, HIGH, timeOutPulseRead);
    long cm = duration / 29 / 2;
    total += cm;
    
    Servo1.write(sweep + 20);
  }
}

