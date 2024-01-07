using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SystemController.Data;
using SystemController.MouseKeyboard.Data;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace SystemController.MouseKeyboard
{
    public class MouseKeyboardController
    {

        //  IMPORTS

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);


        //  VARIABLES

        private bool _pressedLMB = false;
        private bool _pressedMMB = false;
        private bool _pressedRMB = false;
        private List<byte> _pressedKeys;


        //  GETTERS & SETTERS

        public byte[] PressedKeys
        {
            get => _pressedKeys.ToArray();
        }

        public MouseButton[] PressedMouseButtons
        {
            get => GetMousePressedButtons();
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MouseKeyboardController class constructor. </summary>
        public MouseKeyboardController()
        {
            _pressedKeys = new List<byte>();
        }

        #endregion CLASS METHODS

        #region KEYBOARD

        //  --------------------------------------------------------------------------------
        /// <summary> Simulate key press. </summary>
        /// <param name="keyCode"> Key code. </param>
        /// <param name="mode"> Press mode. </param>
        /// <exception cref="Exception"> Key has already been pressed or cannot be released. </exception>
        public void SimulateKeyPress(byte keyCode, KeyState mode = KeyState.PressRelease)
        {
            if (IsPress(mode))
            {
                if (_pressedKeys.Contains(keyCode))
                    throw new Exception($"Release {keyCode} key before it is pressed again.");

                keybd_event(keyCode, 0, Keys.KEYEVENTF_KEYDOWN, UIntPtr.Zero);
                _pressedKeys.Add(keyCode);
            }

            if (IsRelease(mode))
            {
                if (!_pressedKeys.Contains(keyCode))
                    throw new Exception($"Key {keyCode} has not been pressed.");

                keybd_event(keyCode, 0, Keys.KEYEVENTF_KEYUP, UIntPtr.Zero);
                _pressedKeys.Remove(keyCode);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Simulate key combination press. </summary>
        /// <param name="keyCodes"> Key codes. </param>
        /// <exception cref="Exception"> Key has already been pressed or cannot be released. </exception>
        public void SimulateKeyCombination(params byte[] keyCodes)
        {
            var pressedKeys = keyCodes.Where(k => _pressedKeys.Contains(k));

            if (pressedKeys.Any())
                throw new Exception($"Release {string.Join(",", pressedKeys)} keys before they are pressed again.");

            foreach (var keyCode in keyCodes)
                keybd_event(keyCode, 0, Keys.KEYEVENTF_KEYDOWN, UIntPtr.Zero);

            foreach (var keyCode in keyCodes)
                keybd_event(keyCode, 0, Keys.KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        #endregion KEYBOARD

        #region MOUSE

        //  --------------------------------------------------------------------------------
        /// <summary> Get mouse position. </summary>
        /// <returns> Point. </returns>
        public POINT GetMousePosition()
        {
            GetCursorPos(out POINT cursorPos);
            return cursorPos;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Simulate mouse click. </summary>
        /// <param name="mouseButton"> Mouse button. </param>
        /// <param name="mode"> Press mode. </param>
        /// <exception cref="Exception"> The button has already been pressed or cannot be released. </exception>
        public void SimulateMouseClick(MouseButton mouseButton, KeyState mode = KeyState.PressRelease)
        {
            GetCursorPos(out POINT cursorPos);

            switch (mouseButton)
            {
                case MouseButton.LeftButton:
                    if (IsPress(mode))
                    {
                        if (_pressedLMB)
                            throw new Exception($"Release LMB before it is pressed again.");

                        mouse_event(MouseButtons.MOUSEEVENTF_LEFTDOWN, cursorPos.X, cursorPos.Y, 0, UIntPtr.Zero);
                        _pressedLMB = true;
                    }
                    if (IsRelease(mode))
                    {
                        if (!_pressedLMB)
                            throw new Exception($"LMB has not been pressed.");

                        mouse_event(MouseButtons.MOUSEEVENTF_LEFTUP, cursorPos.X, cursorPos.Y, 0, UIntPtr.Zero);
                        _pressedLMB = false;
                    }
                    break;

                case MouseButton.RightButton:
                    if (IsPress(mode))
                    {
                        if (_pressedRMB)
                            throw new Exception($"Release RMB before it is pressed again.");

                        mouse_event(MouseButtons.MOUSEEVENTF_RIGHTDOWN, cursorPos.X, cursorPos.Y, 0, UIntPtr.Zero);
                        _pressedRMB = true;
                    }
                    if (IsRelease(mode))
                    {
                        if (!_pressedRMB)
                            throw new Exception($"LMB has not been pressed.");

                        mouse_event(MouseButtons.MOUSEEVENTF_RIGHTUP, cursorPos.X, cursorPos.Y, 0, UIntPtr.Zero);
                        _pressedRMB = false;
                    }
                    break;

                case MouseButton.MiddleButton:
                    if (IsPress(mode))
                    {
                        if (_pressedMMB)
                            throw new Exception($"Release MMB before it is pressed again.");

                        mouse_event(MouseButtons.MOUSEEVENTF_MIDDLEDOWN, cursorPos.X, cursorPos.Y, 0, UIntPtr.Zero);
                        _pressedMMB = true;
                    }
                    if (IsRelease(mode))
                    {
                        if (!_pressedMMB)
                            throw new Exception($"MMB has not been pressed.");

                        mouse_event(MouseButtons.MOUSEEVENTF_MIDDLEUP, cursorPos.X, cursorPos.Y, 0, UIntPtr.Zero);
                        _pressedMMB = false;
                    }
                    break;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Simulate key move. </summary>
        /// <param name="x"> Destination position X. </param>
        /// <param name="y"> Destination position Y. </param>
        public void SimulateMouseMove(int x, int y)
        {
            SetCursorPos(x, y);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Simulate horizontal mouse scroll. </summary>
        /// <param name="delta"> Scroll delta. </param>
        public void SimulateHorizontalMouseScroll(int delta)
        {
            GetCursorPos(out POINT cursorPos);
            mouse_event(MouseButtons.MOUSEEVENTF_HWHEEL, cursorPos.X, cursorPos.Y, (uint)delta, UIntPtr.Zero);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Simulate vertical mouse scroll. </summary>
        /// <param name="delta"> Scroll delta. </param>
        public void SimulateVerticalMouseScroll(int delta)
        {
            GetCursorPos(out POINT cursorPos);
            mouse_event(MouseButtons.MOUSEEVENTF_WHEEL, cursorPos.X, cursorPos.Y, (uint)delta, UIntPtr.Zero);
        }

        #endregion MOUSE

        #region UTILITY METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Get mouse pressed buttons. </summary>
        /// <returns> List of mouse pressed buttons. </returns>
        private MouseButton[] GetMousePressedButtons()
        {
            var pressedButtons = new List<MouseButton>();

            if (_pressedLMB)
                pressedButtons.Add(MouseButton.LeftButton);

            if (_pressedMMB)
                pressedButtons.Add(MouseButton.MiddleButton);

            if (_pressedRMB)
                pressedButtons.Add(MouseButton.RightButton);

            return pressedButtons.ToArray();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if key press mode is press. </summary>
        /// <param name="mode"> Key press mode. </param>
        /// <returns> True - press; False - otherwise. </returns>
        private bool IsPress(KeyState mode)
        {
            return mode == KeyState.Press || mode == KeyState.PressRelease;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if key press mode is release. </summary>
        /// <param name="mode"> Key press mode. </param>
        /// <returns> True - release; False - otherwise. </returns>
        private bool IsRelease(KeyState mode)
        {
            return mode == KeyState.Release || mode == KeyState.PressRelease;
        }

        #endregion UTILITY METHODS

    }
}
