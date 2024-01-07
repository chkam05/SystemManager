# KeyboardReader

A class for listening for keyboard events.

### *bool* IsListening { get; }:

Returns whether KeyboardReader is actively listening for keyboard events.

### *int* HeldKeys { get; }:

Returns information about the number of actively pressed keys.

## Methods

### *List of Exception* GetThrownExceptions():

Returns a list of thrown exceptions from the last listener run.

### *void* StartListening():

Starts listening for keyboard events.

### *void* StopListening():

Stops listening for keyboard events.

## Events

### KeyPressedEventHandler:

An event triggered when key usage is detected.

Event parameters:
- *object* - The object that triggered the event (this).
- *KeyPressEventArgs* - Event arguments:
  - *byte* **KeyCode** - Used key code.
  - *KeyState* **KeyState** - Key state (pressed/released) (*enum item*).
  - *byte Array* **HeldKeyCodes** - Array of currently pressed keys codes.
  - *int* **HeldKeys** - The number of actively pressed keys.

### KeysListeningFinishedEventHandler:

An event dispatched when listening for keyboard events ends.

Event parameters:
- *object* - The object that triggered the event (this).
- *KeysListeningFinishedEventArgs* - Event arguments:
  - *int* **HeldButtons** - The number of actively pressed keys.
  - *List of Exception* **ThrownExceptions** - List of thrown exceptions by KeyboardReader.

### ThrowExceptionEventHandler:

An event triggered when an exception is thrown by KeyboardReader.

Event parameter:
- *object* - The object that triggered the event (this).
- *ThrownExceptionEventArgs* - Event arguments:
  - *List of Exception* **ThrownExceptions** - List of thrown exceptions by KeyboardReader.
