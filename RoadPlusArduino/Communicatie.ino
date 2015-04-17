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
    meta = message.substring(startSymbol + 1, metaSymbol); //xxxx
    metaMessage = message.substring(metaSymbol + 1, endSymbol); //iets
    if (metaMessage.indexOf(':') > -1)
    {
      metaMessage = metaMessage.substring(0, metaMessage.indexOf(':'));
    }
    message = "";
  }
}

void GetSpeedLimit()
{
  if (metaMessage == "10")
  {
    speedLimit = 10;
  }
  if (metaMessage == "15")
  {
    speedLimit = 15;
  }
  if (metaMessage == "20")
  {
    speedLimit = 20;
  }
  if (metaMessage == "25")
  {
    speedLimit = 25;
  }
  if (metaMessage == "30")
  {
    speedLimit = 30;
  }
  if (metaMessage == "35")
  {
    speedLimit = 35;
  }
  if (metaMessage == "40")
  {
    speedLimit = 40;
  }
  if (metaMessage == "45")
  {
    speedLimit = 45;
  }
  if (metaMessage == "50")
  {
    speedLimit = 50;
  }
  if (metaMessage == "55")
  {
    speedLimit = 55;
  }
  if (metaMessage == "60")
  {
    speedLimit = 60;
  }
  if (metaMessage == "65")
  {
    speedLimit = 65;
  }
  if (metaMessage == "70")
  {
    speedLimit = 70;
  }
  if (metaMessage == "75")
  {
    speedLimit = 75;
  }
  if (metaMessage == "80")
  {
    speedLimit = 80;
  }
  if (metaMessage == "85")
  {
    speedLimit = 85;
  }
  if (metaMessage == "90")
  {
    speedLimit = 90;
  }
  if (metaMessage == "95")
  {
    speedLimit = 95;
  }
  if (metaMessage == "100")
  {
    speedLimit = 100;
  }
  if (metaMessage == "110")
  {
    speedLimit = 110;
  }
  if (metaMessage == "120")
  {
    speedLimit = 120;
  }
}
