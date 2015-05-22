
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

int test = 10;
int test2 = 0;

String First;
String Second;
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

//set constants:
const int timeOutPulseRead = 15000;
const int MaxDistance = 200.0;
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

const unsigned char  signWarning[32] =
{
  0xfe, 0xfd, 0xfd, 0xfb, 0xfa, 0xf6, 0xf6, 0xee,
  0xee, 0xde, 0xde, 0xbf, 0xbe, 0x7e, 0x7f, 0x00,
  0x7f, 0xbf, 0xbf, 0xdf, 0x5f, 0x6f, 0x6f, 0x77,
  0x77, 0x7b, 0x7b, 0xfd, 0x7d, 0x7e, 0xfe, 0x00,
};

const unsigned char  OneDirection[32] =
{
  0xf8, 0xe0, 0xe0, 0x80, 0x80, 0x00, 0x00, 0x0f,
  0x0f, 0x00, 0x00, 0x80, 0x80, 0xc0, 0xe0, 0xf8,
  0x1f, 0x07, 0x03, 0x01, 0x01, 0x00, 0x00, 0xf0,
  0xf0, 0x00, 0x00, 0x01, 0x01, 0x03, 0x07, 0x1f,
};


unsigned char  sign0[16] =
{
  0xff, 0xff, 0x83, 0xbb, 0xbb, 0xbb, 0xbb, 0xbb,
  0xbb, 0xbb, 0xbb, 0xbb, 0xbb, 0x83, 0xff, 0xff,
};
unsigned char  sign1[16] =
{
  0xff, 0xff, 0xfb, 0xf3, 0xeb, 0xfb, 0xfb, 0xfb,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0xff, 0xff,
};

const unsigned char  sign2[16] =
{
  0xff, 0xff, 0xc1, 0xfd, 0xfd, 0xfd, 0xfd, 0xc1,
  0xdf, 0xdf, 0xdf, 0xdf, 0xdf, 0xc1, 0xff, 0xff,
};

const unsigned char  sign3[16] =
{
  0xff, 0xFF, 0xc1, 0xfd, 0xfd, 0xfd, 0xfd, 0xf1,
  0xf1, 0xfd, 0xfd, 0xfd, 0xfd, 0xc1, 0xff, 0xff,
};

const unsigned char  sign4[16] =
{
  0xff, 0xff, 0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1,
  0xfd, 0xfd, 0xfd, 0xfd, 0xfd, 0xfd, 0xff, 0xff,
};

const unsigned char  sign5[16] =
{
  0xff, 0xff, 0x83, 0xbf, 0xbf, 0xbf, 0xbf, 0x83,
  0xfb, 0xfb, 0xfb, 0xfb, 0xfb, 0x83, 0xff, 0xff,
};

const unsigned char  sign6[16] =
{
  0xff, 0xff, 0xc1, 0xdf, 0xdf, 0xdf, 0xdf, 0xc1,
  0xdd, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1, 0xff, 0xff,
};

const unsigned char  sign7[16] =
{
  0xff, 0xff, 0xc1, 0xfd, 0xfd, 0xfb, 0xfb, 0xf7,
  0xf7, 0xf7, 0xef, 0xef, 0xdf, 0xdf, 0xff, 0xff,
};

const unsigned char  sign8[16] =
{
  0xff, 0xff, 0xc1, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1,
  0xc1, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1, 0xff, 0xff,
};

const unsigned char  sign9[16] =
{
  0xff, 0xff, 0xc1, 0xdd, 0xdd, 0xdd, 0xdd, 0xc1,
  0xfd, 0xfd, 0xfd, 0xfd, 0xfd, 0xc1, 0xff, 0xff,
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
  Serial.begin(19200);
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
  }


  test2++;
  if (test2 >= 20)
  {
    test2 = 0;
    test++;
    if (test > 100)
    {
      test = 0;
    }
  }
}


void DisplaySpeedLimit()
{  
  if (warning)
  {
    timetillnext = millis();
    if (timetillnext - previousTime > 1000)
    {
      previousTime = timetillnext;
      state = !state;      
    }
    if (state)
    {
      Display(signWarning);
    }
    else
    {
      GetSpeedLimit();
    }
  }
  else
  {
    GetSpeedLimit();
  }
}

void Display(const unsigned char dat[32])
{
  unsigned char i;

  for ( i = 0 ; i < 16 ; i++ )
  {
    digitalWrite(LEDARRAY_G, HIGH);

    Display_Buffer[0] = dat[i];
    Display_Buffer[1] = dat[i+ 16];

    Send(Display_Buffer[1]);
    Send(Display_Buffer[0]);

    digitalWrite(LEDARRAY_LAT, HIGH);
    delayMicroseconds(1);

    digitalWrite(LEDARRAY_LAT, LOW);
    delayMicroseconds(1);

    Scan_Line(i);

    digitalWrite(LEDARRAY_G, LOW);

    delayMicroseconds(100);
  }
}

