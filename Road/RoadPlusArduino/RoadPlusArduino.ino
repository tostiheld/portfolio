  int Distances[100];
  int average;
  int sum;
  float distance1;
//voor serial
char incomingByte = 0;
String message;
String metaMessage;
int speedLimit = 0;
String meta;
long timetillnext;
long previousTime;
boolean state;
boolean warning;
boolean sign;
boolean dist;
boolean temp;
#include <Servo.h>

//Define instances of Servo:
Servo Servo1;
Servo Servo2;

//Define Pins:
const int trigPin1 = A0;
const int echoPin1 = A1;
const int trigPin2 = A4;
const int echoPin2 = A3;
const int servoPin1 = A2;
const int servoPin2 = A5;
long first;
long second;

//set constants:
const int timeOutPulseRead = 15000;
const int MaxDistance = 250.0;
const int MinDistance = 2.0;
const int offsetServo1 = 20;
const int offsetServo2 = 20;

//Define Variables:
int Positie = 55;
boolean Richting;

//voor temperatuur
#include <OneWire.h>
#include <DallasTemperature.h>
#define ONE_WIRE_BUS 11
OneWire ourWire(ONE_WIRE_BUS);
DallasTemperature sensors(&ourWire);

//voor led matrix
#include <Arduino.h>

//IO
#define LEDARRAY_D 2
#define LEDARRAY_C 3
#define LEDARRAY_B 4
#define LEDARRAY_A 5
#define LEDARRAY_G 6
#define LEDARRAY_DI 7
#define LEDARRAY_CLK 8
#define LEDARRAY_LAT 9


unsigned char Display_Buffer[2];
const unsigned char  Init_Display[1][32] =
{
  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
  0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
};

const unsigned char  signWarning[1][32] =
{
  0xfe, 0xfd, 0xfd, 0xfb, 0xfa, 0xf6, 0xf6, 0xee,
  0xee, 0xde, 0xde, 0xbf, 0xbe, 0x7e, 0x7f, 0x00,
  0x7f, 0xbf, 0xbf, 0xdf, 0x5f, 0x6f, 0x6f, 0x77,
  0x77, 0x7b, 0x7b, 0xfd, 0x7d, 0x7e, 0xfe, 0x00,
};

const unsigned char  OneDirection[1][32] =
{
  0xf8, 0xe0, 0xe0, 0x80, 0x80, 0x00, 0x00, 0x0f,
  0x0f, 0x00, 0x00, 0x80, 0x80, 0xc0, 0xe0, 0xf8,
  0x1f, 0x07, 0x03, 0x01, 0x01, 0x00, 0x00, 0xf0,
  0xf0, 0x00, 0x00, 0x01, 0x01, 0x03, 0x07, 0x1f,
};

const unsigned char  sign10[1][32] =
{
  0xff, 0xff, 0xfb, 0xf3, 0xeb, 0xfb, 0xfb, 0xfb,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0xff, 0xff,
  0xff, 0xff, 0x07, 0x77, 0x77, 0x77, 0x77, 0x77,
  0x77, 0x77, 0x77, 0x77, 0x77, 0x07, 0xff, 0xff,
};

const unsigned char  sign15[1][32] =
{
  0xff, 0xff, 0xfb, 0xf3, 0xeb, 0xfb, 0xfb, 0xfb,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0xff, 0xff,
  0xff, 0xff, 0x07, 0x7f, 0x7f, 0x7f, 0x7f, 0x07,
  0xf7, 0xf7, 0xf7, 0xf7, 0xf7, 0x07, 0xff, 0xff,
};

const unsigned char  sign20[1][32] =
{
  0xff, 0xff, 0xc1, 0xfd, 0xfd, 0xfd, 0xfd, 0xc1,
  0xdf, 0xdf, 0xdf, 0xdf, 0xdf, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbb, 0xbb, 0xbb, 0xbb, 0xbb,
  0xbb, 0xbb, 0xbb, 0xbb, 0xbb, 0x83, 0xff, 0xff,
};

