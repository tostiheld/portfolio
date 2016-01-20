#include <unistd.h>

#include <iostream>
#include <string>
#include <sstream>

#include <libxbox360pad/xbox360pad.h>
//#include <libxbox360pad/gamepadtypes.h>

#include "sharedobject.h"
#include "messagequeue.h"
#include "common.h"

bool run;
Xbox360Pad manager;

void setLeds(std::string mode)
{
    if (mode.compare("alloff") == 0)
    {
        manager.setLeds(0, LedAllOff);
    }
    else if(mode.compare("allblink") == 0)
    {
        manager.setLeds(0, LedAllBlink);
    }
    else if(mode.compare("firstonflash") == 0)
    {
        manager.setLeds(0, LedFirstOnFlash);
    }
    else if(mode.compare("secondonflash") == 0)
    {
        manager.setLeds(0, LedSecondOnFlash);
    }
    else if(mode.compare("thirdonflash") == 0)
    {
        manager.setLeds(0, LedThirdOnFlash);
    }
    else if(mode.compare("fourthonflash") == 0)
    {
        manager.setLeds(0, LedFourthOnFlash);
    }
    else if(mode.compare("firston") == 0)
    {
        manager.setLeds(0, LedFirstOn);
    }
    else if(mode.compare("secondon") == 0)
    {
        manager.setLeds(0, LedSecondOn);
    }
    else if(mode.compare("thirdon") == 0)
    {
        manager.setLeds(0, LedThirdOn);
    }
    else if(mode.compare("fourthon") == 0)
    {
        manager.setLeds(0, LedFourthOn);
    }
    else if(mode.compare("rotate") == 0)
    {
        manager.setLeds(0, LedRotate);
    }
    else if(mode.compare("blink") == 0)
    {
        manager.setLeds(0, LedBlink);
    }
    else if(mode.compare("slowblink") == 0)
    {
        manager.setLeds(0, LedSlowBlink);
    }
    else if(mode.compare("alternate") == 0)
    {
        manager.setLeds(0, LedAlternate);
    }
}

void setRumble(std::string mode)
{
    std::stringstream bigw_ss;
    std::stringstream smallw_ss;
    bool split = false;
    for (uint32_t i = 0; i < mode.length(); i++)
    {
        if (mode[i] == ',')
        {
            split = true;
            continue;
        }

        if (!split)
        {
            bigw_ss << mode[i];
        }
        else
        {
            smallw_ss << mode[i];
        }
    }

    uint16_t bigw;
    bigw_ss >> bigw;
    uint16_t smallw;
    smallw_ss >> smallw;

    manager.setRumble(0, bigw, smallw);
}

void parseMessage(Message& msg)
{
    std::string cmd(msg.Command);
    if (cmd.compare("setleds") == 0)
    {
        setLeds(std::string(msg.Payload));
    }
    else if (cmd.compare("setrumble") == 0)
    {
        setRumble(std::string(msg.Payload));
    }
}

int main()
{
    // don't daemonize in debug builds
    // gdb can't follow detached children
#ifndef DEBUG/*
    if (daemon(0, 0) != 0)
    {
        std::cout << "Failed to daemonize" << std::endl;
    }*/
#endif

    run = true;
    SharedObject<GamePadState> shared(std::string(SHARED_DATA_LOCATION), false);
    MessageQueue mq(std::string(MSG_QUEUE_NAME), true);

    while (run)
    {
        GamePadState state = manager.getState(0);
        shared.writeData(state);

        Message msg;
        while (mq.checkMessages(&msg) != 0)
        {
            parseMessage(msg);
        }

        std::chrono::microseconds delay(500);
        std::this_thread::sleep_for(delay);
    }

    return 0;
}
