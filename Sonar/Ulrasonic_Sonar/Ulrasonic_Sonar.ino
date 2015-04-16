#include <Servo.h> 

//Define instances of Servo:
Servo Servo1;
Servo Servo2;

//Define Pins:
const int trigPin1 = 12;
const int echoPin1 = 13;
const int trigPin2 = 6;
const int echoPin2 = 7;
const int servoPin1 = 10;
const int servoPin2 = 5;

//set constants:
const int BaudRate = 19200;
const int timeOutPulseRead = 15000;
const int MaxDistance = 200.0;
const int MinDistance = 2.0;
const int offsetServo1 = 20;
const int offsetServo2 = 20;

//Define Variables:
int Positie = 55;
boolean Richting;

void setup() 
{
  //start serial connection:
  Serial.begin(BaudRate);
  
  //define pins for servo motors:
  Servo1.attach(servoPin1);
  Servo2.attach(servoPin2);
  
  //set pinmode for all pins:
  pinMode(trigPin1, OUTPUT);
  pinMode(echoPin1, INPUT);
  pinMode(trigPin2, OUTPUT);
  pinMode(echoPin2, INPUT);

  //give conformational Message:
  Serial.println("Startup Successfull");  
}

void loop() 
{
  //motor Rotation:
  if (Richting){
    Positie += 3;
  }
  else {
    Positie -= 3;
  }
  if (Positie <= 0){
    Richting = true;
  }
  else if (Positie >= 90){
    Richting = false;
  }
  Servo1.write(Positie + offsetServo1);
  Servo2.write(Positie + offsetServo2);


  //Measure Distance Sensor 1
  digitalWrite(trigPin1, HIGH);
  delayMicroseconds(100);
  digitalWrite(trigPin1, LOW);
  float distance1 = (pulseIn(echoPin1, HIGH, timeOutPulseRead)/2) / 29.1;
  //Measure Distance Sensor 2
  digitalWrite(trigPin2, HIGH);
  delayMicroseconds(100);
  digitalWrite(trigPin2, LOW);  
  float distance2 = (pulseIn(echoPin2, HIGH, timeOutPulseRead)/2) / 29.1;



  //Servo 1 print:
  Serial.print(">Dist1:");
  Serial.print(Positie);
  Serial.print(":");
  if (distance1 < MinDistance || distance1 > MaxDistance)
  {
    Serial.print("Out of Range");      
  }
  else
  {
    Serial.print(distance1);
  }
  Serial.println(":;");  

  //Servo 2 print:
  Serial.print(">Dist2:");
  Serial.print(Positie);
  Serial.print(":");
  if (distance2 < MinDistance || distance2 > MaxDistance)
  {
    Serial.print("Out of Range");      
  }
  else
  {
    Serial.print(distance2);
  }
  Serial.println(":;");  
}





