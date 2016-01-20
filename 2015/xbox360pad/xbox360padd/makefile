CXX=g++
CXX-ARM=arm-linux-g++
COMMON_CXXFLAGS=-Wall -Werror -std=gnu++11 -lrt -lpthread -lxbox360pad
DAEMON_CXXFLAGS=$(COMMON_CXXFLAGS)
REMOTE_CXXFLAGS=$(COMMON_CXXFLAGS)

COMMON_SOURCES=sharedobject.h messagequeue.cpp
DAEMON_SOURCES=$(COMMON_SOURCES) daemon.cpp
REMOTE_SOURCES=$(COMMON_SOURCES) remote.cpp

all: debug

arm: COMMON_CXXFLAGS+=-I../../opdracht2/ -L../../opdracht2/libxbox360pad/
arm: xbox360padd-arm xbox360padremote-arm

debug: COMMON_CXXFLAGS += -DDEBUG -g
debug: xbox360padd xbox360padremote

xbox360padd: $(DAEMON_SOURCES)
	$(CXX) $^ $(DAEMON_CXXFLAGS) -o $@

xbox360padremote: $(REMOTE_SOURCES)
	$(CXX) $^ $(REMOTE_CXXFLAGS) -o $@

xbox360padd-arm: $(DAEMON_SOURCES)
	$(CXX-ARM) $^ $(DAEMON_CXXFLAGS) -o $@

xbox360padremote-arm: $(REMOTE_SOURCES)
	$(CXX-ARM) $^ $(REMOTE_CXXFLAGS) -o $@

clean:
	rm -f xbox360padd xbox360padd-arm xbox360padremote xbox360padremote-arm

.PHONY: clean