const unsigned char  sign25[1][32] =
{
  0xff, 0xff, 0xc1, 0xfd, 0xfd, 0xfd, 0xfd, 0xc1,
  0xdf, 0xdf, 0xdf, 0xdf, 0xdf, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbf, 0xbf, 0xbf, 0xbf, 0x83,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0x83, 0xff, 0xff,
};

const unsigned char  sign30[1][32] =
{
  0xff, 0xFF, 0xc1, 0xfd, 0xfd, 0xfd, 0xfd, 0xf1,
  0xf1, 0xfd, 0xfd, 0xfd, 0xfd, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbb, 0xbb, 0xbb, 0xbb, 0xbb,
  0xbb, 0xbb, 0xbb, 0xbb, 0xbb, 0x83, 0xff, 0xff,
};

const unsigned char  sign35[1][32] =
{
  0xff, 0xFF, 0xc1, 0xfd, 0xfd, 0xfd, 0xfd, 0xf1,
  0xf1, 0xfd, 0xfd, 0xfd, 0xfd, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbf, 0xbf, 0xbf, 0xbf, 0x83,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0x83, 0xff, 0xff,
};

const unsigned char  sign40[1][32] =
{
  0xff, 0xff, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1,
  0xfd, 0xfd, 0xfd, 0xfd, 0xfd, 0xfd, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbb, 0xbb, 0xbb, 0xbb, 0xbb,
  0xbb, 0xbb, 0xbb, 0xbb, 0xbb, 0x83, 0xff, 0xff,
};

const unsigned char  sign45[1][32] =
{
  0xff, 0xff, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1,
  0xfd, 0xfd, 0xfd, 0xfd, 0xfd, 0xfd, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbf, 0xbf, 0xbf, 0xbf, 0x83,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0x83, 0xff, 0xff,
};

const unsigned char  sign50[1][32] =
{
  0xff, 0xff, 0xc1, 0xdf, 0xdf, 0xdf, 0xdf, 0xc1,
  0xfd, 0xfd, 0xfd, 0xfd, 0xfd, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbb, 0xbb, 0xbb, 0xbb, 0xbb,
  0xbb, 0xbb, 0xbb, 0xbb, 0xbb, 0x83, 0xff, 0xff,
};

const unsigned char  sign55[1][32] =
{
  0xff, 0xff, 0xc1, 0xdf, 0xdf, 0xdf, 0xdf, 0xc1,
  0xfd, 0xfd, 0xfd, 0xfd, 0xfd, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbf, 0xbf, 0xbf, 0xbf, 0x83,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0x83, 0xff, 0xff,
};

const unsigned char  sign60[1][32] =
{
  0xff, 0xff, 0xc1, 0xdf, 0xdf, 0xdf, 0xdf, 0xc1,
  0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbb, 0xbb, 0xbb, 0xbb, 0xbb,
  0xbb, 0xbb, 0xbb, 0xbb, 0xbb, 0x83, 0xff, 0xff,
};

const unsigned char  sign65[1][32] =
{
  0xff, 0xff, 0xc1, 0xdf, 0xdf, 0xdf, 0xdf, 0xc1,
  0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbf, 0xbf, 0xbf, 0xbf, 0x83,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0x83, 0xff, 0xff,
};

const unsigned char  sign70[1][32] =
{
  0xff, 0xff, 0xc1, 0xfd, 0xfd, 0xfb, 0xfb, 0xf7,
  0xf7, 0xf7, 0xef, 0xef, 0xdf, 0xdf, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbb, 0xbb, 0xbb, 0xbb, 0xbb,
  0xbb, 0xbb, 0xbb, 0xbb, 0xbb, 0x83, 0xff, 0xff,
};

const unsigned char  sign75[1][32] =
{
  0xff, 0xff, 0xc1, 0xfd, 0xfd, 0xfb, 0xfb, 0xf7,
  0xf7, 0xf7, 0xef, 0xef, 0xdf, 0xdf, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbf, 0xbf, 0xbf, 0xbf, 0x83,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0x83, 0xff, 0xff,
};

