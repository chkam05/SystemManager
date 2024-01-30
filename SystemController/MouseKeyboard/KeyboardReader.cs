using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SystemController.MouseKeyboard.Data;
using SystemController.MouseKeyboard.Events;

namespace SystemController.MouseKeyboard
{
    public class KeyboardReader
    {

        //  CONST

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const string ERROR_MESSAGE = "An exception was thrown at \"{0}\" with the message: {1}";


        //  DELEGATES

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private LowLevelKeyboardProc _handler;

        public delegate void KeyPressedEventHandler(object sender, Events.KeyPressEventArgs e);
        public delegate void KeysListeningFinishedEventHandler(object sender, KeysListeningFinishedEventArgs e);
        public delegate void ThrowExceptionEventHandler(object sender, ThrownExceptionEventArgs e);


        //  EVENTS

        public KeyPressedEventHandler? OnKeyPress;
        public KeysListeningFinishedEventHandler? OnStopListening;
        public ThrowExceptionEventHandler? OnThrownException;


        //  IMPORTS

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);


        //  VARIABLES

        private IntPtr _hookId = IntPtr.Zero;

        private List<Exception> _exceptions;
        private List<byte> _pressedKeys;

        private object _exceptionsLock = new object();
        private object _pressedKeysLock = new object();

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

        private List<byte> pressedKeys
        {
            get
            {
                lock (_pressedKeysLock)
                {
                    return _pressedKeys;
                }
            }
        }

        public int HeldKeys
        {
            get => pressedKeys.Count;
        }



        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> KeyboardReader class constructor. </summary>
        public KeyboardReader()
        {
            IsListening = false;

            _exceptions = new List<Exception>();
            _pressedKeys = new List<byte>();
        }

        #endregion CLASS METHODS

        #region HOOK METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Set keyboard hook. </summary>
        private void SetHook()
        {
            try
            {
                IntPtr moduleHandle = IntPtr.Zero;

                _handler = HookCallback;

                /*using (var process = System.Diagnostics.Process.GetCurrentProcess())
                {
                    using (var module = process.MainModule)
                    {
                        moduleHandle = GetModuleHandle(module?.ModuleName);
                    }
                }*/

                _hookId = SetWindowsHookEx(WH_KEYBOARD_LL, _handler, IntPtr.Zero, 0);

                if (_hookId == IntPtr.Zero)
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            catch (Exception innerException)
            {
                HandleException(innerException, nameof(SetHook));
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Unset keyboard hook. </summary>
        private void UnHook()
        {
            try
            {
                if (_hookId != IntPtr.Zero)
                {
                    UnhookWindowsHookEx(_hookId);
                    _hookId = IntPtr.Zero;
                }
            }
            catch (Exception innerException)
            {
                HandleException(innerException, nameof(UnHook));
            }
        }

        #endregion HOOK METHODS

        #region INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Get list of thrown exceptions from last run. </summary>
        /// <returns> List of thrown exceptions. </returns>
        public List<Exception> GetThrownExceptions()
        {
            return exceptions;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Start listening keyboard. </summary>
        public void StartListening()
        {
            pressedKeys.Clear();

            IsListening = true;
            SetHook();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop listening keyboard. </summary>
        public void StopListening()
        {
            UnHook();
            IsListening = false;
            OnStopListening?.Invoke(this, new KeysListeningFinishedEventArgs(exceptions, HeldKeys));
        }

        #endregion INTERACTION METHODS

        #region LISTENING METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after pressing keys on keyboard. </summary>
        /// <param name="nCode"> Status code that indicates whether the data has been processed by the hook procedure. </param>
        /// <param name="wParam"> Key message type. </param>
        /// <param name="lParam"> Pointer to a structure containing data about the key. </param>
        /// <returns> Value returned by the string procedure and should also be returned by CallNextHookEx. </returns>
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYUP))
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    if (wParam == (IntPtr)WM_KEYDOWN)
                    {
                        HandleKey((byte)vkCode, KeyState.Press);
                    }
                    else if (wParam == (IntPtr)WM_KEYUP)
                    {
                        HandleKey((byte)vkCode, KeyState.Release);
                    }
                }

                return CallNextHookEx(_hookId, nCode, wParam, lParam);
            }
            catch (Exception innerException)
            {
                HandleException(innerException, nameof(HookCallback));

                StopListening();

                return IntPtr.Zero;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Handle thrown exception. </summary>
        /// <param name="thrownException"> Thrown exception. </param>
        /// <param name="methodName"> Method from which method has been thrown. </param>
        private void HandleException(Exception thrownException, string methodName = null)
        {
            var message = string.Format(ERROR_MESSAGE, methodName, thrownException.Message);
            var exception = new Exception(message, thrownException);

            exceptions.Add(exception);

            if (OnThrownException != null)
                OnThrownException.Invoke(this, new ThrownExceptionEventArgs(exception));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method for handle keys. </summary>
        /// <param name="keyCode"> Key code. </param>
        /// <param name="keyState"> Key state. </param>
        private void HandleKey(byte keyCode, KeyState keyState)
        {
            if (pressedKeys.Contains(keyCode) && keyState == KeyState.Press)
                return;

            if (pressedKeys.Contains(keyCode))
            {
                if (keyState == KeyState.Release)
                    pressedKeys.Remove(keyCode);
            }
            else
            {
                if (keyState == KeyState.Press)
                    pressedKeys.Add(keyCode);
            }

            OnKeyPress?.Invoke(this, new Events.KeyPressEventArgs(keyCode, pressedKeys.ToArray(), keyState, HeldKeys));
        }

        #endregion LISTENING METHODS

    }
}
