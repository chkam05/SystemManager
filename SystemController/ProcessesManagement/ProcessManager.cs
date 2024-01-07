using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SystemController.Data;
using SystemController.ProcessesManagement.Data;

namespace SystemController.ProcessesManagement
{
    public class ProcessManager
    {

        //  CONST

        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_RESTORE = 9;
        private const uint SWP_SHOWWINDOW = 0x40;


        //  DELEGATES

        private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);


        //  IMPORTS

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessManager class constructor. </summary>
        public ProcessManager()
        {
            //
        }

        #endregion CLASS METHODS

        #region PROCESS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Get list of processes. </summary>
        /// <returns> List of processes. </returns>
        public List<ProcessInfo> GetProcesses()
        {
            List<ProcessInfo> processes = new List<ProcessInfo>();

            foreach (Process process in Process.GetProcesses())
            {
                ProcessInfo processInfo = new ProcessInfo()
                {
                    Id = process.Id,
                    Name = process.ProcessName,
                    Description = process.MainWindowTitle,
                    Type = process.MainWindowHandle == IntPtr.Zero ? "Background Process" : "Application",
                    CommandLocation = process.MainModule?.FileName,
                    Windows = GetWindows(process)
                };

                processes.Add(processInfo);
            }

            return processes;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Kill process. </summary>
        /// <param name="processInfo"> Process information. </param>
        /// <param name="force"> Force kill process. </param>
        public static void KillProcess(ProcessInfo processInfo, bool force = false)
        {
            Process process = Process.GetProcessById(processInfo.Id);

            if (force)
                process.Kill();

            else
            {
                process.CloseMainWindow();
                process.WaitForExit();
            }
        }

        #endregion PROCESS METHODS

        #region WINDOWS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Get window information. </summary>
        /// <param name="hWnd"> Window handle pointer. </param>
        /// <returns> Window information. </returns>
        private static WindowInfo GetWindowInfo(IntPtr hWnd)
        {
            WindowInfo windowInfo = new WindowInfo
            {
                Handle = hWnd,
                Position = GetWindowPosition(hWnd),
                Size = GetWindowSize(hWnd),
                State = GetWindowState(hWnd),
                Title = GetWindowTitle(hWnd),
                Visible = GetWindowVisibility(hWnd)
            };

            return windowInfo;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get window position. </summary>
        /// <param name="hWnd"> Window handle pointer. </param>
        /// <returns> Window position. </returns>
        private static POINT GetWindowPosition(IntPtr hWnd)
        {
            RECT rect;
            GetWindowRect(hWnd, out rect);
            
            return new POINT
            {
                X = rect.Left,
                Y = rect.Top
            };
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get window size. </summary>
        /// <param name="hWnd"> Window handle pointer. </param>
        /// <returns> Window size. </returns>
        private static SIZE GetWindowSize(IntPtr hWnd)
        {
            RECT rect;
            GetWindowRect(hWnd, out rect);

            return new SIZE
            {
                Width = rect.Right - rect.Left,
                Height = rect.Bottom - rect.Top
            };
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get window state. </summary>
        /// <param name="hWnd"> Window handle pointer. </param>
        /// <returns> Window state. </returns>
        private static WindowState GetWindowState(IntPtr hWnd)
        {
            if (IsIconic(hWnd))
                return WindowState.Minimized;

            else if (IsZoomed(hWnd))
                return WindowState.Maximized;

            else
                return WindowState.Normal;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get window title. </summary>
        /// <param name="hWnd"> Window handle pointer. </param>
        /// <returns> Window title. </returns>
        private static string GetWindowTitle(IntPtr hWnd)
        {
            const int nChars = 256;
            var stringBuilder = new StringBuilder();
            GetWindowText(hWnd, stringBuilder, nChars);
            return stringBuilder.ToString();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get window visibility. </summary>
        /// <param name="hWnd"> Window handle pointer. </param>
        /// <returns> True - window is visible; False - otherwise. </returns>
        private static bool GetWindowVisibility(IntPtr hWnd)
        {
            return IsWindowVisible(hWnd) ? true : false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process windows. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> List of process windows. </returns>
        public List<WindowInfo> GetWindows(Process process)
        {
            List<WindowInfo> windows = new List<WindowInfo>();

            IntPtr mainWindowHandle = process.MainWindowHandle;
            if (mainWindowHandle != IntPtr.Zero)
            {
                windows.Add(GetWindowInfo(mainWindowHandle));
            }

            foreach (ProcessThread thread in process.Threads)
            {
                EnumThreadWindows(thread.Id, (hWnd, lParam) =>
                {
                    windows.Add(GetWindowInfo(hWnd));
                    return true;
                }, IntPtr.Zero);
            }

            return windows;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Close window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public static void CloseWindow(WindowInfo windowInfo)
        {
            CloseWindow(windowInfo.Handle);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Focus window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public static void FocusWindow(WindowInfo windowInfo)
        {
            if (windowInfo.State == WindowState.Minimized)
                ShowWindowAsync(windowInfo.Handle, SW_RESTORE);

            SetForegroundWindow(windowInfo.Handle);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Maximize window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public static void MaximizeWindow(WindowInfo windowInfo)
        {
            ShowWindow(windowInfo.Handle, SW_SHOWMAXIMIZED);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Minimize window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public static void MinimizeWindow(WindowInfo windowInfo)
        {
            ShowWindow(windowInfo.Handle, SW_SHOWMINIMIZED);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Resize window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        /// <param name="newSize"> New window size. </param>
        public static void ResizeWindow(WindowInfo windowInfo, SIZE newSize)
        {
            SetWindowPos(windowInfo.Handle, IntPtr.Zero, windowInfo.Position.X, windowInfo.Position.Y,
                newSize.Width, newSize.Height, SWP_SHOWWINDOW);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Move window on screen. </summary>
        /// <param name="windowInfo"> Window information. </param>
        /// <param name="newPosition"> New window position. </param>
        public static void MoveWindow(WindowInfo windowInfo, POINT newPosition)
        {
            SetWindowPos(windowInfo.Handle, IntPtr.Zero, newPosition.X, newPosition.Y,
                windowInfo.Size.Width, windowInfo.Size.Height, SWP_SHOWWINDOW);
        }

        #endregion WINDOWS METHODS

    }
}