const unsigned char  sign80[1][32] =
{
  0xff, 0xff, 0xc1, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1,
  0xc1, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbb, 0xbb, 0xbb, 0xbb, 0xbb,
  0xbb, 0xbb, 0xbb, 0xbb, 0xbb, 0x83, 0xff, 0xff,
};

const unsigned char  sign85[1][32] =
{
  0xff, 0xff, 0xc1, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1,
  0xc1, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbf, 0xbf, 0xbf, 0xbf, 0x83,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0x83, 0xff, 0xff,
};

const unsigned char  sign90[1][32] =
{
  0xff, 0xff, 0xc1, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1,
  0xfd, 0xfd, 0xfd, 0xfd, 0xfd, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbb, 0xbb, 0xbb, 0xbb, 0xbb,
  0xbb, 0xbb, 0xbb, 0xbb, 0xbb, 0x83, 0xff, 0xff,
};

const unsigned char  sign95[1][32] =
{
  0xff, 0xff, 0xc1, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1,
  0xfd, 0xfd, 0xfd, 0xfd, 0xfd, 0xc1, 0xff, 0xff,
  0xff, 0xff, 0x83, 0xbf, 0xbf, 0xbf, 0xbf, 0x83,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0x83, 0xff, 0xff,
};

const unsigned char  sign100[1][32] =
{
  0xff, 0xff, 0xe8, 0xcb, 0xab, 0xeb, 0xeb, 0xeb,
  0xeb, 0xeb, 0xeb, 0xeb, 0xeb, 0xe8, 0xff, 0xff,
  0xff, 0xff, 0x43, 0x5b, 0x5b, 0x5b, 0x5b, 0x5b,
  0x5b, 0x5b, 0x5b, 0x5b, 0x5b, 0x43, 0xff, 0xff,
};

const unsigned char  sign110[1][32] =
{
  0xff, 0xff, 0xf7, 0xe6, 0xd5, 0xf7, 0xf7, 0xf7,
  0xf7, 0xf7, 0xf7, 0xf7, 0xf7, 0xf7, 0xff, 0xff,
  0xff, 0xff, 0x43, 0x5b, 0x5b, 0x5b, 0x5b, 0x5b,
  0x5b, 0x5b, 0x5b, 0x5b, 0x5b, 0x43, 0xff, 0xff,
};

const unsigned char  sign120[1][32] =
{
  0xff, 0xff, 0xe8, 0xcf, 0xaf, 0xef, 0xef, 0xe8,
  0xeb, 0xeb, 0xeb, 0xeb, 0xeb, 0xe8, 0xff, 0xff,
  0xff, 0xff, 0x43, 0x5b, 0x5b, 0x5b, 0x5b, 0x5b,
  0xdb, 0xdb, 0xdb, 0xdb, 0xdb, 0x43, 0xff, 0xff,
};

void setup()
{
  pinMode(LEDARRAY_D, OUTPUT);
  pinMode(LEDARRAY_C, OUTPUT);
  pinMode(LEDARRAY_B, OUTPUT);
  pinMode(LEDARRAY_A, OUTPUT);
  pinMode(LEDARRAY_G, OUTPUT);
  pinMode(LEDARRAY_DI, OUTPUT);
  pinMode(LEDARRAY_CLK, OUTPUT);
  pinMode(LEDARRAY_LAT, OUTPUT);
  //Display(Init_Display);
  Serial.begin(38400);
  sensors.begin();
  Servo1.attach(servoPin1);
  Servo2.attach(servoPin2);

  //set pinmode for all pins:
  pinMode(trigPin1, OUTPUT);
  pinMode(echoPin1, INPUT);
  pinMode(trigPin2, OUTPUT);
  pinMode(echoPin2, INPUT);
}

void loop()
{

  GetMessage();

  if (sign || speedLimit != 0)
  {
    DisplaySpeedLimit();
  }
  if (temp)
  {
    SendTemperature();
    temp = false;
  }
  if (dist)
  {
    GetDistance();
    dist = false;
  }
}







