/*
 * GifFrame.cpp
 *
 *  Created on: 4 de fev de 2017
 *      Author: gifuser
 */

#include "GifFrame.h"
#include <stdexcept>

GifFrame::GifFrame(uint16_t w, uint16_t h, uint16_t delay)
{
    if (w == 0)
    {
        throw std::invalid_argument("width must be positive");
    }

    if (h == 0)
    {
        throw std::invalid_argument("height must be positive");
    }

    _w = w;
    _h = h;
    _delay = delay;

    _buffer = new uint32_t[w * h];
}
GifFrame::~GifFrame()
{
    delete[] _buffer;
}
uint32_t GifFrame::getColor(uint16_t col, uint16_t row)
{
    uint32_t index = getIndex(col, row);
    return _buffer[index];
}
void GifFrame::setColor(uint16_t col, uint16_t row, uint32_t color)
{
    uint32_t index = getIndex(col, row);
    _buffer[index] = color;
}
uint32_t GifFrame::getIndex(uint16_t col, uint16_t row)
{
    if (col >= _w)
    {
        throw std::invalid_argument("col is out of range");
    }

    if (row >= _h)
    {
        throw std::invalid_argument("row is out of range");
    }

    return row * _w + col;
}
uint16_t GifFrame::getWidth() const
{
    return _w;
}
uint16_t GifFrame::getHeight() const
{
    return _h;
}
uint16_t GifFrame::getDelay() const
{
    return _delay;
}

const uint8_t* GifFrame::getBuffer() {
	return (const uint8_t *)_buffer;
}
