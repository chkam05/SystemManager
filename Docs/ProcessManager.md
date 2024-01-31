# ProcessesManager

A class for listening running processes and windows attached to them.

## Methods

### *List of ProcessInfo* GetProcesses(out *List of Exception* exceptions):

Returns a list of running processes, and as parameter, list of thrown exceptions when process could not be read.

Parameters:
- out *List of Exception* **exceptions** - List of thrown exceptions when process could not be read.

### *bool* IsProcessAlive(*ProcessInfo* processInfo, out *Process?* process)

Returns information about whether the process is alive, and as a parameter, the underlying Process object.

Parameters:
- *ProcessInfo* **processInfo** - Process information container (available in *SystemController.Processes.Data.ProcessInfo*).
- out *Process?* **process** - Underlying Process object.

### *ProcessActionResult* KillProcess(*ProcessInfo* processInfo, *bool* force = false)

Method for kill process.

Parameters:
- *ProcessInfo* **processInfo** - Process information container (available in *SystemController.Processes.Data.ProcessInfo*).
- *bool* **force** - True: Kill the process, False: invoke classic shutdown.

### *List of WindowInfo* GetWindows(*ProcessInfo* processInfo)

Returns a list of windows, attached to process.

Parameters:
- *ProcessInfo* **processInfo** - Process information container (available in *SystemController.Processes.Data.ProcessInfo*).

### *bool* IsWindowAlive(*WindowInfo* windowInfo, out *IntPtr* windowHandle, out *Process?* process)

Returns information about whether the window exists, and as a parameter, the underlying Process object and Window handle.

Parameters:
- *WindowInfo* **windowInfo** - Window information container (available in *SystemController.Processes.Data.WindowInfo*).
- out *IntPtr* **windowHandle** - Window handle (memory address).
- out *Process?* **process** - Underlying Process object.

### *WindowActionResult* CloseWindow(*WindowInfo* windowInfo)

Close window.

Parameters:
- *WindowInfo* **windowInfo** - Window information container (available in *SystemController.Processes.Data.WindowInfo*).

### *WindowActionResult* FocusWindow(*WindowInfo* windowInfo)

Bring window to front.

Parameters:
- *WindowInfo* **windowInfo** - Window information container (available in *SystemController.Processes.Data.WindowInfo*).

### *WindowActionResult* MaximizeWindow(*WindowInfo* windowInfo)

Maximize window.

Parameters:
- *WindowInfo* **windowInfo** - Window information container (available in *SystemController.Processes.Data.WindowInfo*).

### *WindowActionResult* MinimizeWindow(*WindowInfo* windowInfo)

Minimize window.

Parameters:
- *WindowInfo* **windowInfo** - Window information container (available in *SystemController.Processes.Data.WindowInfo*).

### *WindowActionResult* RestoreWindow(*WindowInfo* windowInfo)

Restore minimized window.

Parameters:
- *WindowInfo* **windowInfo** - Window information container (available in *SystemController.Processes.Data.WindowInfo*).

### *WindowActionResult* ResizeWindow(*WindowInfo* windowInfo, *SIZE* newSize)

Resize window.

Parameters:
- *WindowInfo* **windowInfo** - Window information container (available in *SystemController.Processes.Data.WindowInfo*).
- *SIZE* **newSize** - New window size.

### *WindowActionResult* MoveWindow(*WindowInfo* windowInfo, *POINT* newPosition)

Move window.

Parameters:
- *WindowInfo* **windowInfo** - Window information container (available in *SystemController.Processes.Data.WindowInfo*).
- *POINT* **newPosition** - New window position.
