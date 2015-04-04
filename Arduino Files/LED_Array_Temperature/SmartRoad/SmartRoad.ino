//***************************************************************************
//----------------1.开发环境:Arduino IDE or Visual Studio 2010----------------
//----------------2.使用开发板型号：Arduino UNO          ----------------
//----------------3.单片机使用晶振：16M		            ----------------
//----------------4.店铺名称：大学生电子商铺/小强电子设计  ----------------
//----------------5.淘宝网址：Ilovemcu.taobao.com		 ----------------
//----------------	    52dpj.taobao.com                ----------------
//----------------6.作者：神秘藏宝室			    ----------------
//***********************************************************************/
//voor serial
char incomingByte = 0;
String bericht;
String heleBericht;
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
const unsigned char  Word1[1][32] =
{
  0x00, 0x00, 0x03, 0x0f, 0x1f, 0x11, 0x3d, 0x31, 0x3d, 0x31, 0x1f, 0x1f, 0x0f, 0x03, 0x00, 0x00 ,
  0x00, 0x00, 0xc0, 0xf0, 0xf8, 0x18, 0x5c, 0x5c, 0x5c, 0x1c, 0xf8, 0xf8, 0xf0, 0xc0, 0x00, 0x00,
};

const unsigned char  Init_Display[1][32] =
{
  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
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

  Display(Init_Display);
  Serial.begin(9600);
  sensors.begin();
}

void loop()
{
  GetMessage();
  if (heleBericht == "GeefTemperatuur")
  {
    MeasureTemp();
    bericht = "";
    heleBericht = "";
  }
  Display(Word1);
}



//************************************************************
//num为字数  dat[][32]为字模的名称
//*************************************************************
void Display(const unsigned char dat[][32])
{
  unsigned char i;

  for ( i = 0 ; i < 16 ; i++ )
  {
    digitalWrite(LEDARRAY_G, HIGH);		//更新数据时候关闭显示。等更新完数据，打开138显示行。防止重影。

    Display_Buffer[0] = dat[0][i];
    Display_Buffer[1] = dat[0][i + 16];

    Send(Display_Buffer[1]);
    Send(Display_Buffer[0]);

    digitalWrite(LEDARRAY_LAT, HIGH);					//锁存数据
    delayMicroseconds(1);

    digitalWrite(LEDARRAY_LAT, LOW);
    delayMicroseconds(1);

    Scan_Line(i);							//选择第i行

    digitalWrite(LEDARRAY_G, LOW);

    delayMicroseconds(100);
    ;			//延时一段时间，让LED亮起来。
  }
}

//****************************************************
//扫描行
//****************************************************
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

//****************************************************
//发送数据
//****************************************************
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
    digitalWrite(LEDARRAY_CLK, HIGH);				//上升沿发送数据
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
    bericht += incomingByte;
  }
  int beginTeken = bericht.indexOf('>');
  int eindTeken = bericht.indexOf(';');
  if (beginTeken > -1 && eindTeken > -1) {
    heleBericht = bericht.substring(beginTeken + 1, eindTeken);
  }
}

void MeasureTemp()
{
  sensors.requestTemperatures();
  //String message = "#" + sensors.getTempCByIndex(0) + "%";
  double temperatuur = sensors.getTempCByIndex(0);
  String message = ">TEMP:" + (String)temperatuur + ":;";
  Serial.println(message);
  message = "";
}




