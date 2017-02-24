using System;

namespace Gifuser.Upload
{
	public class UploadProgressEventArgs : EventArgs
	{
		private readonly int _progress;
		private readonly object _userState;

		public int Progress
		{
			get
			{
				return _progress;
			}
		}

		public object UserState
		{
			get
			{
				return _userState;
			}
		}

		public UploadProgressEventArgs(int progress, object userState)
		{
			if (progress < 0)
			{
				throw new ArgumentOutOfRangeException("progress");
			}

			_progress = progress;
			_userState = userState;
		}

		public UploadProgressEventArgs(int progress)
			: this(progress, null)
		{
			
		}
	}
}