void Scan_Line( unsigned char m)
{
  switch (m)
  {
  case 0:
    digitalWrite(LEDARRAY_D, LOW);
    digitalWrite(LEDARRAY_C, LOW);
    digitalWrite(LEDARRAY_B, LOW);
    digitalWrite(LEDARRAY_A, LOW);
    break;
  case 1:
    digitalWrite(LEDARRAY_D, LOW);
    digitalWrite(LEDARRAY_C, LOW);
    digitalWrite(LEDARRAY_B, LOW);
    digitalWrite(LEDARRAY_A, HIGH);
    break;
  case 2:
    digitalWrite(LEDARRAY_D, LOW);
    digitalWrite(LEDARRAY_C, LOW);
    digitalWrite(LEDARRAY_B, HIGH);
    digitalWrite(LEDARRAY_A, LOW);
    break;
  case 3:
    digitalWrite(LEDARRAY_D, LOW);
    digitalWrite(LEDARRAY_C, LOW);
    digitalWrite(LEDARRAY_B, HIGH);
    digitalWrite(LEDARRAY_A, HIGH);
    break;
  case 4:
    digitalWrite(LEDARRAY_D, LOW);
    digitalWrite(LEDARRAY_C, HIGH);
    digitalWrite(LEDARRAY_B, LOW);
    digitalWrite(LEDARRAY_A, LOW);
    break;
  case 5:
    digitalWrite(LEDARRAY_D, LOW);
    digitalWrite(LEDARRAY_C, HIGH);
    digitalWrite(LEDARRAY_B, LOW);
    digitalWrite(LEDARRAY_A, HIGH);
    break;
  case 6:
    digitalWrite(LEDARRAY_D, LOW);
    digitalWrite(LEDARRAY_C, HIGH);
    digitalWrite(LEDARRAY_B, HIGH);
    digitalWrite(LEDARRAY_A, LOW);
    break;
  case 7:
    digitalWrite(LEDARRAY_D, LOW);
    digitalWrite(LEDARRAY_C, HIGH);
    digitalWrite(LEDARRAY_B, HIGH);
    digitalWrite(LEDARRAY_A, HIGH);
    break;
  case 8:
    digitalWrite(LEDARRAY_D, HIGH);
    digitalWrite(LEDARRAY_C, LOW);
    digitalWrite(LEDARRAY_B, LOW);
    digitalWrite(LEDARRAY_A, LOW);
    break;
  case 9:
    digitalWrite(LEDARRAY_D, HIGH);
    digitalWrite(LEDARRAY_C, LOW);
    digitalWrite(LEDARRAY_B, LOW);
    digitalWrite(LEDARRAY_A, HIGH);
    break;
  case 10:
    digitalWrite(LEDARRAY_D, HIGH);
    digitalWrite(LEDARRAY_C, LOW);
    digitalWrite(LEDARRAY_B, HIGH);
    digitalWrite(LEDARRAY_A, LOW);
    break;
  case 11:
    digitalWrite(LEDARRAY_D, HIGH);
    digitalWrite(LEDARRAY_C, LOW);
    digitalWrite(LEDARRAY_B, HIGH);
    digitalWrite(LEDARRAY_A, HIGH);
    break;
  case 12:
    digitalWrite(LEDARRAY_D, HIGH);
    digitalWrite(LEDARRAY_C, HIGH);
    digitalWrite(LEDARRAY_B, LOW);
    digitalWrite(LEDARRAY_A, LOW);
    break;
  case 13:
    digitalWrite(LEDARRAY_D, HIGH);
    digitalWrite(LEDARRAY_C, HIGH);
    digitalWrite(LEDARRAY_B, LOW);
    digitalWrite(LEDARRAY_A, HIGH);
    break;
  case 14:
    digitalWrite(LEDARRAY_D, HIGH);
    digitalWrite(LEDARRAY_C, HIGH);
    digitalWrite(LEDARRAY_B, HIGH);
    digitalWrite(LEDARRAY_A, LOW);
    break;
  case 15:
    digitalWrite(LEDARRAY_D, HIGH);
    digitalWrite(LEDARRAY_C, HIGH);
    digitalWrite(LEDARRAY_B, HIGH);
    digitalWrite(LEDARRAY_A, HIGH);
    break;
  default :
    break;
  }
}

void Send( unsigned char dat)
{
  unsigned char i;
  digitalWrite(LEDARRAY_CLK, LOW);
  delayMicroseconds(1);
  ;
  digitalWrite(LEDARRAY_LAT, LOW);
  delayMicroseconds(1);
  ;

  for ( i = 0 ; i < 8 ; i++ )
  {
    if ( dat & 0x01 )
    {
      digitalWrite(LEDARRAY_DI, HIGH);
    }
    else
    {
      digitalWrite(LEDARRAY_DI, LOW);
    }

    delayMicroseconds(1);
    digitalWrite(LEDARRAY_CLK, HIGH);
    delayMicroseconds(1);
    digitalWrite(LEDARRAY_CLK, LOW);
    delayMicroseconds(1);
    dat >>= 1;

  }
}

