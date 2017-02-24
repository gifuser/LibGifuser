using System;

namespace Gifuser.Upload
{
	public interface IFileTrackedUpload : IFileUpload
	{
		event EventHandler<UploadProgressEventArgs> Progress;
	}
}
