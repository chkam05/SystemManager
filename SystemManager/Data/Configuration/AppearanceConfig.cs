using chkam05.Tools.ControlsEx.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SystemManager.ViewModels.Base;

namespace SystemManager.Data.Configuration
{
    public class AppearanceConfig : BaseViewModel
    {

        //  CONST

        private static readonly double LUMINANCE_R = 0.299;
        private static readonly double LUMINANCE_G = 0.587;
        private static readonly double LUMINANCE_B = 0.114;

        private static readonly int INACTIVE_FACTOR = 15;
        private static readonly int MOUSE_OVER_FACTOR = 15;
        private static readonly int PRESSED_FACTOR = 10;
        private static readonly int SELECTED_FACTOR = 5;

        public static readonly Color ACCENT_COLOR = Color.FromArgb(255, 0, 120, 215);
        public static readonly ThemeType THEME_TYPE = ThemeType.Dark;
        public static readonly Color DARK_THEME_COLOR = Color.FromArgb(255, 36, 36, 36);
        public static readonly Color LIGHT_THEME_COLOR = Color.FromArgb(255, 219, 219, 219);


        //  VARIABLES

        private static AppearanceConfig? _instance;
        private static object _instanceLock = new object();

        private Color _accentColor = ACCENT_COLOR;
        private ThemeType _themeType = THEME_TYPE;

        private Brush _accentColorBrush;
        private Brush _accentForegroundBrush;
        private Brush _accentMouseOverBrush;
        private Brush _accentPressedBrush;
        private Brush _accentSelectedBrush;
        private Brush _themeBackgroundBrush;
        private Brush _themeForegroundBrush;
        private Brush _themeShadeBackgroundBrush;
        private Brush _themeMouseOverBrush;
        private Brush _themePressedBrush;
        private Brush _themeSelectedBrush;


        //  GETTERS & SETTERS

        public static AppearanceConfig Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                        _instance = new AppearanceConfig();

