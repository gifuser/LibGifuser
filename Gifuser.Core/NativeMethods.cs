using System;
using System.Runtime.InteropServices;

namespace Gifuser.Core
{
    internal static class NativeMethods
    {
        [DllImport("Gifuser")]
        public static extern IntPtr beginScreenRecord(string fileName, ushort delay);

        [DllImport("Gifuser")]
        public static extern void captureScreen(IntPtr screenRecord, ushort delay);

        [DllImport("Gifuser")]
        public static extern void endScreenRecord(IntPtr screenRecord);

    }
}
