using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SystemController.MouseKeyboard.Data;

namespace SystemManager.Data.Macros.Converters
{
    public class MacroKeyNameConverter : IValueConverter
    {

        //  CONST

        private static readonly string _defaultValue = string.Empty;

        public static readonly Dictionary<byte, string?> KeysMapping = new Dictionary<byte, string?>()
        {
            { Keys.VK_LBUTTON, "LMB" },
            { Keys.VK_RBUTTON, "RMB" },
            { Keys.VK_CANCEL, "CtrlBreak" },
            { Keys.VK_MBUTTON, "MMB" },
            { Keys.VK_XBUTTON1, "X1MB" },
            { Keys.VK_XBUTTON2, "X2MB" },
            { Keys.VK_BACK, "Backspace" },
            { Keys.VK_TAB, "Tab" },
            { Keys.VK_CLEAR, "Clear" },
            { Keys.VK_RETURN, "Enter" },
            { Keys.VK_SHIFT, "Shift" },
            { Keys.VK_CONTROL, "Ctrl" },
            { Keys.VK_MENU, "Alt" },
            { Keys.VK_PAUSE, "Pause" },
            { Keys.VK_CAPITAL, "CapsLock" },
            { Keys.VK_KANA, "IME Kana Mode" },
            { Keys.VK_IME_ON, "IME On" },
            { Keys.VK_JUNJA, "IME Junja Mode" },
            { Keys.VK_FINAL, "IME Final Mode" },
            { Keys.VK_HANJA, "IME Hanja Mode" },
            { Keys.VK_IME_OFF, "IME Off" },
            { Keys.VK_ESCAPE, "Esc" },
            { Keys.VK_CONVERT, "IME Convert" },
            { Keys.VK_NONCONVERT, "IME No Convert" },
            { Keys.VK_ACCEPT, "IME Accept" },
            { Keys.VK_MODECHANGE, "IME Mode Change" },
            { Keys.VK_SPACE, "Space" },
            { Keys.VK_PRIOR, "PageUp" },
            { Keys.VK_NEXT, "PageDown" },
            { Keys.VK_END, "End" },
            { Keys.VK_HOME, "Home" },
            { Keys.VK_LEFT, "Left" },
            { Keys.VK_UP, "Up" },
            { Keys.VK_RIGHT, "Right" },
            { Keys.VK_DOWN, "Down" },
            { Keys.VK_SELECT, "Select" },
            { Keys.VK_PRbyte, "PR Byte" },
            { Keys.VK_EXECUTE, "Execute" },
            { Keys.VK_SNAPSHOT, "PrintScreen" },
            { Keys.VK_INSERT, "Insert" },
            { Keys.VK_DELETE, "Delete" },
            { Keys.VK_HELP, "Help" },
            { Keys.VK_0, "0" },
            { Keys.VK_1, "1" },
            { Keys.VK_2, "2" },
            { Keys.VK_3, "3" },
            { Keys.VK_4, "4" },
            { Keys.VK_5, "5" },
            { Keys.VK_6, "6" },
            { Keys.VK_7, "7" },
            { Keys.VK_8, "8" },
            { Keys.VK_9, "9" },
            { Keys.VK_A, "A" },
            { Keys.VK_B, "B" },
            { Keys.VK_C, "C" },
            { Keys.VK_D, "D" },
            { Keys.VK_E, "E" },
            { Keys.VK_F, "F" },
            { Keys.VK_G, "G" },
            { Keys.VK_H, "H" },
            { Keys.VK_I, "I" },
            { Keys.VK_J, "J" },
            { Keys.VK_K, "K" },
            { Keys.VK_L, "L" },
            { Keys.VK_M, "M" },
            { Keys.VK_N, "N" },
            { Keys.VK_O, "O" },
            { Keys.VK_P, "P" },
            { Keys.VK_Q, "Q" },
            { Keys.VK_R, "R" },
            { Keys.VK_S, "S" },
            { Keys.VK_T, "T" },
            { Keys.VK_U, "U" },
            { Keys.VK_V, "V" },
            { Keys.VK_W, "W" },
            { Keys.VK_X, "X" },
            { Keys.VK_Y, "Y" },
            { Keys.VK_Z, "Z" },
            { Keys.VK_LWIN, "Left Win" },
            { Keys.VK_RWIN, "Right Win" },
            { Keys.VK_APPS, "Applications" },
            { Keys.VK_SLEEP, "Sleep" },
            { Keys.VK_NUMPAD0, "Num 0" },
            { Keys.VK_NUMPAD1, "Num 1" },
            { Keys.VK_NUMPAD2, "Num 2" },
            { Keys.VK_NUMPAD3, "Num 3" },
            { Keys.VK_NUMPAD4, "Num 4" },
            { Keys.VK_NUMPAD5, "Num 5" },
            { Keys.VK_NUMPAD6, "Num 6" },
            { Keys.VK_NUMPAD7, "Num 7" },
            { Keys.VK_NUMPAD8, "Num 8" },
            { Keys.VK_NUMPAD9, "Num 9" },
            { Keys.VK_MULTIPLY, "*" },
            { Keys.VK_ADD, "+" },
            { Keys.VK_SEPARATOR, "Separator" },
            { Keys.VK_SUBTRACT, "-" },
            { Keys.VK_DECIMAL, "." },
            { Keys.VK_DIVIDE, "," },
            { Keys.VK_F1, "F1" },
            { Keys.VK_F2, "F2" },
            { Keys.VK_F3, "F3" },
            { Keys.VK_F4, "F4" },
            { Keys.VK_F5, "F5" },
            { Keys.VK_F6, "F6" },
            { Keys.VK_F7, "F7" },
            { Keys.VK_F8, "F8" },
            { Keys.VK_F9, "F9" },
            { Keys.VK_F10, "F10" },
            { Keys.VK_F11, "F11" },
            { Keys.VK_F12, "F12" },
            { Keys.VK_F13, "F13" },
            { Keys.VK_F14, "F14" },
            { Keys.VK_F15, "F15" },
            { Keys.VK_F16, "F16" },
            { Keys.VK_F17, "F17" },
            { Keys.VK_F18, "F18" },
            { Keys.VK_F19, "F19" },
            { Keys.VK_F20, "F20" },
            { Keys.VK_F21, "F21" },
            { Keys.VK_F22, "F22" },
            { Keys.VK_F23, "F23" },
            { Keys.VK_F24, "F24" },
            { Keys.VK_NUMLOCK, "NumLock" },
            { Keys.VK_SCROLL, "ScrollLock" },
            { Keys.VK_LSHIFT, "Left Shift" },
            { Keys.VK_RSHIFT, "Right Shift" },
            { Keys.VK_LCONTROL, "Left Ctrl" },
            { Keys.VK_RCONTROL, "Right Ctrl" },
            { Keys.VK_LMENU, "Left Alt" },
            { Keys.VK_RMENU, "Right Alt" },
            { Keys.VK_BROWSER_BACK, "Browser Back" },
            { Keys.VK_BROWSER_FORWARD, "Browser Forward" },
            { Keys.VK_BROWSER_REFRESH, "Browser Refresh" },
            { Keys.VK_BROWSER_STOP, "Browser Stop" },
            { Keys.VK_BROWSER_SEARCH, "Browser Search" },
            { Keys.VK_BROWSER_FAVORITES, "Browser Favourites" },
            { Keys.VK_BROWSER_HOME, "Browser Home" },
            { Keys.VK_VOLUME_MUTE, "Mute" },
            { Keys.VK_VOLUME_DOWN, "Vol -" },
            { Keys.VK_VOLUME_UP, "Vol +" },
            { Keys.VK_MEDIA_NEXT_TRACK, "Next Song" },
            { Keys.VK_MEDIA_PREV_TRACK, "Previous Song" },
            { Keys.VK_MEDIA_STOP, "Stop" },
            { Keys.VK_MEDIA_PLAY_PAUSE, "Play/Pause" },
            { Keys.VK_LAUNCH_MAIL, "App Mail" },
            { Keys.VK_LAUNCH_MEDIA_SELECT, "App Media" },
            { Keys.VK_LAUNCH_APP1, "App1" },
            { Keys.VK_LAUNCH_APP2, "App2" },
            { Keys.VK_OEM_1, ";:" },
            { Keys.VK_OEM_PLUS, "+" },
            { Keys.VK_OEM_COMMA, "," },
            { Keys.VK_OEM_MINUS, "-" },
            { Keys.VK_OEM_PERIOD, "." },
            { Keys.VK_OEM_2, "/?" },
            { Keys.VK_OEM_3, "`~" },
            { Keys.VK_OEM_4, "[{" },
            { Keys.VK_OEM_5, "\\|" },
            { Keys.VK_OEM_6, "]}" },
            { Keys.VK_OEM_7, "'\"" },
            { Keys.VK_OEM_8, "VK_OEM_8" },
            { Keys.VK_OEM_102, "\\|" },
            { Keys.VK_PROCESSKEY, "IME Process" },
            { Keys.VK_PACKET, "VK_PACKET" },
            { Keys.VK_ATTN, "Attn" },
            { Keys.VK_CRSEL, "CrSel" },
            { Keys.VK_EXSEL, "ExSel" },
            { Keys.VK_EREOF, "Erase EOF" },
            { Keys.VK_PLAY, "Play" },
            { Keys.VK_ZOOM, "Zoom" },
            { Keys.VK_NONAME, "Reserved" },
            { Keys.VK_PA1, "PA1" },
            { Keys.VK_OEM_CLEAR, "Clear" }
        };


        //  METHODS

        //  --------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var keyCode = value as byte?;

            if (keyCode.HasValue)
                return GetKeyName(keyCode.Value);

            return _defaultValue;
        }

        //  --------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get name of key code. </summary>
        /// <param name="keyCode"> Key code. </param>
        /// <returns> Name of key code. </returns>
        public static string GetKeyName(byte keyCode)
        {
            return KeysMapping.TryGetValue(keyCode, out string? result) && !string.IsNullOrEmpty(result)
                ? result : _defaultValue;
        }

    }
}
