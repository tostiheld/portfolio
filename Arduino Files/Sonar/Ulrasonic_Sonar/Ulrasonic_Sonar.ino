#include <Servo.h> 
Servo Servo1;
const int trigPin = 12;
const int echoPin = 13;
int hoek = 55;
boolean Richting;

void setup() 
{
  Servo1.attach(10);
  Serial.begin (9600);
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);  
}

void loop() 
{
  if (Richting){hoek+=3;}
  if (!Richting){hoek-=3;}
  if (hoek <= 0){Richting = true;}
  if (hoek >= 90){Richting = false;}
  Servo1.write(hoek + 20);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(1000);
  digitalWrite(trigPin, LOW);
  int distance = (pulseIn(echoPin, HIGH)/2) / 29.1;
  if (distance >= 200 || distance <= 0)
  {
    Serial.println("Out of range");
  }
  else 
  {
    double x = (distance * sin(hoek/57.2957795));
    double y = (distance * cos(hoek/57.2957795));
    Serial.print(distance);
    Serial.print(" cm");
    Serial.print("    X = ");
    Serial.print(x);
    Serial.print("    Y = ");
    Serial.println(y);
  }
}

