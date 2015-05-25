void GetDistance()
{
  for (int i = 0; i < 1501; i++)
  {

//    if (Richting) {
//      Positie += 3;
//    }
//    else {
//      Positie -= 3;
//    }
//    if (Positie <= 0) {
//      Richting = true;
//    }
//    else if (Positie >= 90) {
//      Richting = false;
//    }
//    Servo1.write(Positie + offsetServo1);
//    Servo2.write(Positie + offsetServo2);


    //Measure Distance Sensor 1
    digitalWrite(trigPin1, HIGH);
    delayMicroseconds(100);
    digitalWrite(trigPin1, LOW);
    float distance1 = (pulseIn(echoPin1, HIGH, timeOutPulseRead) / 2) / 29.1;
    //Measure Distance Sensor 2
    //digitalWrite(trigPin2, HIGH);
    //delayMicroseconds(100);
    //digitalWrite(trigPin2, LOW);
    //float distance2 = (pulseIn(echoPin2, HIGH, timeOutPulseRead) / 2) / 29.1;
    sum += distance1;
  }
    average = sum / 1500;
    map(average, 1, 50, 1, 10);
    String distMessage = ">DIST:" + (String)average + ":;";
    Serial.println(distMessage);
    average = 0;
    sum = 0;
    return;
}
