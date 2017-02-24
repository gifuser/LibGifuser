#include "Gifuser.h"
#include "GifFile.h"
#include <X11/Xlib.h>
#include <X11/Xutil.h>
#include <X11/extensions/Xfixes.h>

struct ScreenRecord {
    GifFile *file;
    GifFrame *frame;
};

GIFUSER_API
ScreenRecord *
GIFUSER_CALLCONV
beginScreenRecord(const char *fileName, uint16_t delay)
{
    Display *display = XOpenDisplay(NULL);
    int screen = XDefaultScreen(display);
    uint16_t w = (uint16_t)(XDisplayWidth(display, screen));
    uint16_t h = (uint16_t)(XDisplayHeight(display, screen));
    XCloseDisplay(display);

    GifFile *file = new GifFile(fileName, w, h, delay);
    GifFrame *frame = new GifFrame(w, h, delay);
    ScreenRecord *screenRecord = new ScreenRecord();

    screenRecord->file = file;
    screenRecord->frame = frame;
    return screenRecord;
}

GIFUSER_API
void
GIFUSER_CALLCONV
captureScreen(ScreenRecord *screenRecord)
{
    GifFile *file = screenRecord->file;
    GifFrame *frame = screenRecord->frame;
    
    Display *display = XOpenDisplay(NULL);
    int screen = XDefaultScreen(display);
    Window root = XRootWindow(display, screen);
    uint16_t w = frame->getWidth();
    uint16_t h = frame->getHeight();

    XImage *image = XGetImage(
        display,
        root,
        0,
        0,
        w,
        h,
        AllPlanes,
        ZPixmap
    );

    uint16_t row, col;
    uint32_t pixel, color;
    uint8_t red, green, blue;

    for (row = 0; row < h; row++)
    {
        for (col = 0; col < w; col++)
        {
            pixel = XGetPixel(image, col, row);
            
            red = (pixel & image->red_mask) >> 16;
            green = (pixel & image->green_mask) >> 8;
            blue = pixel & image->blue_mask;

            color = (blue << 16) | (green << 8) | red;

            frame->setColor(col, row, color);
        }
    }

    XFixesCursorImage *cursor = XFixesGetCursorImage(display);

    int16_t cursorCol, cursorRow;
    uint16_t cursorWidth, cursorHeight;
    
    cursorCol = cursor->x - cursor->xhot;
    cursorRow = cursor->y - cursor->yhot;
    cursorWidth = cursor->width;
    cursorHeight = cursor->height;

    if (cursorCol < 0)
    {
        cursorCol = 0;
    }

    if (cursorRow < 0)
    {
        cursorRow = 0;
    }

    uint16_t transparency;

    for (row = 0; row < cursorWidth; row++)
    {
        for (col = 0; col < cursorHeight; col++)
        {
            pixel = cursor->pixels[row * cursorWidth + col];

            transparency = pixel >> 24;

            if (transparency > 100)
            {
                red = pixel >> 16;
                green = pixel >> 8;
                blue = pixel;

                color = (blue << 16) | (green << 8) | red;

                frame->setColor(cursorCol + col, cursorRow + row, color);
            }
        }
    }

    XFree(cursor);

    file->write(frame);

    XDestroyImage(image);

    XCloseDisplay(display);
}

GIFUSER_API
void
GIFUSER_CALLCONV
endScreenRecord(ScreenRecord *screenRecord)
{
    delete screenRecord->file;
    delete screenRecord->frame;
    delete screenRecord;
}
