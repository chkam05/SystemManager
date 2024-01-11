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

        private const int GWL_EXSTYLE = -20;
        private const int GW_OWNER = 4;
        private const int GWL_STYLE = -16;
        private const uint GW_CHILD = 5;
        private const uint GW_HWNDNEXT = 2;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_RESTORE = 9;
        private const uint SWP_SHOWWINDOW = 0x40;
        private const int WS_EX_DISABLED = 0x8000000;
        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_DLGFRAME = 0x00400000;
        private const int WS_TOOLWINDOW = 0x00000080;
        private const int WS_SYSMENU = 0x80000;

        private const int NameSamCompatible = 2;


        //  DELEGATES

        private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);


        #region IMPORTS

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

        //  secur32

        [DllImport("secur32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetUserNameEx(int nameFormat, StringBuilder userName, ref uint userNameSize);

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

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpwdProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindow(IntPtr hWnd);

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

        #endregion IMPORTS


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
        /// <summary> Get process command location. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> Process command location or null. </returns>
        private string? GetProcessCommandLocation(Process process)
        {
            try
            {
                if (process.MainModule != null && process.MainModule.FileName != null)
                {
                    return process.MainModule.FileName;
                }
                else
                {
                    return null;
                }
            }
            catch (Win32Exception)
            {
                return null;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process CPU usage. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> CPU usage. </returns>
        private double? GetProcessCPUUsage(Process process)
        {
            try
            {
                TimeSpan totalProcessorTime = process.TotalProcessorTime;
                TimeSpan uptime = process.StartTime - DateTime.Now;

                if (uptime.TotalSeconds > 0)
                    return (double)(totalProcessorTime.TotalMilliseconds / uptime.TotalMilliseconds) * 100.0;
            }
            catch (Win32Exception)
            {
                return null;
            }

            return null;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process memory usage. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> Process memory usage. </returns>
        private long? GetProcessMemoryUsage(Process process)
        {
            PROCESS_MEMORY_COUNTERS_EX counters = new PROCESS_MEMORY_COUNTERS_EX();

            try
            {
                if (GetProcessMemoryInfo(process.Handle, out counters, (uint)Marshal.SizeOf(typeof(PROCESS_MEMORY_COUNTERS_EX))))
                    return counters.PrivateUsage;
            }
            catch (Win32Exception)
            {
                return null;
            }

            return null;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process mode. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> Process mode. </returns>
        private ProcessMode GetProcessMode(Process process)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                try
                {
                    bool isWow64;

                    if (IsWow64Process(process.Handle, out isWow64) && isWow64)
                        return ProcessMode.Bit32;
                }
                catch (Win32Exception)
                {
                    //
                }

                return ProcessMode.Bit64;
            }

            return ProcessMode.Bit32;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process priority class. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> Process priority class. </returns>
        private ProcessPriorityClass? GetProcesssPriorityClass(Process process)
        {
            try
            {
                if (process?.PriorityClass != null)
                {
                    return process.PriorityClass;
                }
                else
                {
                    return null;
                }
            }
            catch (Win32Exception)
            {
                return null;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process uptime. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> Process uptime. </returns>
        private TimeSpan? GetProcessUptime(Process process)
        {
            FILETIME creationTime, exitTime, kernelTime, userTime;

            try
            {
                if (GetProcessTimes(process.Handle, out creationTime, out exitTime, out kernelTime, out userTime))
                {
                    long ticks = userTime.dwHighDateTime << 32 | (uint)userTime.dwLowDateTime;
                    return TimeSpan.FromTicks(ticks);
                }
            }
            catch (Win32Exception)
            {
                return null;
            }

            return null;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process username. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> Process username. </returns>
        private string? GetProcessUserName(Process process)
        {
            IntPtr processHandle = IntPtr.Zero;

            try
            {
                processHandle = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, process.Id);

                if (processHandle != IntPtr.Zero)
                {
                    uint length = 200;

                    StringBuilder sb = new StringBuilder((int)length);

                    if (GetUserNameEx(NameSamCompatible, sb, ref length))
                    {
                        return sb.ToString();
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                    CloseHandle(processHandle);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if process has available handle. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> True - process has handle; False - otherwise. </returns>
        private bool HasProcessHandle(Process process)
        {
            try
            {
                if (process.Handle != IntPtr.Zero)
                    return true;
            }
            catch (Win32Exception)
            {
                return false;
            }

            return false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if process has available main module. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> True - process has main moudle; False - otherwise. </returns>
        private bool HasProcessMainModule(Process process)
        {
            try
            {
                if (process.MainModule != null)
                    return true;
            }
            catch (Win32Exception)
            {
                return false;
            }

            return false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if process has main window. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> True - process has main window; False - otherwise. </returns>
        private bool HasProcessMainWindow(Process process)
        {
            return process.MainWindowHandle != IntPtr.Zero;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if process has available priority class. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> True - process has priority class; False - otherwise. </returns>
        private bool HasProcessPriorityClass(Process process)
        {
            try
            {
                ProcessPriorityClass? processPriorityClass = process.PriorityClass;

                if (processPriorityClass != null)
                    return true;
            }
            catch (Win32Exception)
            {
                return false;
            }

            return false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if process has any subwindows. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> True - process has any subwindows; False - otherwise. </returns>
        private bool HasProcessSubWindows(Process process)
        {
            bool anyWindow = false;

            /*foreach (ProcessThread thread in process.Threads)
            {
                EnumThreadWindows(thread.Id, (hWnd, lParam) =>
                {
                    anyWindow = true;
                    return true;
                },
                IntPtr.Zero);
            }*/

            return anyWindow;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if process has available total processor time. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> True - process has total processor time; False - otherwise. </returns>
        private bool HasProcessTotalProcessorTime(Process process)
        {
            try
            {
                if (process.TotalProcessorTime >= TimeSpan.Zero)
                    return true;
            }
            catch (Win32Exception)
            {
                return false;
            }

            return false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Is process system process. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> True - is system process; False - otherwise. </returns>
        private bool IsSystemProcess(Process process, string? processUserName)
        {
            try
            {
                bool hasSystemCharacteristics = process.PriorityClass == ProcessPriorityClass.High
                    && process.Id <= 4;

                bool isSystemAccount = processUserName?.Equals("SYSTEM", StringComparison.OrdinalIgnoreCase) ?? false;

                return hasSystemCharacteristics || isSystemAccount;
            }
            catch (Win32Exception)
            {
                return false;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get list of processes. </summary>
        /// <returns> List of processes. </returns>
        public List<ProcessInfo> GetProcesses(out List<Exception> exceptions)
        {
            exceptions = new List<Exception>();
            List<ProcessInfo> processes = new List<ProcessInfo>();

            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    bool hasHandle = HasProcessHandle(process);
                    bool hasMainModule = HasProcessMainModule(process);
                    bool hasPriorityClass = HasProcessPriorityClass(process);
                    bool hasTotalProcTime = HasProcessTotalProcessorTime(process);

                    var processUserName = GetProcessUserName(process);

                    ProcessInfo processInfo = new ProcessInfo()
                    {
                        Id = process.Id,
                        Name = process.ProcessName,
                        Description = process.MainWindowTitle,
                        Type = HasProcessMainWindow(process) ? "Application" : "Background Process",
                        HasWindows = HasProcessMainWindow(process) ? true : HasProcessSubWindows(process),
                        ThreadCount = process.Threads.Count,
                        UserName = processUserName,
                    };

                    if (hasHandle)
                    {
                        processInfo.MemoryUsage = GetProcessMemoryUsage(process);
                        processInfo.Mode = GetProcessMode(process);
                        processInfo.Uptime = GetProcessUptime(process);
                    }

                    if (hasMainModule)
                    {
                        processInfo.CommandLocation = GetProcessCommandLocation(process);
                    }

                    if (hasPriorityClass)
                    {
                        processInfo.IsSystemService = IsSystemProcess(process, processUserName);
                        processInfo.Priority = GetProcesssPriorityClass(process);
                    }
                    
                    if (hasTotalProcTime)
                    {
                        processInfo.CPUUsage = GetProcessCPUUsage(process);
                    }

                    processes.Add(processInfo);
                }
                catch (Exception exception)
                {
                    exceptions.Add(exception);
                }
            }

            return processes;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if process is alive. </summary>
        /// <param name="processInfo"> Process information. </param>
        /// <param name="process"> Process. </param>
        /// <returns> True - process is alive; False - otherwise. </returns>
        public bool IsProcessAlive(ProcessInfo processInfo, out Process? process)
        {
            try
            {
                process = Process.GetProcessById(processInfo.Id);
                return true;
            }
            catch (Exception)
            {
                process = null;
                return false;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Kill process. </summary>
        /// <param name="processInfo"> Process information. </param>
        /// <param name="force"> Force kill process. </param>
        public ProcessActionResult KillProcess(ProcessInfo processInfo, bool force = false)
        {
            try
            {
                if (!IsProcessAlive(processInfo, out Process? process))
                    return ProcessActionResult.ProcessNotExist;

                if (force)
                {
                    process?.Kill();
                    return ProcessActionResult.Success;
                }

                process?.CloseMainWindow();
                process?.WaitForExit();
                return ProcessActionResult.Success;
            }
            catch (Exception)
            {
                return ProcessActionResult.UnknownError;
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
            try
            {
                Process process = Process.GetProcessById(processInfo.Id);

                if (process != null)
                    return GetWindows(process);

                return new List<WindowInfo>();
            }
            catch (Exception)
            {
                return new List<WindowInfo>();
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get process windows. </summary>
        /// <param name="process"> Process. </param>
        /// <returns> List of windows. </returns>
        private List<WindowInfo> GetWindows(Process process)
        {
            List<WindowInfo> windows = new List<WindowInfo>();
            IntPtr mainWindowHandle = process.MainWindowHandle;

            if (mainWindowHandle != IntPtr.Zero)
                windows.Add(GetWindowInfo(mainWindowHandle));

            /*foreach (ProcessThread thread in process.Threads)
            {
                EnumThreadWindows(thread.Id, (hWnd, lParam) =>
                {
                    windows.Add(GetWindowInfo(hWnd));
                    return true;
                },
                IntPtr.Zero);
            }*/

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
        /// <summary> Check if window is alive. </summary>
        /// <param name="windowInfo"> Window informations. </param>
        /// <param name="windowHandle"> Window handle. </param>
        /// <param name="process"> Process. </param>
        /// <returns> True - window is alive; False - otherwise. </returns>
        public bool IsWindowAlive(WindowInfo windowInfo, out IntPtr windowHandle, out Process? process)
        {
            process = null;
            windowHandle = IntPtr.Zero;

            if (windowInfo.Handle == IntPtr.Zero)
                return false;

            if (!IsWindow(windowInfo.Handle))
                return false;

            uint processId;
            GetWindowThreadProcessId(windowInfo.Handle, out processId);

            try
            {
                process = Process.GetProcessById((int)processId);
                windowHandle = windowInfo.Handle;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Close window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public WindowActionResult CloseWindow(WindowInfo windowInfo)
        {
            try
            {
                if (!IsWindowAlive(windowInfo, out IntPtr windowHandle, out Process? process))
                {
                    if (process == null)
                        return WindowActionResult.ProcessNotExist;
                    else
                        return WindowActionResult.WindowNotExist;
                }

                CloseWindow(windowHandle);
                return WindowActionResult.Success;
            }
            catch (Exception)
            {
                return WindowActionResult.UnknownError;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Focus window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public WindowActionResult FocusWindow(WindowInfo windowInfo)
        {
            try
            {
                if (!IsWindowAlive(windowInfo, out IntPtr windowHandle, out Process? process))
                {
                    if (process == null)
                        return WindowActionResult.ProcessNotExist;
                    else
                        return WindowActionResult.WindowNotExist;
                }

                if (windowInfo.State == WindowState.Minimized)
                    ShowWindow(windowInfo.Handle, SW_RESTORE);

                SetForegroundWindow(windowInfo.Handle);
                return WindowActionResult.Success;
            }
            catch (Exception)
            {
                return WindowActionResult.UnknownError;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Maximize window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public WindowActionResult MaximizeWindow(WindowInfo windowInfo)
        {
            try
            {
                if (!IsWindowAlive(windowInfo, out IntPtr windowHandle, out Process? process))
                {
                    if (process == null)
                        return WindowActionResult.ProcessNotExist;
                    else
                        return WindowActionResult.WindowNotExist;
                }

                ShowWindow(windowInfo.Handle, SW_SHOWMAXIMIZED);
                return WindowActionResult.Success;
            }
            catch (Exception)
            {
                return WindowActionResult.UnknownError;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Minimize window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public WindowActionResult MinimizeWindow(WindowInfo windowInfo)
        {
            try
            {
                if (!IsWindowAlive(windowInfo, out IntPtr windowHandle, out Process? process))
                {
                    if (process == null)
                        return WindowActionResult.ProcessNotExist;
                    else
                        return WindowActionResult.WindowNotExist;
                }

                ShowWindow(windowInfo.Handle, SW_SHOWMINIMIZED);
                return WindowActionResult.Success;
            }
            catch (Exception)
            {
                return WindowActionResult.UnknownError;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Restore window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public WindowActionResult RestoreWindow(WindowInfo windowInfo)
        {
            try
            {
                if (!IsWindowAlive(windowInfo, out IntPtr windowHandle, out Process? process))
                {
                    if (process == null)
                        return WindowActionResult.ProcessNotExist;
                    else
                        return WindowActionResult.WindowNotExist;
                }

                ShowWindow(windowInfo.Handle, SW_RESTORE);
                return WindowActionResult.Success;
            }
            catch (Exception)
            {
                return WindowActionResult.UnknownError;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Resize window. </summary>
        /// <param name="windowInfo"> Window information. </param>
        /// <param name="newSize"> New window size. </param>
        public WindowActionResult ResizeWindow(WindowInfo windowInfo, SIZE newSize)
        {
            try
            {
                if (!IsWindowAlive(windowInfo, out IntPtr windowHandle, out Process? process))
                {
                    if (process == null)
                        return WindowActionResult.ProcessNotExist;
                    else
                        return WindowActionResult.WindowNotExist;
                }

                SetWindowPos(windowInfo.Handle, IntPtr.Zero, windowInfo.Position.X, windowInfo.Position.Y,
                    newSize.Width, newSize.Height, SWP_SHOWWINDOW);

                return WindowActionResult.Success;
            }
            catch (Exception)
            {
                return WindowActionResult.UnknownError;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Move window on screen. </summary>
        /// <param name="windowInfo"> Window information. </param>
        /// <param name="newPosition"> New window position. </param>
        public WindowActionResult MoveWindow(WindowInfo windowInfo, POINT newPosition)
        {
            try
            {
                if (!IsWindowAlive(windowInfo, out IntPtr windowHandle, out Process? process))
                {
                    if (process == null)
                        return WindowActionResult.ProcessNotExist;
                    else
                        return WindowActionResult.WindowNotExist;
                }

                SetWindowPos(windowInfo.Handle, IntPtr.Zero, newPosition.X, newPosition.Y,
                    windowInfo.Size.Width, windowInfo.Size.Height, SWP_SHOWWINDOW);

                return WindowActionResult.Success;
            }
            catch (Exception)
            {
                return WindowActionResult.UnknownError;
            }
        }

        #endregion WINDOWS METHODS

    }
}
