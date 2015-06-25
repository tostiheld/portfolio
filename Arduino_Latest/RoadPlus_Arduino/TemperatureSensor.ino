void SendTemperature()
{
  sensors.requestTemperatures();
  float temperature = sensors.getTempCByIndex(0);
    if (temperature > -0.01 && temperature < 0.01)
  {
    Serial.print(">Sensor:Error_NotConnected;");
  }
  else
  {
    String tempMessage = ">TEMP:" + (String)(temperature) + ":;";
    Serial.print(tempMessage);
  }
  meta = "";
}


