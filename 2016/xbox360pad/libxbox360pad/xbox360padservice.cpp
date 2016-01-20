#include "xbox360padservice.h"

#include <iostream>

int xbox360pad_hotplug(libusb_context *ctx, libusb_device *device, libusb_hotplug_event event, void *user_data)
{
    Xbox360PadService* service = (Xbox360PadService*)user_data;

    if (event == LIBUSB_HOTPLUG_EVENT_DEVICE_ARRIVED)
    {
        service->addPad(device);
    }
    else if (event == LIBUSB_HOTPLUG_EVENT_DEVICE_LEFT)
    {
        service->removePad(device);
    }

    return 0;
}

Xbox360PadService::Xbox360PadService()
{
    newPads = new std::queue<libusb_device*>();
    removedPads = new std::queue<libusb_device*>();
    newLedStates = new std::map<uint32_t, Xbox360Led>();
    newRumbleStates = new std::map<uint32_t, Xbox360Rumble>();

    libusb_init(&Context);

    libusb_hotplug_register_callback(
                Context, LIBUSB_HOTPLUG_EVENT_DEVICE_ARRIVED,
                LIBUSB_HOTPLUG_ENUMERATE,
                XBOX360PAD_VID, XBOX360PAD_PID,
                LIBUSB_HOTPLUG_MATCH_ANY,
                xbox360pad_hotplug, this, 0);

    libusb_hotplug_register_callback(
                Context, LIBUSB_HOTPLUG_EVENT_DEVICE_LEFT,
                LIBUSB_HOTPLUG_ENUMERATE,
                XBOX360PAD_VID, XBOX360PAD_PID,
                LIBUSB_HOTPLUG_MATCH_ANY,
                xbox360pad_hotplug, this, 0);

    running = true;
    pollThread = new std::thread(&Xbox360PadService::pollHardware, this);
}

Xbox360PadService::~Xbox360PadService()
{
    running = false;
    pollThread->join();
    delete pollThread;

    delete newPads;
    delete removedPads;
    delete newLedStates;
    delete newRumbleStates;

    for (uint32_t i = 0; i < ConnectedPads.size(); i++)
    {
        libusb_close(ConnectedPads.at(i).Handle);
    }

    libusb_exit(Context);
}

void Xbox360PadService::pollHardware()
{
    while (running)
    {
        timeval tv;
        tv.tv_sec = 0;
        tv.tv_usec = 0;
        libusb_handle_events_timeout_completed(
                    Context, &tv, NULL);

        processNewPads();
        processRemovedPads();
        processOutput();
        gatherInput();

        // sleep to reduce cpu usage
        std::chrono::microseconds timespan(500);
        std::this_thread::sleep_for(timespan);
    }
}

uint32_t Xbox360PadService::getConnectedPadCount()
{
    return ConnectedPads.size();
}

bool Xbox360PadService::getState(uint32_t index, Xbox360PadState* state)
{
    if (index >= ConnectedPads.size())
    {
        return false;
    }

    if (ConnectedPadsMutex.try_lock())
    {
        *state = ConnectedPads.at(index).State;
        ConnectedPadsMutex.unlock();
        return true;
    }

    return false;
}

void Xbox360PadService::setLeds(uint32_t index, Xbox360Led leds)
{
    //newLedStatesMutex.lock();

    std::map<uint32_t, Xbox360Led>::iterator it =
            newLedStates->find(index);

    if (it != newLedStates->end())
    {
        newLedStates->erase(it);
    }

    std::pair<uint32_t, Xbox360Led> p =
            std::pair<uint32_t, Xbox360Led>(index, leds);
    newLedStates->insert(p);

    //newLedStatesMutex.unlock();
}

void Xbox360PadService::setRumble(uint32_t index, Xbox360Rumble rumble)
{
    //newRumbleStatesMutex.lock();

    std::map<uint32_t, Xbox360Rumble>::iterator it =
            newRumbleStates->find(index);

    if (it != newRumbleStates->end())
    {
        newRumbleStates->erase(it);
    }

    std::pair<uint32_t, Xbox360Rumble> p =
            std::pair<uint32_t, Xbox360Rumble>(index, rumble);
    newRumbleStates->insert(p);

    //newRumbleStatesMutex.unlock();
}

