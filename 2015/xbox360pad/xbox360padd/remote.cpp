#include <iostream>
#include <string>
#include <sstream>
#include <fstream>

#include <stdlib.h>
#include <string.h>

#include <libxbox360pad/gamepadstate.h>

#include "sharedobject.h"
#include "messagequeue.h"
#include "common.h"

#define BODY_LOCATION "/var/www/body.html"
#define IN_BUFFER_SIZE 1000

std::string getHeader()
{
    return "Content-type: text/html\n\n";
}

std::string getHead(std::string title)
{
    std::stringstream ss;
    ss << "<head>"
       << "<title>" << title << "</title>"
       << "<link rel='stylesheet type='text/css' href='/style.css'>"
       << "</head>";
    return ss.str();
}

std::string getBody()
{
    std::stringstream ss;
    std::ifstream body;
    body.open(BODY_LOCATION);

    if (body.is_open())
    {
        char buffer[IN_BUFFER_SIZE];
        while (body.getline(buffer, IN_BUFFER_SIZE))
        {
            ss << buffer;
        }
    }

    ss << "<script src='/ajax.js'></script>";
    return ss.str();
}

std::string getPadInfo()
{
    SharedObject<GamePadState> shared(std::string(SHARED_DATA_LOCATION), true);
    GamePadState state = shared.readData();
    std::stringstream ss;

    ss << "Content-type: application/json\n\n"
       << "{\"buttons\":{"
       << "\"A\":\"" << state.getButtons().A << "\","
       << "\"B\":\"" << state.getButtons().B << "\","
       << "\"Y\":\"" << state.getButtons().Y << "\","
       << "\"X\":\"" << state.getButtons().X << "\","
       << "\"Start\":\"" << state.getButtons().Start << "\","
       << "\"Back\":\"" << state.getButtons().Back << "\","
       << "\"Xbox\":\"" << state.getButtons().XboxButton << "\","
       << "\"LBumper\":\"" << state.getButtons().LeftShoulder << "\","
       << "\"RBumper\":\"" << state.getButtons().RightShoulder << "\","
       << "\"LStick\":\"" << state.getButtons().LeftStick << "\","
       << "\"RStick\":\"" << state.getButtons().RightStick << "\","
       << "\"Left\":\"" << state.getDPad().Left << "\","
       << "\"Right\":\"" << state.getDPad().Right << "\","
       << "\"Up\":\"" << state.getDPad().Up << "\","
       << "\"Down\":\"" << state.getDPad().Down << "\""
       << "},\"sticks\":{"
       << "\"LeftX\":\"" << state.getThumbsticks().LeftX << "\","
       << "\"LeftY\":\"" << state.getThumbsticks().LeftY << "\","
       << "\"RightX\":\"" << state.getThumbsticks().RightX << "\","
       << "\"RightY\":\"" << state.getThumbsticks().RightY << "\""
       << "},\"triggers\":{"
       << "\"Left\":\"" << (uint16_t)state.getTriggers().Left << "\","
       << "\"Right\":\"" << (uint16_t)state.getTriggers().Right << "\""
       << "}}";

    return ss.str();
}

int main()
{
    char* env_query = getenv("QUERY_STRING");
    if (env_query != NULL)
    {
        std::string query(env_query);

        if (query.compare("update") == 0)
        {
            std::cout << getPadInfo();
            return 0;
        }
        else if (query.compare(0, 7, "setleds") == 0)
        {
            MessageQueue mq(MSG_QUEUE_NAME, false);
            int32_t length = query.length() - 8;
            std::string mode = query.substr(8, length);

            Message msg;
            strcpy(msg.Command, "setleds");
            strcpy(msg.Payload, mode.c_str());
            mq.sendMessage(msg);

            std::cout << getHeader()
                      << "ok";
            return 0;
        }
        else if (query.compare(0, 9, "setrumble") == 0)
        {
            MessageQueue mq(MSG_QUEUE_NAME, false);
            int32_t length = query.length() - 10;
            std::string mode = query.substr(10, length);

            Message msg;
            strcpy(msg.Command, "setrumble");
            strcpy(msg.Payload, mode.c_str());
            mq.sendMessage(msg);

            std::cout << getHeader()
                      << "ok";
            return 0;
        }
    }

    std::cout << getHeader()
              << "<html>"
              << getHead("Gamepad status")
              << "<body>"
              << getBody()
              << "</body></html>";
    return 0;
}
