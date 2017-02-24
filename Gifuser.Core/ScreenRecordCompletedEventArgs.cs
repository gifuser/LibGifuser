using System;
using System.IO;

namespace Gifuser.Core
{
    public class ScreenRecordCompletedEventArgs : EventArgs
    {
        private readonly string _fileName;
        private readonly TimeSpan _delay;
        private readonly int _frames;
		private readonly Exception _error;

        public ScreenRecordCompletedEventArgs(string fileName, TimeSpan delay, int frames)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Invalid fileName");
            }
            else if (!File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }

            if (frames < 0)
            {
                throw new ArgumentOutOfRangeException("frames", "The number of frames cannot be negative");
            }

            _fileName = fileName;
            _delay = delay;
            _frames = frames;
            _error = null;
        }

		public ScreenRecordCompletedEventArgs(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}

            _fileName = null;
            _delay = TimeSpan.Zero;
            _frames = 0;
			_error = exception;
		}

		public string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public TimeSpan Delay
        {
            get
            {
                return _delay;
            }
        }

        public int Frames
        {
            get
            {
                return _frames;
            }
        }

		public Exception Error
		{
			get
			{
				return _error;
			}
		}
	}
}