                    return _instance;
                }
            }
        }

        public Color AccentColor
        {
            get => _accentColor;
            set
            {
                UpdateProperty(ref _accentColor, value);
                Setup();
            }
        }

        public ThemeType ThemeType
        {
            get => _themeType;
            set
            {
                UpdateProperty(ref _themeType, value);
                Setup();
            }
        }

        public Brush AccentColorBrush
        {
            get => _accentColorBrush;
            set
            {
                UpdateProperty(ref _accentColorBrush, value);
            }
        }

        public Brush AccentForegroundBrush
        {
            get => _accentForegroundBrush;
            set
            {
                UpdateProperty(ref _accentForegroundBrush, value);
            }
        }

        public Brush AccentMouseOverBrush
        {
            get => _accentMouseOverBrush;
            set
            {
                UpdateProperty(ref _accentMouseOverBrush, value);
            }
        }

        public Brush AccentPressedBrush
        {
            get => _accentPressedBrush;
            set
            {
                UpdateProperty(ref _accentPressedBrush, value);
            }
        }

        public Brush AccentSelectedBrush
        {
            get => _accentSelectedBrush;
            set
            {
                UpdateProperty(ref _accentSelectedBrush, value);
            }
        }

        public Brush ThemeBackgroundBrush
        {
            get => _themeBackgroundBrush;
            set
            {
                UpdateProperty(ref _themeBackgroundBrush, value);
            }
        }

        public Brush ThemeForegroundBrush
        {
            get => _themeForegroundBrush;
            set
            {
                UpdateProperty(ref _themeForegroundBrush, value);
            }
        }

        public Brush ThemeShadeBackgroundBrush
        {
            get => _themeShadeBackgroundBrush;
            set
            {
                UpdateProperty(ref _themeShadeBackgroundBrush, value);
            }
        }

        public Brush ThemeMouseOverBrush
        {
            get => _themeMouseOverBrush;
            set
            {
                UpdateProperty(ref _themeMouseOverBrush, value);
            }
        }

        public Brush ThemePressedBrush
        {
            get => _themePressedBrush;
            set
            {
                UpdateProperty(ref _themePressedBrush, value);
            }
        }

        public Brush ThemeSelectedBrush
        {
            get => _themeSelectedBrush;
            set
            {
                UpdateProperty(ref _themeSelectedBrush, value);
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Private AppearanceConfig class constructor. </summary>
        private AppearanceConfig()
        {
            Setup();
        }

        #endregion CLASS METHODS

        #region SETUP METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Setup. </summary>
        private void Setup()
        {
            var accentAhslColor = AHSLColor.FromColor(AccentColor);
            var accentForegroundColor = GetForegroundContrastedColor(AccentColor);

            var accentMouseOverColor = UpdateAHSLColor(accentAhslColor,
                lightness: accentAhslColor.L + MOUSE_OVER_FACTOR).ToColor();

            var accentPressedColor = UpdateAHSLColor(accentAhslColor,
                lightness: accentAhslColor.L - PRESSED_FACTOR).ToColor();

            var accentSelectedColor = UpdateAHSLColor(accentAhslColor,
                lightness: accentAhslColor.L - SELECTED_FACTOR).ToColor();

            AccentColorBrush = new SolidColorBrush(AccentColor);
            AccentForegroundBrush = new SolidColorBrush(accentForegroundColor);
            AccentMouseOverBrush = new SolidColorBrush(accentMouseOverColor);
            AccentPressedBrush = new SolidColorBrush(accentPressedColor);
            AccentSelectedBrush = new SolidColorBrush(accentSelectedColor);

            var backgroundColor = ThemeType == ThemeType.Dark
                ? Colors.Black
                : Colors.White;

            var foregroundColor = ThemeType == ThemeType.Dark
                ? Colors.White
                : Colors.Black;

            var shadeBackgroundColor = ThemeType == ThemeType.Dark
                ? AppearanceConfig.DARK_THEME_COLOR
                : AppearanceConfig.LIGHT_THEME_COLOR;

            var backgroundAhslColor = AHSLColor.FromColor(backgroundColor);

            var themeMouseOverColor = UpdateAHSLColor(backgroundAhslColor,
                lightness: backgroundAhslColor.S > 50
                    ? backgroundAhslColor.S + MOUSE_OVER_FACTOR
                    : backgroundAhslColor.L - MOUSE_OVER_FACTOR,
                saturation: 0).ToColor();

            var themePressedColor = UpdateAHSLColor(backgroundAhslColor,
                lightness: backgroundAhslColor.S > 50
                    ? backgroundAhslColor.S - PRESSED_FACTOR
                    : backgroundAhslColor.L + PRESSED_FACTOR,
                saturation: 0).ToColor();

            var themeSelectedColor = UpdateAHSLColor(backgroundAhslColor,
                lightness: backgroundAhslColor.S > 50
                    ? backgroundAhslColor.S - SELECTED_FACTOR
                    : backgroundAhslColor.L + SELECTED_FACTOR,
                saturation: 0).ToColor();

            ThemeBackgroundBrush = new SolidColorBrush(backgroundColor);
            ThemeForegroundBrush = new SolidColorBrush(foregroundColor);
            ThemeShadeBackgroundBrush = new SolidColorBrush(shadeBackgroundColor);
            ThemeMouseOverBrush = new SolidColorBrush(themeMouseOverColor);
            ThemePressedBrush = new SolidColorBrush(themePressedColor);
            ThemeSelectedBrush = new SolidColorBrush(themeSelectedColor);
        }

        #endregion SETUP METHODS

        #region UTILITY METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Get foreground contrasted color. </summary>
        /// <param name="accentColor"> Accent color. </param>
        /// <returns> Contrasted foreground color. </returns>
        public static Color GetForegroundContrastedColor(Color accentColor)
        {
            double luminance = (LUMINANCE_R * accentColor.R + LUMINANCE_G * accentColor.G
                + LUMINANCE_B * accentColor.B) / 255;

            if (luminance > 0.5)
                return Colors.Black;
            else
                return Colors.White;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Update AHSL color. </summary>
        /// <param name="ahslColor"> AHSL color. </param>
        /// <param name="alpha"> Alpha factor. </param>
        /// <param name="hue"> Hue factor. </param>
        /// <param name="saturation"> Saturation factor. </param>
        /// <param name="lightness"> Lightness factor. </param>
        /// <returns> Updated AHSL color. </returns>
        private static AHSLColor UpdateAHSLColor(AHSLColor ahslColor, byte? alpha = null,
            int? hue = null, int? saturation = null, int? lightness = null)
        {
            return new AHSLColor(
                alpha.HasValue ? alpha.Value : ahslColor.A,
                hue.HasValue ? hue.Value : ahslColor.H,
                saturation.HasValue ? saturation.Value : ahslColor.S,
                lightness.HasValue ? lightness.Value : ahslColor.L);
        }

        #endregion UTILITY METHODS

    }
}
