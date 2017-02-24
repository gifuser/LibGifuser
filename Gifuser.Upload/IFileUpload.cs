using System;

namespace Gifuser.Upload
{
	public interface IFileUpload : IDisposable
	{
		string Name { get; }
		string Url { get; }
		long MaxFileSize { get; }
		bool ReportsProgress { get; }

		event EventHandler<UploadCompletedEventArgs> Completed;

		UploadRequirementStatus CheckRequirementStatus(string fileName);
		void StartAsync(string fileName);
		void StartAsync(string fileName, object userState);
		void CancelAsync();
	}
}
