# ScreenManager

A class for getting list of screens attached to computer.

## Methods

### *Array of ScreenInfo* GetAllScreens()

Get array of screen information objects.

### *ScreenInfo* GetMainScreen()

Get main screen information object.

### *int* GetScreenCount()

Get number of screens attached to computer.

### *Dictionary if ScreenInfo, Rectangle* GetSelectedRegions(*Rectangle* selection)

Get dictionary of ScreenInfo and Rectangle, corresponding to the selection of an area in specific screens.

Parameters:
- *Rectangle* **selection** - Selected area as Rectangle (X, Y, Width, Height).
