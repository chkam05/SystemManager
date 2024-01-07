using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
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
        private const int GWL_EXSTYLE = -20;
        private const int GWL_STYLE = -16;
        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_DISABLED = 0x8000000;
        private const int WS_SYSMENU = 0x80000;
        private const int WS_DLGFRAME = 0x00400000;
        private const int WS_TOOLWINDOW = 0x00000080;
        private const int SW_RESTORE = 9;
        private const uint SWP_SHOWWINDOW = 0x40;


        //  DELEGATES

        private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);


        //  IMPORTS

        //  advapi32

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ConvertSidToStringSid(IntPtr sid, out IntPtr stringSid);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetTokenInformation(IntPtr tokenHandle, TokenInformationClass tokenInformationClass, IntPtr tokenInformation, int tokenInformationLength, out int returnLength);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenProcessToken(IntPtr processHandle, TokenAccessLevels desiredAccess, out IntPtr tokenHandle);

        //  kerner32

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcessHeap();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetProcessTimes(IntPtr hProcess, out FILETIME lpCreationTime, out FILETIME lpExitTime, out FILETIME lpKernelTime, out FILETIME lpUserTime);

        [DllImport("kernel32.dll")]
        private static extern IntPtr HeapAlloc(IntPtr hHeap, uint dwFlags, IntPtr dwBytes);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr processHandle, [Out] out bool wow64Process);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        //  psapi

        [DllImport("psapi.dll", SetLastError = true)]
        private static extern bool GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS_EX counters, uint size);

        //  user32

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool GetLayeredWindowAttributes(IntPtr hwnd, out uint crKey, out byte bAlpha, out uint dwFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowEnabled(IntPtr hWnd);

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
        /// <summary> Get process CPU usage. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> CPU usage. </returns>
        private double GetProcessCPUUsage(Process process)
        {
            TimeSpan totalProcessorTime = process.TotalProcessorTime;
            TimeSpan uptime = process.StartTime - DateTime.Now;

            if (uptime.TotalSeconds > 0)
                return (double)(totalProcessorTime.TotalMilliseconds / uptime.TotalMilliseconds) * 100.0;

            return 0.0;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process memory usage. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> Process memory usage. </returns>
        private long GetProcessMemoryUsage(Process process)
        {
            PROCESS_MEMORY_COUNTERS_EX counters = new PROCESS_MEMORY_COUNTERS_EX();

            if (GetProcessMemoryInfo(process.Handle, out counters, (uint)Marshal.SizeOf(typeof(PROCESS_MEMORY_COUNTERS_EX))))
                return counters.PrivateUsage;

            return 0;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process mode. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> Process mode. </returns>
        private ProcessMode GetProcessMode(Process process)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                bool isWow64;

                if (IsWow64Process(process.Handle, out isWow64) && isWow64)
                    return ProcessMode.Bit32;

                else
                    return ProcessMode.Bit64;
            }

            return ProcessMode.Bit32;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process uptime. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> Process uptime. </returns>
        private TimeSpan GetProcessUptime(Process process)
        {
            FILETIME creationTime, exitTime, kernelTime, userTime;

            if (GetProcessTimes(process.Handle, out creationTime, out exitTime, out kernelTime, out userTime))
            {
                long ticks = userTime.dwHighDateTime << 32 | (uint)userTime.dwLowDateTime;
                return TimeSpan.FromTicks(ticks);
            }

            return TimeSpan.Zero;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process username. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> Process username. </returns>
        private string GetProcessUserName(Process process)
        {
            IntPtr processHandle = IntPtr.Zero;

            try
            {
                processHandle = OpenProcess(ProcessAccessFlags.QueryInformation, false, process.Id);

                if (processHandle != IntPtr.Zero)
                {
                    int size = 0;
                    GetTokenInformation(processHandle, TokenInformationClass.TokenUser, IntPtr.Zero, 0, out size);

                    IntPtr tokenInformation = Marshal.AllocHGlobal(size);
                    try
                    {
                        if (GetTokenInformation(processHandle, TokenInformationClass.TokenUser, tokenInformation, size, out size))
                        {
                            TOKEN_USER tokenUser = (TOKEN_USER)Marshal.PtrToStructure(tokenInformation, typeof(TOKEN_USER));

                            IntPtr userSid = tokenUser.User.Sid;
                            IntPtr userNamePtr;

                            if (ConvertSidToStringSid(userSid, out userNamePtr))
                            {
                                string userName = Marshal.PtrToStringAuto(userNamePtr);
                                return userName;
                            }
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(tokenInformation);
                    }
                }
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                    CloseHandle(processHandle);
            }

            return "Unknown User";
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if process has any window. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> True - process has any window; False - otherwise. </returns>
        private bool HasWindows(Process process)
        {
            bool anyWindow = false;
            IntPtr mainWindowHandle = process.MainWindowHandle;

            if (mainWindowHandle != IntPtr.Zero)
                return true;

            foreach (ProcessThread thread in process.Threads)
            {
                EnumThreadWindows(thread.Id, (hWnd, lParam) =>
                {
                    anyWindow = true;
                    return true;
                },
                IntPtr.Zero);
            }

            return anyWindow;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Is process system process. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> True - is system process; False - otherwise. </returns>
        private bool IsSystemProcess(Process process, string processUserName)
        {
            bool hasSystemCharacteristics = process.PriorityClass == ProcessPriorityClass.High
                && process.Id <= 4;

            bool isSystemAccount = processUserName.Equals("SYSTEM", StringComparison.OrdinalIgnoreCase);

            return hasSystemCharacteristics || isSystemAccount;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get list of processes. </summary>
        /// <returns> List of processes. </returns>
        public List<ProcessInfo> GetProcesses()
        {
            List<ProcessInfo> processes = new List<ProcessInfo>();

            foreach (Process process in Process.GetProcesses())
            {
                var processUserName = GetProcessUserName(process);

                ProcessInfo processInfo = new ProcessInfo()
                {
                    Id = process.Id,
                    Name = process.ProcessName,
                    Description = process.MainWindowTitle,
                    Type = process.MainWindowHandle == IntPtr.Zero ? "Background Process" : "Application",
                    CommandLocation = process.MainModule?.FileName,
                    CPUUsage = GetProcessCPUUsage(process),
                    MemoryUsage = GetProcessMemoryUsage(process),
                    HasWindows = HasWindows(process),
                    IsSystemService = IsSystemProcess(process, processUserName),
                    Mode = GetProcessMode(process),
                    Priority = ProcessPriorityClass.Normal,
                    ThreadCount = process.Threads.Count,
                    Uptime = GetProcessUptime(process),
                    UserName = processUserName,
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
                Attributes = GetWindowAttributes(hWnd),
                Position = GetWindowPosition(hWnd),
                Role = GetWindowRole(hWnd),
                Size = GetWindowSize(hWnd),
                State = GetWindowState(hWnd),
                Title = GetWindowTitle(hWnd),
                Transparency = GetWindowTransparency(hWnd),
                Visible = GetWindowVisibility(hWnd)
            };

            return windowInfo;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get window attributes. </summary>
        /// <param name="hWnd"> Window handle pointer. </param>
        /// <returns> Window attributes. </returns>
        private WindowAttributes GetWindowAttributes(IntPtr hWnd)
        {
            uint exStyle = (uint)GetWindowLong(hWnd, GWL_EXSTYLE);

            if ((exStyle & WS_EX_LAYERED) != 0)
                return WindowAttributes.Visible;

            else if ((exStyle & WS_EX_DISABLED) != 0)
                return WindowAttributes.Disabled;

            else
                return WindowAttributes.None;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get window class name. </summary>
        /// <param name="hWnd"> Window handle pointer. </param>
        /// <returns> Window class name. </returns>
        private string GetWindowClassName(IntPtr hWnd)
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
        /// <summary> Get window role. </summary>
        /// <param name="hWnd"> Window handle pointer. </param>
        /// <returns> Window role. </returns>
        private WindowRole GetWindowRole(IntPtr hWnd)
        {
            uint windowStyle = (uint)GetWindowLong(hWnd, GWL_STYLE);

            if ((windowStyle & WS_SYSMENU) != 0)
                return WindowRole.Main;

            else if ((windowStyle & WS_DLGFRAME) != 0)
                return WindowRole.Dialog;

            else if ((windowStyle & WS_TOOLWINDOW) != 0)
                return WindowRole.Tool;

            else
                return WindowRole.Other;
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
        /// <summary> Get window transparency. </summary>
        /// <param name="hWnd">Window handle pointer. </param>
        /// <returns> Window transparency. </returns>
        private int GetWindowTransparency(IntPtr hWnd)
        {
            uint crKey;
            byte bAlpha;
            uint dwFlags;

            if (GetLayeredWindowAttributes(hWnd, out crKey, out bAlpha, out dwFlags))
                return bAlpha;

            return 0;
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
