# MouseReader

A class for listening for mouse events.

## Getters & Setter

### *bool* IsListening { get; }:

Returns whether MouseReader is actively listening for mouse events.

### *int* HeldButtons { get; }:

Returns information about the number of actively pressed mouse buttons.

## Methods

### *List of Exception* GetThrownExceptions():

Returns a list of thrown exceptions from the last listener run.

### *void* StartListening():

Starts listening for mouse events.

### *void* StopListening():

Stops listening for mouse events.

## Events

### MouseClickEventHandler:

An event triggered when mouse button usage is detected.

Event parameters:
- *object* - The object that triggered the event (this).
- *MouseClickEventArgs* - Event arguments:
  - *MouseButton* **Button** - Pressed button (*enum item*).
  - *int* **HeldButtons** - The number of actively pressed mouse buttons.
  - *KeyState* **State** - Button state (pressed/released) (*enum item*).

### MouseMoveEventHandler:

An event triggered when mouse move is detected.

Event parameters:
- *object* - The object that triggered the event (this).
- *MouseMoveEventArgs* - Event arguments:
  - *POINT* **CurrentPosition** - Current cursor position on screen (*custom POINT struct*).
  - *POINT* **PreviousPosition** - Previous cursor position on screen (*custom POINT struct*).

### MouseScrollEventHandler:

An event triggered when mouse scroll is detected.

Event parameters:
- *object* - The object that triggered the event (this).
- *MouseScrollEventArgs* - Event arguments:
  - *int* **Delta** - Scroll delta.
  - *ScrollOrientation* **Orientation** - Scroll orientation (Horizontal/Vertical) (*enum item*).

### MouseListeningFinishedEventHandler:

An event dispatched when listening for mouse events ends.

Event parameters:
- *object* - The object that triggered the event (this).
- *MouseListeningFinishedEventArgs* - Event arguments:
  - *int* **HeldButtons** - The number of actively pressed mouse buttons.
  - *POINT* **LastCursorPosition** - The last cursor position before listening ends (*custom POINT struct*).
  - *List of Exception* **ThrownExceptions** - List of thrown exceptions by MouseReader.

### ThrowExceptionEventHandler:

An event triggered when an exception is thrown by MouseReader.

Event parameter:
- *object* - The object that triggered the event (this).
- *ThrownExceptionEventArgs* - Event arguments:
  - *List of Exception* **ThrownExceptions** - List of thrown exceptions by MouseReader.
