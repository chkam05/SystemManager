using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SystemController.Data;
using SystemController.MouseKeyboard.Data;
using SystemController.MouseKeyboard.Events;
using static SystemController.MouseKeyboard.KeyboardReader;

namespace SystemController.MouseKeyboard
{
    public class MouseReader
    {

        //  CONST

        private const int WH_MOUSE_LL = 14;
        private const string ERROR_MESSAGE = "An exception was thrown at \"{0}\" with the message: {1}";


        //  DELEGATES

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate void MouseClickEventHandler(object sender, MouseClickEventArgs e);
        public delegate void MouseMoveEventHandler(object sender, MouseMoveEventArgs e);
        public delegate void MouseScrollEventHandler(object sender, MouseScrollEventArgs e);
        public delegate void MouseListeningFinishedEventHandler(object sender, MouseListeningFinishedEventArgs e);


        //  EVENTS

        public MouseClickEventHandler? OnMouseClick;
        public MouseMoveEventHandler? OnMouseMove;
        public MouseScrollEventHandler? OnMouseScroll;
        public MouseListeningFinishedEventHandler? OnStopListening;
        public ThrowExceptionEventHandler? OnThrownException;


        //  IMPORTS

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);


        //  VARIABLES

        private IntPtr _hookIdMouse;
        private HookProc? _mouseHookCallback;

        private List<Exception> _exceptions;
        private POINT _mousePosition;
        private List<MouseButton> _pressedButtons;

        private object _exceptionsLock = new object();
        private object _mousePositionLock = new object();
        private object _pressedButtonsLock = new object();

        public bool IsListening { get; private set; }


        //  GETTERS & SETTERS

        private List<Exception> exceptions
        {
            get
            {
                lock (_exceptionsLock)
                {
                    return _exceptions;
                }
            }
        }

        private POINT mousePosition
        {
            get
            {
                lock (_mousePositionLock)
                {
                    return _mousePosition;
                }
            }
            set
            {
                lock (_mousePositionLock)
                {
                    _mousePosition = value;
                }
            }
        }

        private List<MouseButton> pressedButtons
        {
            get
            {
                lock (_pressedButtonsLock)
                {
                    return _pressedButtons;
                }
            }
        }

