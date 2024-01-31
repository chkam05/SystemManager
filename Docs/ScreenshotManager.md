# ScreenManager

A class for making screen shots.

## Methods

### *Bitmap* CaptureAllScreens()

Make screenshot from all screens attached to computer.

### *BitmapImage* CaptureAllScreensAsBitmapImage()

Make screenshot from all screens attached to computer as WPF BitmapImage.

### *void* CaptureAllScreensToClipboard()

Make screenshot from all screens attached to computer and copy to clipboard.

### *void* CaptureAllScreensToFile(*string* filePath, *ImageFormat?* imageFormat = null)

Make screenshot from all screens attached to computer and save to file.

Parameters:
- *string* **filePath** - Path to file, where screenshot will be saved.
- *ImageFormat?* **imageFormat** - Image format (default Png).

### *Bitmap* CaptureArea(*Rectangle* selectedArea)

Make screenshot from specified area.

Parameters:
- *Rectangle* **selectedArea** - Selected area as Rectangle (X, Y, Width, Height).

### *Bitmap* CaptureArea(*int* x, *int* y, *int* width, *int* height)

Make screenshot from specified area.

Parameters:
- *int* **x** - Area start position X.
- *int* **y** - Area start position Y.
- *int* **width** - Area width.
- *int* **height** - Area height.

### *BitmapImage* CaptureAreaAsBitmapImage(*Rectangle* selectedArea)

Make screenshot from specified area as WPF BitmapImage.

Parameters:
- *Rectangle* **selectedArea** - Selected area as Rectangle (X, Y, Width, Height).

### *BitmapImage* CaptureAreaAsBitmapImage(*int* x, *int* y, *int* width, *int* height)

Make screenshot from specified area as WPF BitmapImage.

Parameters:
- *int* **x** - Area start position X.
- *int* **y** - Area start position Y.
- *int* **width** - Area width.
- *int* **height** - Area height.

### *void* CaptureAreaToClipboard(*Rectangle* selectedArea)

Make screenshot from specified area and copy to clipboard.

Parameters:
- *Rectangle* **selectedArea** - Selected area as Rectangle (X, Y, Width, Height).

### *void* CaptureAreaToClipboard(*int* x, *int* y, *int* width, *int* height)

Make screenshot from specified area and copy to clipboard.

Parameters:
- *int* **x** - Area start position X.
- *int* **y** - Area start position Y.
- *int* **width** - Area width.
- *int* **height** - Area height.

### *void* CaptureAreaToFile(*Rectangle* selectedArea, *string* filePath, *ImageFormat?* imageFormat = null)

Make screenshot from specified area and save to file.

Parameters:
- *Rectangle* **selectedArea** - Selected area as Rectangle (X, Y, Width, Height).
- *string* **filePath** - Path to file, where screenshot will be saved.
- *ImageFormat?* **imageFormat** - Image format (default Png).

### *void* CaptureAreaToFile(*int* x, *int* y, *int* width, *int* height, *string* filePath, *ImageFormat?* imageFormat = null)

Make screenshot from specified area and save to file.

Parameters:
- *int* **x** - Area start position X.
- *int* **y** - Area start position Y.
- *int* **width** - Area width.
- *int* **height** - Area height.
- *string* **filePath** - Path to file, where screenshot will be saved.
- *ImageFormat?* **imageFormat** - Image format (default Png).

### *Bitmap* CaptureMainScreen()

Make screenshot from main screen attached to computer.

### *BitmapImage* CaptureMainScreenAsBitmapImage()

Make screenshot from main screen attached to computer as WPF BitmapImage.

### *void* CaptureMainScreenToClipboard()

Make screenshot from main screen attached to computer and save to clipboard.

### *void* CaptureMainScreenToFile(string filePath, ImageFormat? imageFormat = null)

Make screenshot from main screen attached to computer and save to file.

Parameters:
- *string* **filePath** - Path to file, where screenshot will be saved.
- *ImageFormat?* **imageFormat** - Image format (default Png).

### *Bitmap* CaptureScreen(*ScreenInfo* screenInfo)

Make screenshot from selected screen attached to computer.

Parameters:
- *ScreenInfo* **screenInfo** - Selected screen info object (available in *SystemController.Screens.Data.ScreenInfo*).

### *BitmapImage* CaptureScreenAsBitmapImage(*ScreenInfo* screenInfo)

Make screenshot from selected screen attached to computer as WPF BitmapImage.

Parameters:
- *ScreenInfo* **screenInfo** - Selected screen info object (available in *SystemController.Screens.Data.ScreenInfo*).

### *void* CaptureScreenToClipboard(*ScreenInfo* screenInfo)

Make screenshot from selected screen attached to computer and save to clipboard.

Parameters:
- *ScreenInfo* **screenInfo** - Selected screen info object (available in *SystemController.Screens.Data.ScreenInfo*).

### *void* CaptureScreenToFile(*ScreenInfo* screenInfo, *string* filePath, *ImageFormat?* imageFormat = null)

Make screenshot from selected screen attached to computer and save to file.

Parameters:
- *ScreenInfo* **screenInfo** - Selected screen info object (available in *SystemController.Screens.Data.ScreenInfo*).
- *string* **filePath** - Path to file, where screenshot will be saved.
- *ImageFormat?* **imageFormat** - Image format (default Png).
