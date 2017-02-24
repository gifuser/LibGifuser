#ifndef GIFFILE_H_
#define GIFFILE_H_

#include "GifFrame.h"

class GifFile
{
public:
    GifFile(const char *fileName, uint16_t w, uint16_t h, uint16_t delay);
    ~GifFile();

    void write(GifFrame *frame);
    
private:

    void *_writer;
};

#endif
