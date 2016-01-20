CXX=g++
CXX-ARM=arm-linux-g++
CXXFLAGS=-Wall -Werror -I. -std=gnu++11 -shared -fPIC
LIBS=-lusb-1.0 -lpthread

OBJS=gamepadstate.o xbox360pad.o xbox360padservice.o

all: debug

release: CXXFLAGS += -O2
release: libxbox360pad.so

debug: CXXFLAGS += -DDEBUG -g
debug: libxbox360pad.so

install: libxbox360pad.so
	mkdir -p /usr/include/libxbox360pad/
	cp *.h /usr/include/libxbox360pad/
	cp libxbox360pad.so /usr/lib/

libxbox360pad.so: $(OBJS)
	$(CXX) $^ $(LIBS) $(CXXFLAGS) -o $@

libxbox360pad-arm.so: CXXFLAGS += -O2
libxbox360pad-arm.so: override CXX=arm-linux-g++
libxbox360pad-arm.so: $(OBJS)
	$(CXX) $^ $(LIBS) $(CXXFLAGS) -o $@

%.o: %.c
	$(CXX) $^ $(CXXFLAGS) -o $@

clean:
	rm -f *.o *.so

.PHONY: clean
