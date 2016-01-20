#ifndef SHAREDOBJECT_H
#define SHAREDOBJECT_H

#include <string>
#include <stdexcept>
#include <mutex>

#include <sys/mman.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <fcntl.h>
#include <unistd.h>

template<class T>
class SharedObject
{
private:
    std::string name;
    T* data;
    std::mutex dataMutex;
    bool isReadonly;

public:
    SharedObject(std::string location, bool readonly);
    ~SharedObject();

    void writeData(T& newdata);
    T& readData();
};


template<class T>
SharedObject<T>::SharedObject(std::string location, bool readonly)
{
    name = location;
    isReadonly = readonly;
    uint32_t size = sizeof(T);

    int32_t flags = O_RDWR;
    if (readonly)
    {
        flags = O_RDONLY;
    }

    int32_t shmd = shm_open(
                location.c_str(),
                flags,
                0600);

    bool isnew = false;
    if (shmd == -1 &&
        !readonly)
    {
        shmd = shm_open(
                    location.c_str(),
                    O_RDWR | O_CREAT | O_EXCL,
                    0600);
        isnew = true;

        if (shmd == -1)
        {
            throw std::runtime_error(
                        "unable to create or open shared memory");
        }
    }
    else if (shmd == -1 && readonly)
    {
        throw std::runtime_error(
                    "unable to open shared memory");
    }


    if (isnew)
    {
        int32_t result = ftruncate(shmd, size);
        if (result == -1)
        {
            throw std::runtime_error(
                        "unable to set shared memory size");
        }
    }

    int32_t memflags = PROT_READ;
    if (!readonly)
    {
        memflags |= PROT_WRITE;
    }

    data = (T*)mmap(NULL, size, memflags,
            MAP_SHARED, shmd, 0);

    if (data == MAP_FAILED)
    {
        throw std::runtime_error(
                    "failed to map shared object");
    }
}

template<class T>
SharedObject<T>::~SharedObject()
{
    if (!isReadonly)
    {
        munmap(data, sizeof(T));
        shm_unlink(name.c_str());
    }
}

template<class T>
void SharedObject<T>::writeData(T& newdata)
{
    if (isReadonly)
    {
        throw std::runtime_error(
                    "writing to read only shared object");
    }

    std::lock_guard<std::mutex> guard(dataMutex);

    *data = newdata;
}

template<class T>
T& SharedObject<T>::readData()
{
    std::lock_guard<std::mutex> guard(dataMutex);

    return *data;
}

#endif // SHAREDOBJECT_H
