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
    Serial.print(">Message:Received:;"); 
    message = "";
  }

  if (meta == "12")
  {
    if (metaMessage == "On")
    {
      TestConnection();      
      meta = "";
      metaMessage = "";
    }
    else if(metaMessage == "Off")
    {
      sonarOn = false;
    }
    else if(metaMessage == "Read")
    {
      if(sonarOn)
      {
        Serial.print(">Dens:");
        Serial.print(density);
        Serial.print(":;");
      }
      else
      {
        Serial.print(">Dens:ErrorSonarOff:;");
      }
          
    }
    meta = "";
  }
  if (meta == "13")
  {

  }
  //Send temperature to pc
  if (meta == "14")
  {
    SendTemperature(); 
    meta = "";
  }
  //Discovery echo
  if (meta == "11")
  {
    Serial.print(">ok:;");
    meta = "";
  }
  if (meta == "Discover")
  {
    Serial.print(">ok:;");
    meta = "";
  }
  //turn the sign On or Off
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