void Xbox360PadService::processNewPads()
{
    if (ConnectedPadsMutex.try_lock())
    {
        while (!newPads->empty())
        {
            libusb_device* device = (libusb_device*)newPads->front();
            newPads->pop();

#if DEBUG
            //std::cout << "new pad!" << std::endl;
#endif

            libusb_device_handle* h;
            int32_t result = libusb_open(device, &h);

            if (result == 0)
            {
                Xbox360PadHardware pad;
                pad.Device = device;
                pad.Handle = h;

                libusb_detach_kernel_driver(h, 0);
                libusb_claim_interface(h, 0);

                ConnectedPads.push_back(pad);
            }
            else
            {
                std::cout << "error: " << result << std::endl;
            }
        }

        ConnectedPadsMutex.unlock();
    }
}

void Xbox360PadService::processRemovedPads()
{
    if (ConnectedPadsMutex.try_lock())
    {
        while (!removedPads->empty())
        {
            libusb_device* device = removedPads->front();
            removedPads->pop();

            for (std::vector<Xbox360PadHardware>::iterator it = ConnectedPads.begin();
                 it != ConnectedPads.end();
                 ++it)
            {
                if (it->Device == device)
                {
                    libusb_device_handle* handle = it->Handle;
                    ConnectedPads.erase(it);
                    libusb_close(handle);
                    return;
                }
            }
        }

        ConnectedPadsMutex.unlock();
    }
}

void Xbox360PadService::processOutput()
{
    if (ConnectedPadsMutex.try_lock())
    {
        uint32_t i = 0;
        typedef std::map<uint32_t, Xbox360Led>::iterator led_it;
        for(led_it it = newLedStates->begin();
            it != newLedStates->end(); it++)
        {
            libusb_device_handle* handle = ConnectedPads.at(i).Handle;

            int32_t transferred;
            int32_t result = libusb_interrupt_transfer(
                        handle,
                        XBOX360PAD_EPOUT,
                        (uint8_t*)&it->second,
                        sizeof(Xbox360Led),
                        &transferred,
                        XBOX360PAD_TIMEOUT);

            if (result != 0)
            {
                std::cout << "Write error!" << std::endl;
            }
            else
            {
                newLedStates->erase(it);
            }

            i++;
        }

        i = 0;
        typedef std::map<uint32_t, Xbox360Rumble>::iterator rumble_it;
        for(rumble_it it = newRumbleStates->begin();
            it != newRumbleStates->end(); it++)
        {
            libusb_device_handle* handle = ConnectedPads.at(i).Handle;

            int32_t transferred;
            int32_t result = libusb_interrupt_transfer(
                        handle,
                        XBOX360PAD_EPOUT,
                        (uint8_t*)&it->second,
                        sizeof(Xbox360Rumble),
                        &transferred,
                        XBOX360PAD_TIMEOUT);

            if (result != 0)
            {
                std::cout << "Write error!" << std::endl;
            }
            else
            {
                newRumbleStates->erase(it);
            }

            i++;
        }

        ConnectedPadsMutex.unlock();
    }
}

void Xbox360PadService::gatherInput()
{
    if (ConnectedPadsMutex.try_lock())
    {
        for (uint32_t i = 0; i < ConnectedPads.size(); i++)
        {
            Xbox360PadHardware* hw = &ConnectedPads.at(i);
            int32_t transferred;
            int32_t result = libusb_interrupt_transfer(
                        hw->Handle,
                        XBOX360PAD_EPIN,
                        (uint8_t*)&hw->State,
                        sizeof(Xbox360PadState),
                        &transferred,
                        XBOX360PAD_TIMEOUT);

            if (result != 0 &&
                result != LIBUSB_ERROR_TIMEOUT)
            {
                std::cout << "Read error!" << std::endl;
            }
        }

        ConnectedPadsMutex.unlock();
    }
}

void Xbox360PadService::addPad(libusb_device* device)
{
    newPads->push(device);
}

void Xbox360PadService::removePad(libusb_device* device)
{
    removedPads->push(device);
}
