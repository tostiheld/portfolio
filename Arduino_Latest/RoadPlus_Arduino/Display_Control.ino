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
