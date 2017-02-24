/*
 * GifFrame.h
 *
 *  Created on: 4 de fev de 2017
 *      Author: gifuser
 */

#ifndef GIFFRAME_H_
#define GIFFRAME_H_

#include <stdint.h>

class GifFrame
{
public:
    GifFrame(uint16_t w, uint16_t h, uint16_t delay);
    ~GifFrame();

    uint32_t getColor(uint16_t col, uint16_t row);
    void setColor(uint16_t col, uint16_t row, uint32_t color);

    const uint8_t *getBuffer();

    uint16_t getWidth() const;
    uint16_t getHeight() const;
    uint16_t getDelay() const;

private:

    uint32_t getIndex(uint16_t col, uint16_t row);

    uint32_t *_buffer;
    uint16_t _w;
    uint16_t _h;
    uint16_t _delay;
};

#endif /* GIFFRAME_H_ */
