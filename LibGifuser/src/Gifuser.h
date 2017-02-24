#ifndef GIFUSER_H_H
#define GIFUSER_H_H

#include <stdint.h>

#if defined(_MSC_VER)
    #define GIFUSER_API extern "C" __declspec(dllexport)
#elif defined(__GNUC__)
    #define GIFUSER_API extern "C" __attribute__((visibility("default")))
#else
    #define GIFUSER_API extern "C" __declspec(dllexport)
#endif

#if defined(_WIN32)
#define GIFUSER_CALLCONV __stdcall
#else
#define GIFUSER_CALLCONV
#endif

typedef struct ScreenRecord ScreenRecord;

GIFUSER_API
ScreenRecord *
GIFUSER_CALLCONV
beginScreenRecord(const char *fileName, uint16_t delay);

GIFUSER_API
void
GIFUSER_CALLCONV
captureScreen(ScreenRecord *screenRecord);

GIFUSER_API
void
GIFUSER_CALLCONV
endScreenRecord(ScreenRecord *screenRecord);

#endif
