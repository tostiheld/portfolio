void SendTemperature()
{
  sensors.requestTemperatures();
  double temperature = sensors.getTempCByIndex(0);
  String tempMessage = ">TEMP:" + (String)temperature + ":;";
  if (temperature > -0.01 && temperature < 0.01)
  {
    Serial.print(">Sensor:Error_NotConnected;");
  }
  else
  {
  Serial.print(tempMessage);
  }
  meta = "";
}


