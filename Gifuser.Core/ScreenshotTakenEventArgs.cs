using System;

namespace Gifuser.Core
{
    public class ScreenshotTakenEventArgs : EventArgs
    {
        private readonly int _frames;
        private readonly long _currentFileSize;

        public int Frames
        {
            get
            {
                return _frames;
            }
        }

        public long CurrentFileSize
        {
            get
            {
                return _currentFileSize;
            }
        }

        public ScreenshotTakenEventArgs(long currentFileSize, int frames)
        {
            if (currentFileSize < 0L)
            {
                throw new ArgumentOutOfRangeException("currentFileSize");
            }

            if (frames < 0)
            {
                throw new ArgumentOutOfRangeException("frames");
            }

            _currentFileSize = currentFileSize;
            _frames = frames;
        }
    }
}
