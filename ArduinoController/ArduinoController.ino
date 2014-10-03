/*
  TowerHunter Arduino control
*/

#include <CmdMessenger.h>

// Inputs and outputs
int towers = 2;
int sensorPin[] = {A0, A1};  // Input: Initialized sensor pins
int ledPin[] = {0, 1};      // Output: Initialized LED pins
int threshold = 100;
bool towerOn[] = {0, 0};  // Which tower is on or off, both are off
bool towerHit[] = {0, 0};

CmdMessenger cmdMessenger = CmdMessenger(Serial);

enum {
  kAcknowledge,
  kError,
  kOnLed ,        // Command to turn on a LED
  kOffLed,        // Command to turn off a LED
  kReadSensors,   // Command to start reading sensors
  kResetTower,    // Command to reset the hit state of the towers
};

// Callbacks define on which received commands we take action
void attachCommandCallbacks()
{
  // Attach callback methods
  cmdMessenger.attach(OnUnknownCommand);
  cmdMessenger.attach(kOnLed, onLed);
  cmdMessenger.attach(kOffLed, offLed);
  cmdMessenger.attach(kReadSensors, readSensors);
  cmdMessenger.attach(kResetTower, resetTower);
}

void OnUnknownCommand()
{
  cmdMessenger.sendCmd(kError,"Command without attached callback");
}

void OnArduinoReady()
{
  cmdMessenger.sendCmd(kAcknowledge,"Arduino ready");
}

// onLed() has one argument, the index of the LED/tower to be turned on
void onLed() {
  int pin = cmdMessenger.readInt16Arg(); //first argument as integer
  digitalWrite(ledPin[pin], HIGH);
}

void offLed() {
  int pin = cmdMessenger.readInt16Arg(); // first argument as integer
  digitalWrite(ledPin[pin], LOW);
}

// if you call readSensors it'll (amount of towers) times a bool (0 or 1)
void readSensors(){
  for(int i=0; i<towers; i++){
    if ( analogRead(sensorPin[i]) > threshold){
      towerHit[i] = true;
    }
  cmdMessenger.sendCmd(kAcknowledge,towerHit[i]);
  }
}

void resetTower() {
  int pin = cmdMessenger.readInt16Arg(); // first argument as integer
  towerHit[pin] = false;
}


void setup() {
  Serial.begin(115200);
  
  attachCommandCallbacks();
  
  cmdMessenger.sendCmd(kAcknowledge,"Arduino has started!");
  
  // set pinmode for writing LEDs
  for(int i=0; i<towers; i++){
    pinMode(ledPin[i],OUTPUT);
    digitalWrite(ledPin[i],HIGH);
  }
}

void loop() {
  cmdMessenger.feedinSerialData();
  delay(10);
}
