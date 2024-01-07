# MouseKeyboardController

A class for simulate keyboard and mouse events in Windows system.

## Getters

### *byte Array* PressedKeys:

An array of currently pressed virtual keys (if the command to press the key but not release it was triggered) - key codes.

### *MouseButton Array* PressedMouseButtons:

An array of currently pressed virtual mouse buttons (if the command to press the button but not release it was triggered) (*enum items*).

## Methods

### *void* SimulateKeyPress(*byte* keyCode, *KeyState* mode):

Method to simulate key pressing in Windows system.

Parameters:
- *byte* **keyCode** - Key code (available in *SystemController.MouseKeyboard.Data.Keys*).
- *KeyState* **mode** - Event type key (press/release/PressRelease as default) (*enum item*).

### *void* SimulateKeyCombination(params *byte[]* keyCodes):

Method to simulate entering key combination in Windows system.

Parameters:
- *byte Array* **keyCodes** - Array of key codes to press (available in *SystemController.MouseKeyboard.Data.Keys*).

### *POINT* GetMousePosition():

Method for getting actual cursor position on screen.

Returns:
- *POINT* (*custom POINT struct*).

### *void* SimulateMouseClick(*MouseButton* mouseButton, *KeyState* mode):

Method to simulate mouse button pressing in Windows system.

Parameters:
- *MouseButton* **keyCode** - Mouse button (*enum item*).
- *KeyState* **mode** - Event type key (press/release/PressRelease as default) (*enum item*).

### *void* SimulateMouseMove(*int* x, *int* y):

Method to simulate mouse move on screen.

Parameters:
- *int* **x** - X axis curosr position.
- *int* **y** - Y axis cursor position.

### *void* SimulateHorizontalMouseScroll(*int* delta):

Method to simulate horizontal mouse scroll in Windows system.

Parameters:
- *int* **delta** - Scroll delta.

### *void* SimulateVerticalMouseScroll(*int* delta):

Method to simulate vertical mouse scroll in Windows system.

Parameters:
- *int* **delta** - Scroll delta.
