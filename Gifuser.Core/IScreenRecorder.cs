using System;

namespace Gifuser.Core
{
    public interface IScreenRecorder : IDisposable
    {
        event EventHandler<ScreenRecordCompletedEventArgs> Completed;
        event EventHandler<ScreenshotTakenEventArgs> ScreenshotTaken;

        bool Recording { get; }
        TimeSpan Elapsed { get; }
        TimeSpan Delay { get; set; }
        void StartAsync(string fileName, bool allowOverwrite);
        void FinishAsync();
    }
}
