
void DisplaySpeedLimit()
{
  GetSpeedLimit();
  switch (speedLimit)
  {
    case 10: Display(sign10); break;
    case 15: Display(sign15); break;
    case 20: Display(sign20); break;
    case 25: Display(sign25); break;
    case 30: Display(sign30); break;
    case 35: Display(sign35); break;
    case 40: Display(sign40); break;
    case 45: Display(sign45); break;
    case 50: Display(sign50); break;
    case 55: Display(sign55); break;
    case 60: Display(sign60); break;
    case 65: Display(sign65); break;
    case 70: Display(sign70); break;
    case 75: Display(sign75); break;
    case 80: Display(sign80); break;
    case 85: Display(sign85); break;
    case 90: Display(sign90); break;
    case 95: Display(sign95); break;
    case 100: Display(sign100); break;
    case 110: Display(sign110); break;
    case 120: Display(sign120); break;
  }
}

void Display(const unsigned char dat[][32])
{
  unsigned char i;

  for ( i = 0 ; i < 16 ; i++ )
  {
    digitalWrite(LEDARRAY_G, HIGH);

    Display_Buffer[0] = dat[0][i];
    Display_Buffer[1] = dat[0][i + 16];

    Send(Display_Buffer[1]);
    Send(Display_Buffer[0]);

    digitalWrite(LEDARRAY_LAT, HIGH);
    delayMicroseconds(1);

    digitalWrite(LEDARRAY_LAT, LOW);
    delayMicroseconds(1);

    Scan_Line(i);

    digitalWrite(LEDARRAY_G, LOW);

    delayMicroseconds(100);
    ;
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
