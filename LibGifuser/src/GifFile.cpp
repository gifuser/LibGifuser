/*
 * GifFile.cpp
 *
 *  Created on: 4 de fev de 2017
 *      Author: gifuser
 */

#include "GifFile.h"
#include "gif.h"
#include <stdexcept>

GifFile::GifFile(const char *fileName, uint16_t w, uint16_t h, uint16_t delay)
{
    if (fileName == NULL)
    {
        throw std::invalid_argument("fileName cannot be null");
    }

    if (w == 0)
    {
        throw std::invalid_argument("width must be positive");
    }

    if (h == 0)
    {
        throw std::invalid_argument("height must be positive");
    }

    GifWriter *gwriter = new GifWriter();
    _writer = (void *)gwriter;
    GifBegin(gwriter, fileName, w, h, delay);
}

GifFile::~GifFile()
{
    GifWriter *gwriter = (GifWriter *)_writer;
    GifEnd(gwriter);
    delete gwriter;
}

void GifFile::write(GifFrame *frame)
{
    if (frame == NULL)
    {
        throw std::invalid_argument("frame cannot be null");
    }
    
    GifWriteFrame((GifWriter *)_writer, frame->getBuffer(), frame->getWidth(), frame->getHeight(), frame->getDelay());
}