        public int HeldButtons
        {
            get => pressedButtons.Count;
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MouseReader class constructor. </summary>
        public MouseReader()
        {
            IsListening = false;

            _exceptions = new List<Exception>();
            _pressedButtons = new List<MouseButton>();
        }

        #endregion CLASS METHODS

        #region HOOK METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Set mouse hook. </summary>
        private void SetHook()
        {
            try
            {
                _mouseHookCallback = MouseHookCallback;
                _hookIdMouse = SetWindowsHookEx(WH_MOUSE_LL, _mouseHookCallback, IntPtr.Zero, 0);

                if (_hookIdMouse == IntPtr.Zero)
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
            catch (Exception innerException)
            {
                HandleException(innerException);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Unset mouse hook. </summary>
        private void UnHook()
        {
            try
            {
                if (_hookIdMouse != IntPtr.Zero)
                {
                    UnhookWindowsHookEx(_hookIdMouse);
                    _hookIdMouse = IntPtr.Zero;
                }
            }
            catch (Exception innerException)
            {
                HandleException(innerException);
            }
        }

        #endregion HOOK METHODS

        #region INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Get list of thrown exceptions from last run. </summary>
        /// <returns> List of thrown exceptions. </returns>
        public List<Exception> GetThrownExceptions()
        {
            return _exceptions;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Start listening mouse. </summary>
        public void StartListening()
        {
            exceptions.Clear();
            pressedButtons.Clear();

            IsListening = true;
            SetHook();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop listening mouse. </summary>
        public void StopListening()
        {
            UnHook();
            IsListening = false;
            OnStopListening?.Invoke(this, new MouseListeningFinishedEventArgs(exceptions, HeldButtons, mousePosition));
        }

        #endregion INTERACTION METHODS

        #region LISTENING METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after performing an action by mouse. </summary>
        /// <param name="nCode"> Status code that indicates whether the data has been processed by the hook procedure. </param>
        /// <param name="wParam"> Message type. </param>
        /// <param name="lParam"> Pointer to a structure containing data about action. </param>
        /// <returns> Value returned by the string procedure and should also be returned by CallNextHookEx. </returns>
        private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT mouseHookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                if (wParam == (IntPtr)MouseButtons.WM_LBUTTONDOWN)
                {
                    HandleMouseClick(MouseButton.LeftButton, KeyState.Press);
                }
                else if (wParam == (IntPtr)MouseButtons.WM_LBUTTONUP)
                {
                    HandleMouseClick(MouseButton.LeftButton, KeyState.Release);
                }
                else if (wParam == (IntPtr)MouseButtons.WM_RBUTTONDOWN)
                {
                    HandleMouseClick(MouseButton.RightButton, KeyState.Press);
                }
                else if (wParam == (IntPtr)MouseButtons.WM_RBUTTONUP)
                {
                    HandleMouseClick(MouseButton.LeftButton, KeyState.Release);
                }
                else if (wParam == (IntPtr)MouseButtons.WM_MBUTTONDOWN)
                {
                    HandleMouseClick(MouseButton.MiddleButton, KeyState.Press);
                }
                else if (wParam == (IntPtr)MouseButtons.WM_MBUTTONUP)
                {
                    HandleMouseClick(MouseButton.LeftButton, KeyState.Release);
                }
                else if (wParam == (IntPtr)MouseButtons.WM_MOUSEWHEEL)
                {
                    HandleMouseScroll(mouseHookStruct.mouseData, ScrollOrientation.Vertical);
                }
                else if (wParam == (IntPtr)MouseButtons.WM_MOUSEHWHEEL)
                {
                    HandleMouseScroll(mouseHookStruct.mouseData, ScrollOrientation.Horizontal);
                }

                HandleMouseMove(mouseHookStruct.pt);
            }

            return CallNextHookEx(_hookIdMouse, nCode, wParam, lParam);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Handle thrown exception. </summary>
        /// <param name="thrownException"> Thrown exception. </param>
        /// <param name="methodName"> Method from which method has been thrown. </param>
        private void HandleException(Exception thrownException, [CallerMemberName] string? methodName = null)
        {
            var message = string.Format(ERROR_MESSAGE, methodName, thrownException.Message);
            var exception = new Exception(message, thrownException);

            exceptions.Add(exception);

            if (OnThrownException != null)
                OnThrownException.Invoke(this, new ThrownExceptionEventArgs(exception));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method for handle mouse buttons press. </summary>
        /// <param name="mouseButton"> Mouse button. </param>
        /// <param name="buttonState"> Button state. </param>
        private void HandleMouseClick(MouseButton mouseButton, KeyState buttonState)
        {
            if (pressedButtons.Contains(mouseButton) && buttonState == KeyState.Press)
                return;

            if (pressedButtons.Contains(mouseButton))
            {
                if (buttonState == KeyState.Release)
                    pressedButtons.Remove(mouseButton);
            }
            else
            {
                if (buttonState == KeyState.Press)
                    pressedButtons.Add(mouseButton);
            }

            OnMouseClick?.Invoke(this, new MouseClickEventArgs(mouseButton, buttonState, HeldButtons));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method for handle mouse move press. </summary>
        /// <param name="point"> Current cursor position. </param>
        private void HandleMouseMove(POINT point)
        {
            if (mousePosition.X == point.X && mousePosition.Y == point.Y)
                return;

            OnMouseMove?.Invoke(this, new MouseMoveEventArgs(point, mousePosition));

            mousePosition = point;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method for handle mouse scroll. </summary>
        /// <param name="delta"> Scroll delta. </param>
        /// <param name="scrollOrientation"> Scroll orientation. </param>
        private void HandleMouseScroll(uint delta, ScrollOrientation scrollOrientation)
        {
            OnMouseScroll?.Invoke(this, new MouseScrollEventArgs((int)delta, scrollOrientation));
        }

        #endregion LISTENING METHODS

    }
}
