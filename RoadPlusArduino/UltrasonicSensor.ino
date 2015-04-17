void GetDistance()
{

  if (Richting) {
    Positie += 3;
  }
  else {
    Positie -= 3;
  }
  if (Positie <= 0) {
    Richting = true;
  }
  else if (Positie >= 90) {
    Richting = false;
  }
  Servo1.write(Positie + offsetServo1);
//  Servo2.write(Positie + offsetServo2);


  //Measure Distance Sensor 1
  digitalWrite(trigPin1, HIGH);
  delayMicroseconds(100);
  digitalWrite(trigPin1, LOW);
  float distance1 = (pulseIn(echoPin1, HIGH, timeOutPulseRead) / 2) / 29.1;
  //Measure Distance Sensor 2
//  digitalWrite(trigPin2, HIGH);
//  delayMicroseconds(100);
//  digitalWrite(trigPin2, LOW);
//  float distance2 = (pulseIn(echoPin2, HIGH, timeOutPulseRead) / 2) / 29.1;





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

//  //Servo 2 print:
//  Serial.print(">Dist2:");
//  Serial.print(Positie);
//  Serial.print(":");
//  if (distance2 < MinDistance || distance2 > MaxDistance)
//  {
//    Serial.print("Out of Range");
//  }
//  else
//  {
//    Serial.print(distance2);
//  }
//  Serial.println(":;");
}
