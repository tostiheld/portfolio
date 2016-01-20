#include "messagequeue.h"

MessageQueue::MessageQueue(std::string name, bool readonly)
{
    std::string tmpname = name;
    isReadonly = readonly;

    int32_t flags = O_CREAT | O_NONBLOCK;
    if (isReadonly == true)
    {
        flags |= O_RDONLY;
    }
    else
    {
        flags |= O_WRONLY;
    }

    struct mq_attr attr;
    attr.mq_flags = 0;
    attr.mq_maxmsg = 10;
    attr.mq_msgsize = sizeof(Message);
    attr.mq_curmsgs = 0;

    Queue = mq_open(tmpname.c_str(), flags, 0644, &attr);

    if (Queue == -1)
    {
        throw std::runtime_error(
                    "Unable to open message queue");
    }

    Receive = false;
    MessageThread = NULL;
    if (isReadonly)
    {
        Receive = true;
        MessageThread = new std::thread(&MessageQueue::waitForMessages, this);
    }
}

MessageQueue::~MessageQueue()
{
    if (MessageThread != NULL)
    {
        Receive = false;
        MessageThread->join();
        delete MessageThread;
    }
}

void MessageQueue::waitForMessages()
{
    while (Receive)
    {
        if (MessageMutex.try_lock())
        {
            Message newMessage;
            struct timespec ts;
            clock_gettime(CLOCK_REALTIME, &ts);
            ts.tv_nsec += 100000;

            ssize_t result = mq_timedreceive(
                        Queue, (char*)&newMessage,
                        sizeof(Message), NULL,
                        &ts);

            if (result == sizeof(Message))
            {
                Messages.push(newMessage);
            }

            MessageMutex.unlock();
        }

        std::chrono::microseconds delay(500);
        std::this_thread::sleep_for(delay);
    }
}

uint32_t MessageQueue::checkMessages(Message* newmsg)
{
    if (!isReadonly)
    {
        return 0;
    }

    MessageMutex.lock();
    uint32_t size = Messages.size();

    if (Messages.size() > 0)
    {
        *newmsg = Messages.front();
        Messages.pop();
    }

    MessageMutex.unlock();
    return size;
}

void MessageQueue::sendMessage(Message& message)
{
    if (isReadonly)
    {
        return;
    }

    struct timespec ts;
    clock_gettime(CLOCK_REALTIME, &ts);
    ts.tv_nsec += 100000;

    mq_timedsend(Queue, (const char*)&message, sizeof(Message),
                 0, &ts);
}
