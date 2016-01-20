//set Variables:
String First;
String Second;
long previousTime;

//checks for need of warning sign, if there is no need display only speed limit, else display both
void DisplaySpeedLimit()
{  
  if (warning)
  {
    if (millis() - previousTime > 1000)
    {
      previousTime = millis();
      state = !state;      
    }
    if (state)
    {
      Display(signWarning);
    }
    else
    {
      UpdateSign();
    }
  }
  else
  {    
    UpdateSign();
  }
}

//converts a String into a Array and export this to the base code
void UpdateSign()
{
  //create a base bitmap, for control
  unsigned char complete[32] = {
    0xFF, 0x00, 0xFF, 0x00,  0xFF, 0x00, 0xFF, 0x00,
    0x00, 0xFF, 0x00, 0xFF,  0x00, 0xFF, 0x00, 0xFF,
    0xFF, 0x00, 0xFF, 0x00,  0xFF, 0x00, 0xFF, 0x00,
    0x00, 0xFF, 0x00, 0xFF,  0x00, 0xFF, 0x00, 0xFF,    
  };

  if (metaMessage == "Warning" && meta.indexOf("15") != -1)
  {
    warning = true;
  }
  else if (metaMessage == "NoWarning" && meta.indexOf("15") != -1)
  {
    warning = false;
  }
  else if (metaMessage == "1way" && meta.indexOf("15") != -1)
  {
    oneWay = true;
  }
  else if (metaMessage == "No1way" && meta.indexOf("15") != -1)
  {
    oneWay = false;
  }
  
  else if ((metaMessage.length() == 2 || metaMessage.length() == 1) && meta.indexOf("15") != -1)
  {
    if (metaMessage.length() == 1)
    {
      metaMessage = "0" + metaMessage;
    }
    First = metaMessage.substring(0, 1);
    Second = metaMessage.substring(1, 2);

  }
  //if the display is needed to display a oneway sign, display the sign, else, display the correct speedlimit

  if (oneWay)
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
    else
    {  
      Serial.print(">Sign:InvalidParameterSpeed:;");
      sign = false;
      meta = "";
      metaMessage = "";  
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
    else
    {  
      Serial.print(">Error:InvalidParameterSign:;");
      sign = false;
      meta = "";
      metaMessage = "";
    }
    Display(complete);
  }
}





