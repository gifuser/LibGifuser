using System;

namespace Gifuser.Upload
{
	public class UploadCompletedEventArgs: EventArgs
	{
		private readonly bool _canceled;
		private readonly string _link;
		private readonly object _userState;
		private readonly Exception _error;

		public UploadCompletedEventArgs(bool canceled, string link, object userState, Exception ex)
		{
			_canceled = canceled;
			_link = link;
			_userState = userState;
			_error = ex;
		}

		public UploadCompletedEventArgs(bool canceled, string link)
			: this(canceled, link, null, null)
		{
			
		}

		public string Link
		{
			get
			{
				return _link;
			}
		}

		public Exception Error
		{
			get
			{
				return _error;
			}
		}

		public bool HasError
		{
			get
			{
				return _error != null;
			}
		}

		public bool Canceled
		{
			get
			{
				return _canceled;
			}
		}

		public object UserState
		{
			get
			{
				return _userState;
			}
		}
	}
}
