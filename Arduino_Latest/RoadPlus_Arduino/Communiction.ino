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
