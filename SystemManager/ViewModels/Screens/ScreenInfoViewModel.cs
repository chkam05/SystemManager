using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.Screens.Data;
using SystemManager.ViewModels.Base;

namespace SystemManager.ViewModels.Screens
{
    public class ScreenInfoViewModel : BaseViewModel
    {

        //  VARIABLES

        private ScreenInfo _screenInfo;


        //  GETTERS & SETTERS

        public ScreenInfo ScreenInfo
        {
            get => _screenInfo;
            private set
            {
                UpdateProperty(ref _screenInfo, value);
                UpdateProperties();
            }
        }

        public int BitsPerPixel
        {
            get => _screenInfo.BitsPerPixel;
        }

        public string? DeviceName
        {
            get => _screenInfo.DeviceName;
        }

        public string? DevicePath
        {
            get => _screenInfo.DevicePath;
        }

        public string? DriverVersion
        {
            get => _screenInfo.DriverVersion;
        }

        public int? Frequency
        {
            get => _screenInfo.Frequency;
        }

        public string? InternalDeviceName
        {
            get => _screenInfo.InternalDeviceName;
        }

        public bool IsMainScreen
        {
            get => _screenInfo.IsMainScreen;
        }

        public int? Orientation
        {
            get => _screenInfo.Orientation;
        }

        public bool HasOryginalDimensions
        {
            get => _screenInfo.Position != null && _screenInfo.Size != null;
        }

        public int PositionX
        {
            get => _screenInfo.Position?.X ?? _screenInfo.VirtualPosition.X;
        }

        public int PositionY
        {
            get => _screenInfo.Position?.Y ?? _screenInfo.VirtualPosition.Y;
        }

        public string PositionXY
        {
            get => $"{PositionX} x {PositionY}";
        }

        public int SizeWidth
        {
            get => _screenInfo.Size?.Width ?? _screenInfo.VirtualSize.Width;
        }

        public int SizeHeight
        {
            get => _screenInfo.Size?.Height ?? _screenInfo.VirtualSize.Height;
        }

        public string Size
        {
            get => $"{SizeWidth} x {SizeHeight}";
        }

        public string? SpecVersion
        {
            get => _screenInfo.SpecVersion;
        }

        public int VirtualPositionX
        {
            get => _screenInfo.VirtualPosition.X;
        }

        public int VirtualPositionY
        {
            get => _screenInfo.VirtualPosition.Y;
        }

        public string VirtualPositionXY
        {
            get => $"{VirtualPositionX} x {VirtualPositionY}";
        }

        public int VirtualSizeWidth
        {
            get => _screenInfo.VirtualSize.Width;
        }

        public int VirtualSizeHeight
        {
            get => _screenInfo.VirtualSize.Height;
        }

        public string VirtualSize
        {
            get => $"{VirtualSizeWidth} x {VirtualSizeHeight}";
        }

        public bool HasOryginalWorkDimensions
        {
            get => _screenInfo.WorkPosition != null && _screenInfo.WorkSize != null;
        }

        public int WorkPositionX
        {
            get => _screenInfo.WorkPosition?.X ?? _screenInfo.VirtualWorkPosition.X;
        }

        public int WorkPositionY
        {
            get => _screenInfo.WorkPosition?.Y ?? _screenInfo.VirtualWorkPosition.Y;
        }

        public string WorkPositionXY
        {
            get => $"{WorkPositionX} x {WorkPositionY}";
        }

        public int WorkSizeWidth
        {
            get => _screenInfo.WorkSize?.Width ?? _screenInfo.VirtualWorkSize.Width;
        }

        public int WorkSizeHeight
        {
            get => _screenInfo.WorkSize?.Height ?? _screenInfo.VirtualWorkSize.Height;
        }

        public string WorkSize
        {
            get => $"{WorkSizeWidth} x {WorkSizeHeight}";
        }

        public int VirtualWorkPositionX
        {
            get => _screenInfo.VirtualWorkPosition.X;
        }

        public int VirtualWorkPositionY
        {
            get => _screenInfo.VirtualWorkPosition.Y;
        }

        public string VirtualWorkPositionXY
        {
            get => $"{VirtualWorkPositionX} x {VirtualWorkPositionY}";
        }

        public int VirtualWorkSizeWidth
        {
            get => _screenInfo.VirtualWorkSize.Width;
        }

        public int VirtualWorkSizeHeight
        {
            get => _screenInfo.VirtualWorkSize.Height;
        }

        public string VirtualWorkSize
        {
            get => $"{VirtualWorkSizeWidth} x {VirtualWorkSizeHeight}";
        }

        public float? XScale
        {
            get => _screenInfo.XScale;
        }

        public float? YScale
        {
            get => _screenInfo.YScale;
        }

        public string? Scale
        {
            get => XScale.HasValue && YScale.HasValue
                ? $"{XScale} x {YScale}" : null;
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ScreenInfoViewModel class constructor. </summary>
        /// <param name="screenInfo"> Screen info. </param>
        public ScreenInfoViewModel(ScreenInfo screenInfo)
        {
            _screenInfo = screenInfo;

            UpdateProperties();
        }

        #endregion CLASS METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Update all properties. </summary>
        private void UpdateProperties()
        {
            OnPropertyChanged(nameof(ScreenInfo));
            OnPropertyChanged(nameof(BitsPerPixel));
            OnPropertyChanged(nameof(DeviceName));
            OnPropertyChanged(nameof(DevicePath));
            OnPropertyChanged(nameof(DriverVersion));
            OnPropertyChanged(nameof(Frequency));
            OnPropertyChanged(nameof(InternalDeviceName));
            OnPropertyChanged(nameof(IsMainScreen));
            OnPropertyChanged(nameof(Orientation));
            OnPropertyChanged(nameof(HasOryginalDimensions));
            OnPropertyChanged(nameof(PositionX));
            OnPropertyChanged(nameof(PositionY));
            OnPropertyChanged(nameof(PositionXY));
            OnPropertyChanged(nameof(SizeWidth));
            OnPropertyChanged(nameof(SizeHeight));
            OnPropertyChanged(nameof(Size));
            OnPropertyChanged(nameof(SpecVersion));
            OnPropertyChanged(nameof(VirtualPositionX));
            OnPropertyChanged(nameof(VirtualPositionY));
            OnPropertyChanged(nameof(VirtualPositionXY));
            OnPropertyChanged(nameof(VirtualSizeWidth));
            OnPropertyChanged(nameof(VirtualSizeHeight));
            OnPropertyChanged(nameof(VirtualSize));
            OnPropertyChanged(nameof(HasOryginalWorkDimensions));
            OnPropertyChanged(nameof(WorkPositionX));
            OnPropertyChanged(nameof(WorkPositionY));
            OnPropertyChanged(nameof(WorkPositionXY));
            OnPropertyChanged(nameof(WorkSizeWidth));
            OnPropertyChanged(nameof(WorkSizeHeight));
            OnPropertyChanged(nameof(WorkSize));
            OnPropertyChanged(nameof(VirtualWorkPositionX));
            OnPropertyChanged(nameof(VirtualWorkPositionY));
            OnPropertyChanged(nameof(VirtualWorkPositionXY));
            OnPropertyChanged(nameof(VirtualWorkSizeWidth));
            OnPropertyChanged(nameof(VirtualWorkSizeHeight));
            OnPropertyChanged(nameof(VirtualWorkSize));
            OnPropertyChanged(nameof(XScale));
            OnPropertyChanged(nameof(YScale));
        }

        #endregion PROPERITES CHANGED METHODS

    }
}
