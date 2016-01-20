#ifndef XBOX360PADSERVICE_H
#define XBOX360PADSERVICE_H

#include <time.h>

#include <vector>
#include <queue>
#include <map>
#include <thread>
#include <mutex>

#include <libusb-1.0/libusb.h>

#define XBOX360PAD_VID   0x045E
#define XBOX360PAD_PID   0x028E
#define XBOX360PAD_EPIN  0x81
#define XBOX360PAD_EPOUT 0x01

#define XBOX360PAD_TIMEOUT 20


#define XBOX360PAD_DPAD_UP    0
#define XBOX360PAD_DPAD_DOWN  1
#define XBOX360PAD_DPAD_LEFT  2
#define XBOX360PAD_DPAD_RIGHT 3

#define XBOX360PAD_BUTTON_START 4
#define XBOX360PAD_BUTTON_BACK  5

#define XBOX360PAD_BUTTON_LEFTSTICK  6
#define XBOX360PAD_BUTTON_RIGHTSTICK 7

#define XBOX360PAD_LEFTBUMPER  8
#define XBOX360PAD_RIGHTBUMPER 9

#define XBOX360PAD_BUTTON_XBOX 10

#define XBOX360PAD_BUTTON_A 12
#define XBOX360PAD_BUTTON_B 13
#define XBOX360PAD_BUTTON_X 14
#define XBOX360PAD_BUTTON_Y 15


#define XBOX360PAD_LED_ALLOFF   0x00
#define XBOX360PAD_LED_ALLBLINK 0x01

#define XBOX360PAD_LED_FIRST_ON_FLASH  0x02
#define XBOX360PAD_LED_SECOND_ON_FLASH 0x03
#define XBOX360PAD_LED_THIRD_ON_FLASH  0x04
#define XBOX360PAD_LED_FOURTH_ON_FLASH 0x05

#define XBOX360PAD_LED_FIRST_ON  0x06
#define XBOX360PAD_LED_SECOND_ON 0x07
#define XBOX360PAD_LED_THIRD_ON  0x08
#define XBOX360PAD_LED_FOURTH_ON 0x09

#define XBOX360PAD_LED_ROTATE    0x0A
#define XBOX360PAD_LED_BLINK     0x0B
#define XBOX360PAD_LED_SLOWBLINK 0x0C
#define XBOX360PAD_LED_ALTERNATE 0x0D


struct Xbox360PadState
{
    uint8_t  Type;
    uint8_t  Size;
    uint16_t Buttons;
    uint8_t  LeftTrigger;
    uint8_t  RightTrigger;
    int16_t  LeftStickX;
    int16_t  LeftStickY;
    int16_t  RightStickX;
    int16_t  RightStickY;
    uint8_t  padding[6];
} __attribute__((packed));

struct Xbox360Led
{
    uint8_t Type = 0x01;
    uint8_t Size = 0x03;
    uint8_t LedMode;
} __attribute__((packed));

struct Xbox360Rumble
{
    uint8_t Type = 0x00;
    uint8_t Size = 0x08;
    uint8_t p1 = 0x00;
    uint8_t BigWeight;
    uint8_t SmallWeight;
    uint8_t p2 = 0x0;
    uint8_t p3 = 0x0;
    uint8_t p4 = 0x0;
} __attribute__((packed));

struct Xbox360PadHardware
{
    libusb_device* Device;
    libusb_device_handle* Handle;
    Xbox360PadState State;
    Xbox360Led ledState;
    Xbox360Rumble rumbleState;
};

class Xbox360PadService
{
private:
    libusb_context* Context;
    std::vector<Xbox360PadHardware> ConnectedPads;

    std::queue<libusb_device*>* newPads;
    std::queue<libusb_device*>* removedPads;
    std::map<uint32_t, Xbox360Led>* newLedStates;
    std::map<uint32_t, Xbox360Rumble>* newRumbleStates;

    std::thread* pollThread;
    volatile bool running;
    std::mutex ConnectedPadsMutex;
    std::mutex newLedStatesMutex;
    std::mutex newRumbleStatesMutex;

    void pollHardware();

    void processNewPads();
    void processRemovedPads();
    void processOutput();
    void gatherInput();

public:
    Xbox360PadService();
    ~Xbox360PadService();

    uint32_t getConnectedPadCount();

    bool getState(uint32_t index, Xbox360PadState* state);
    void setLeds(uint32_t index, Xbox360Led leds);
    void setRumble(uint32_t index, Xbox360Rumble rumble);

    void addPad(libusb_device* device);
    void removePad(libusb_device* device);
};

int xbox360pad_hotplug(libusb_context* ctx, libusb_device* device, libusb_hotplug_event event, void* user_data);

#endif // XBOX360PADSERVICE_H