void GetMessage()
{
  if (Serial.available() > 0)
  {
    incomingByte = (char)Serial.read();
    message += incomingByte;
  }
  int startSymbol = message.indexOf('>');
  int endSymbol = message.indexOf(';');
  int metaSymbol = message.indexOf(':');
  if (startSymbol > -1 && endSymbol > -1 && metaSymbol > -1)
  {
    meta = message.substring(startSymbol + 1, metaSymbol);
    metaMessage = message.substring(metaSymbol + 1, endSymbol);
    if (metaMessage.indexOf(':') > -1)
    {
      metaMessage = metaMessage.substring(0, metaMessage.indexOf(':'));
    }
    message = "";
  }
  if (meta == "SIGN")
  {
    sign = true;
  }
  if (meta == "DIST")
  {
    dist = true;
  }
  if (meta == "TEMP")
  {
    temp = true;
  }
  if (meta == "discover")
  {
    Serial.print(">ok:;");
  }
}

void GetSpeedLimit()
{    
  unsigned char complete[32] = {
    0x00, 0x00, 0x00, 0x00,  0x00, 0x00, 0x00, 0x00,
    0xFF, 0xFF, 0xFF, 0xFF,  0xFF, 0xFF, 0xFF, 0xFF,
    0x00, 0x00, 0x00, 0x00,  0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00,  0x00, 0x00, 0x00, 0x00,    
  };

  if (metaMessage == "Warning")
  {
    warning = true;
  }
  else if (metaMessage == "NoWarning")
  {
    warning = false;
  }
  else if (metaMessage.length() <= 2)
  {
    String metaMessage = (String)test;
    if (metaMessage.length() == 1)
    {
      metaMessage = "0" + metaMessage;
    }

    First = metaMessage.substring(0, 1);
    Second = metaMessage.substring(1, 2);

  }

  if (metaMessage == "1way")
  {
    Display(OneDirection);
  }
  else
  { 
    if (First == "0")
    {
      for (int i=0; i<16; i++)
      {
        complete[i] = sign0[i];
      }
    }
    else if (First == "1")
    {
      for (int i=0; i<16; i++)
      {
        complete[i] = sign1[i];
      }
    }
    else if (First == "2")
    {
      for (int i=0; i<16; i++)
      {
        complete[i] = sign2[i];
      }
    }
    else if (First == "3")
    {
      for (int i=0; i<16; i++)
      {
        complete[i] = sign3[i];
      }
    }
    else if (First == "4")
    {
      for (int i=0; i<16; i++)
      {
        complete[i] = sign4[i];
      }
    }
    else if (First == "5")
    {
      for (int i=0; i<16; i++)
      {
        complete[i] = sign5[i];
      }
    }
    else if (First == "6")
    {
      for (int i=0; i<16; i++)
      {
        complete[i] = sign6[i];
      }
    }
    else if (First == "7")
    {
      for (int i=0; i<16; i++)
      {
        complete[i] = sign7[i];
      }
    }
    else if (First == "8")
    {
      for (int i=0; i<16; i++)
      {
        complete[i] = sign8[i];
      }
    }
    else if (First == "9")
    {
      for (int i=0; i<16; i++)
      {
        complete[i] = sign9[i];
      }
    }

    if (Second == "0")
    {
      for (int i=0; i<16; i++)
      {
        complete[i + 16] = sign0[i];
      }
    }
    else if (Second == "1")
    {
      for (int i=0; i<16; i++)
      {
        complete[i + 16] = sign1[i];
      }
    }
    else if (Second == "2")
    {
      for (int i=0; i<16; i++)
      {
        complete[i + 16] = sign2[i];
      }
    }
    else if (Second == "3")
    {
      for (int i=0; i<16; i++)
      {
        complete[i + 16] = sign3[i];
      }
    }
    else if (Second == "4")
    {
      for (int i=0; i<16; i++)
      {
        complete[i + 16] = sign4[i];
      }
    }
    else if (Second == "5")
    {
      for (int i=0; i<16; i++)
      {
        complete[i + 16] = sign5[i];
      }
    }
    else if (Second == "6")
    {
      for (int i=0; i<16; i++)
      {
        complete[i + 16] = sign6[i];
      }
    }
    else if (Second == "7")
    {
      for (int i=0; i<16; i++)
      {
        complete[i + 16] = sign7[i];
      }
    }
    else if (Second == "8")
    {
      for (int i=0; i<16; i++)
      {
        complete[i + 16] = sign8[i];
      }
    }
    else if (Second == "9")
    {
      for (int i=0; i<16; i++)
      {
        complete[i + 16] = sign9[i];
      }
    }
    Display(complete);
  }

}
























