#include "Gifuser.h"
#include "GifFile.h"
#include <windows.h>

struct ScreenRecord {
	GifFile *file;
	GifFrame *frame;
};

GIFUSER_API
ScreenRecord *
GIFUSER_CALLCONV
beginScreenRecord(const char *fileName, uint16_t delay)
{
	HDC screen = GetDC(NULL);
	uint16_t w = (uint16_t)(GetDeviceCaps(screen, HORZRES));
	uint16_t h = (uint16_t)(GetDeviceCaps(screen, VERTRES));
	ReleaseDC(NULL, screen);

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
	uint16_t w = frame->getWidth();
	uint16_t h = frame->getHeight();

	HDC screen = GetDC(NULL);
	HDC memory = CreateCompatibleDC(screen);
	HBITMAP bitmapScreen = CreateCompatibleBitmap(screen, w, h);
	HGDIOBJ obj = SelectObject(memory, bitmapScreen);
	BitBlt(memory, 0, 0, w, h, screen, 0, 0, SRCCOPY);

	CURSORINFO cursorInfo;

	cursorInfo.cbSize = sizeof(CURSORINFO);

	if (GetCursorInfo(&cursorInfo) && ((cursorInfo.flags & CURSOR_SHOWING) != 0))
	{
		ICONINFO iconInfo;

		if (GetIconInfo(cursorInfo.hCursor, &iconInfo))
		{
			DrawIcon(
				memory,
				cursorInfo.ptScreenPos.x - iconInfo.xHotspot,
				cursorInfo.ptScreenPos.y - iconInfo.yHotspot,
				cursorInfo.hCursor
			);

			DeleteObject(iconInfo.hbmColor);
			DeleteObject(iconInfo.hbmMask);
		}
	}

	BITMAPINFOHEADER bi;

	bi.biSize = sizeof(BITMAPINFOHEADER);
	bi.biWidth = w;
	bi.biHeight = -h;
	bi.biPlanes = 1;
	bi.biBitCount = 32;
	bi.biCompression = BI_RGB;
	bi.biSizeImage = 0;
	bi.biXPelsPerMeter = 0;
	bi.biYPelsPerMeter = 0;
	bi.biClrUsed = 0;
	bi.biClrImportant = 0;

	DWORD stride = ((w * bi.biBitCount + 31) / 32) * 4;
	DWORD bmpSize = stride * h;
	uint8_t *pixels = new uint8_t[bmpSize];

	GetDIBits(
		memory,
		bitmapScreen,
		0,
		(UINT)h,
		pixels,
		(BITMAPINFO *)(&bi),
		DIB_RGB_COLORS
	);

	uint16_t row, col;
	uint32_t rowOffset, colOffset;
	for (row = 0; row < h; row++)
	{
		rowOffset = row * stride;
		
		for (col = 0; col < w; col++)
		{
			colOffset = rowOffset + 4 * col;

			uint8_t blue = pixels[colOffset];
			uint8_t green = pixels[colOffset + 1];
			uint8_t red = pixels[colOffset + 2];

			uint32_t color = (blue << 16) | (green << 8) | red;

			frame->setColor(col, row, color);
		}
	}

	delete[] pixels;

	DeleteObject(bitmapScreen);
	DeleteDC(memory);
	ReleaseDC(NULL, screen);

	file->write(frame);
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
