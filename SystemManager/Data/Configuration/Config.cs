using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SystemManager.Data.Processes.Data;
using SystemManager.ViewModels.Base;

namespace SystemManager.Data.Configuration
{
    public class Config : BaseViewModel
    {

        //  VARIABLES

        private Color _appearanceColor;
        private string _lastUsedDirectory;
        private ProcessInfoOption _processInfoOptions;
        private ThemeType _themeType;
        private int _windowPositionX;
        private int _windowPositionY;
        private int _windowWidth;
        private int _windowHeight;


        //  GETTERS & SETTERS

        public Color AppearanceColor
        {
            get => _appearanceColor;
            set
            {
                UpdateProperty(ref _appearanceColor, value);
                AppearanceConfig.Instance.AccentColor = value;
            }
        }

        public string LastUsedDirectory
        {
            get => _lastUsedDirectory;
            set
            {
                UpdateProperty(ref _lastUsedDirectory, value);
            }
        }

        public ProcessInfoOption ProcessInfoOptions
        {
            get => _processInfoOptions;
            set => UpdateProperty(ref _processInfoOptions, value);
        }

        public ThemeType ThemeType
        {
            get => _themeType;
            set
            {
                UpdateProperty(ref _themeType, value);
                AppearanceConfig.Instance.ThemeType = value;
            }
        }

        public int WindowPositionX
        {
            get => _windowPositionX;
            set => UpdateProperty(ref _windowPositionX, value);
        }

        public int WindowPositionY
        {
            get => _windowPositionY;
            set => UpdateProperty(ref _windowPositionY, value);
        }

        public int WindowWidth
        {
            get => _windowWidth;
            set => UpdateProperty(ref _windowWidth, value);
        }

        public int WindowHeight
        {
            get => _windowHeight;
            set => UpdateProperty(ref _windowHeight, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Config class constructor. </summary>
        [JsonConstructor]
        public Config(
            Color? appearanceColor = null,
            string? lastUsedDirectory = null,
            ProcessInfoOption? processInfoOptions = null,
            ThemeType? themeType = null,
            int? windowPositionX = null,
            int? windowPositionY = null,
            int? windowWidth = null,
            int? windowHeight = null)
        {
            AppearanceColor = appearanceColor ?? AppearanceConfig.ACCENT_COLOR;
            LastUsedDirectory = lastUsedDirectory ?? string.Empty;
            ProcessInfoOptions = processInfoOptions ?? new ProcessInfoOption();
            ThemeType = themeType ?? AppearanceConfig.THEME_TYPE;
            WindowPositionX = windowPositionX.HasValue ? windowPositionX.Value : 300;
            WindowPositionY = windowPositionY.HasValue ? windowPositionY.Value : 300;
            WindowWidth = windowWidth.HasValue ? windowWidth.Value : 650;
            WindowHeight = windowHeight.HasValue ? windowHeight.Value : 450;
        }

        #endregion CLASS METHODS

    }
}
