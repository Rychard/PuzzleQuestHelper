using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace PuzzleQuestHelper.Logic
{
    public static class WindowHelper
    {
        public static Bitmap GetGameWindow(String processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            Process pq = processes[0];
            IntPtr handle = pq.MainWindowHandle;

            // Set the focus on the specified window.
            SetForegroundWindow(handle);
            ShowWindow(handle, SW_RESTORE);

            System.Threading.Thread.Sleep(100);

            // Get a rectangle that represents the location (and size) of the window in pixels.
            UnmanagedRectangle rect;
            if (!GetWindowRect(new HandleRef(null, handle), out rect)) { return null; }

            var width = (rect.Right - rect.Left);
            var height = (rect.Bottom - rect.Top);
            Point topLeft = new Point(rect.Left, rect.Top);
            Bitmap bmp = new Bitmap(width, height);
            
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(topLeft, new Point(0, 0), new Size(bmp.Width, bmp.Height), CopyPixelOperation.SourceCopy);
            }
            return bmp;
        }

        public static Bitmap GetGameWindow(String processName, Rectangle region)
        {
            var destRectangle = new Rectangle(0, 0, region.Width, region.Height);

            var bmp = GetGameWindow(processName);
            if (bmp == null) { return null; }

            var bmpRegion = new Bitmap(region.Width, region.Height);
            using (Graphics g = Graphics.FromImage(bmpRegion))
            {
                g.DrawImage(bmp, destRectangle, region, GraphicsUnit.Pixel);
            }
            return bmpRegion;
        }

        #region P/Invoke

        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);
        
        [DllImport("user32.dll")]
        private static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern Boolean GetWindowRect(HandleRef hWnd, out UnmanagedRectangle lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct UnmanagedRectangle
        {
            /// <summary>
            /// X position of upper-left corner
            /// </summary>
            public int Left;
            
            /// <summary>
            /// Y position of upper-left corner
            /// </summary>
            public int Top;
            
            /// <summary>
            /// X position of lower-right corner
            /// </summary>
            public int Right;

            /// <summary>
            /// Y position of lower-right corner
            /// </summary>
            public int Bottom;
        }

        #endregion
    }
}
