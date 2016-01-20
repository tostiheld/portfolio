#ifndef MESSAGEQUEUE_H
#define MESSAGEQUEUE_H

#include <string>
#include <stdexcept>
#include <thread>
#include <queue>
#include <mutex>

#include <fcntl.h>
#include <sys/stat.h>
#include <mqueue.h>
#include <time.h>
#include <errno.h>

struct Message{
    char Command[10];
    char Payload[20];
};

class MessageQueue
{
private:
    mqd_t Queue;
    bool isReadonly;
    std::queue<Message> Messages;

    bool Receive;
    std::thread* MessageThread;
    std::mutex MessageMutex;

    void waitForMessages();

public:
    MessageQueue(std::string name, bool readonly);
    ~MessageQueue();

    uint32_t checkMessages(Message* newmsg);
    void sendMessage(Message& message);
};

#endif // MESSAGEQUEUE_H
