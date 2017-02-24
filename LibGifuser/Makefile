# make CFG=Release XLIB_INC=/usr/include/ XLIB_LIB=/usr/lib/x86_64-linux-gnu/ XFIXES_LIB=/usr/lib/x86_64-linux-gnu/

CXX=g++
CXXFLAGS=-I$(XLIB_INC)
LIBS=-Wl,-lX11 -Wl,-lXfixes
LIBPATH=-Wl,-L$(XLIB_LIB) -Wl,-L$(XFIXES_LIB)
OUT_DIR=../$(CFG)
ASSEMBLY=libGifuser.so
OUT=$(OUT_DIR)/$(ASSEMBLY)

shared:
	mkdir -p $(OUT_DIR)
	$(CXX) -shared -fPIC $(CXXFLAGS) -o $(OUT) src/GifFrame.cpp src/GifFile.cpp src/Gifuser.xlib.cpp $(LIBPATH) $(LIBS)
clean:
	rm $(OUT)
