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

  if (meta == "12")
  {
    if (metaMessage == "On")
    {
      sonarOn = true;
    }
    else if(metaMessage == "Off")
    {
      sonarOn = false;
    }
    else if(metaMessage == "Read")
    {
      Serial.print(">Dens:");
      Serial.print(density);
      Serial.println(":;");      
    }
    meta = "";
  }
  if (meta == "13")
  {

  }
  if (meta == "14")
  {
    SendTemperature();
    meta = "";
  }
  if (meta == "11")
  {
    Serial.println(">ok:;");
    meta = "";
  }
  if (meta == "15")
  {
    if (metaMessage == "Off")
    {
      sign = false;
    }
    else
    {
      sign = true;
    }

  }
}






