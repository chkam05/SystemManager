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
        private const uint GW_HWNDNEXT = 2;
        private const int GW_OWNER = 4;
        private const uint GW_CHILD = 5;
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

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

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
        public void KillProcess(ProcessInfo processInfo, bool force = false)
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
        private WindowInfo GetWindowInfo(IntPtr hWnd)
        {
            WindowInfo windowInfo = new WindowInfo
            {
                Handle = hWnd,
                ClassName = GetWindowClassName(hWnd),
                Position = GetWindowPosition(hWnd),
                Size = GetWindowSize(hWnd),
                State = GetWindowState(hWnd),
                Title = GetWindowTitle(hWnd),
                Visible = GetWindowVisibility(hWnd)
            };

            return windowInfo;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get window class name. </summary>
        /// <param name="hWnd"> Window handle pointer. </param>
        /// <returns> Window class name. </returns>
        public string GetWindowClassName(IntPtr hWnd)
        {
            StringBuilder className = new StringBuilder(256);
            GetClassName(hWnd, className, className.Capacity);
            return className.ToString();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get window position. </summary>
        /// <param name="hWnd"> Window handle pointer. </param>
        /// <returns> Window position. </returns>
        private POINT GetWindowPosition(IntPtr hWnd)
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
        private SIZE GetWindowSize(IntPtr hWnd)
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
        private WindowState GetWindowState(IntPtr hWnd)
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
        private string GetWindowTitle(IntPtr hWnd)
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
        private bool GetWindowVisibility(IntPtr hWnd)
        {
            return IsWindowVisible(hWnd) ? true : false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process windows. </summary>
        /// <param name="process"> Process information. </param>
        /// <returns> List of windows. </returns>
        public List<WindowInfo> GetWindows(ProcessInfo processInfo)
        {
            Process process = Process.GetProcessById(processInfo.Id);

            if (process != null)
                return GetWindows(process);

            return new List<WindowInfo>();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process windows. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> List of windows. </returns>
        public List<WindowInfo> GetWindows(Process process)
        {
            List<WindowInfo> windows = new List<WindowInfo>();
            IntPtr mainWindowHandle = process.MainWindowHandle;

            if (mainWindowHandle != IntPtr.Zero)
                windows.Add(GetWindowInfo(mainWindowHandle));

            foreach (ProcessThread thread in process.Threads)
            {
                EnumThreadWindows(thread.Id, (hWnd, lParam) =>
                {
                    windows.Add(GetWindowInfo(hWnd));
                    return true;
                },
                IntPtr.Zero);
            }

            foreach (WindowInfo window in windows)
            {
                IntPtr parentHandle = GetParent(window.Handle);

                if (parentHandle != IntPtr.Zero)
                    window.ParentWindow = GetWindowInfo(parentHandle);

                List<WindowInfo> childWindows = new List<WindowInfo>();
                IntPtr childHandle = GetWindow(window.Handle, GW_CHILD);

                while (childHandle != IntPtr.Zero)
                {
                    childWindows.Add(GetWindowInfo(childHandle));
                    childHandle = GetWindow(childHandle, GW_HWNDNEXT);
                }

                window.ChildWindows = childWindows;
            }

            return windows;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Close window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public void CloseWindow(WindowInfo windowInfo)
        {
            CloseWindow(windowInfo.Handle);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Focus window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public void FocusWindow(WindowInfo windowInfo)
        {
            if (windowInfo.State == WindowState.Minimized)
                ShowWindowAsync(windowInfo.Handle, SW_RESTORE);

            SetForegroundWindow(windowInfo.Handle);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Maximize window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public void MaximizeWindow(WindowInfo windowInfo)
        {
            ShowWindow(windowInfo.Handle, SW_SHOWMAXIMIZED);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Minimize window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public void MinimizeWindow(WindowInfo windowInfo)
        {
            ShowWindow(windowInfo.Handle, SW_SHOWMINIMIZED);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Resize window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        /// <param name="newSize"> New window size. </param>
        public void ResizeWindow(WindowInfo windowInfo, SIZE newSize)
        {
            SetWindowPos(windowInfo.Handle, IntPtr.Zero, windowInfo.Position.X, windowInfo.Position.Y,
                newSize.Width, newSize.Height, SWP_SHOWWINDOW);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Move window on screen. </summary>
        /// <param name="windowInfo"> Window information. </param>
        /// <param name="newPosition"> New window position. </param>
        public void MoveWindow(WindowInfo windowInfo, POINT newPosition)
        {
            SetWindowPos(windowInfo.Handle, IntPtr.Zero, newPosition.X, newPosition.Y,
                windowInfo.Size.Width, windowInfo.Size.Height, SWP_SHOWWINDOW);
        }

        #endregion WINDOWS METHODS

    }
}
