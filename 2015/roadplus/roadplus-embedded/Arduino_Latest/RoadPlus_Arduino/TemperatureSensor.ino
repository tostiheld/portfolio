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
    char tempStr[10];
    dtostrf(temperature, 5, 2, tempStr);
    
    Serial.print(">TEMP:");
    Serial.print(tempStr);
    Serial.print(":;");
  }
  meta = "";
}


