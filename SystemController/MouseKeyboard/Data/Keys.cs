using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SystemController.MouseKeyboard.Data
{
    public static class Keys
    {

        //  CONST

        public const byte VK_LBUTTON = 0x01;                //  Left mouse button.
        public const byte VK_RBUTTON = 0x02;                //  Right mouse button.
        public const byte VK_CANCEL = 0x03;                 //  Control-break processing.
        public const byte VK_MBUTTON = 0x04;                //  Middle mouse button.
        public const byte VK_XBUTTON1 = 0x05;               //  X1 mouse button.
        public const byte VK_XBUTTON2 = 0x06;               //  X2 mouse button.
        public const byte VK_BACK = 0x08;                   //  Bakspace key.
        public const byte VK_TAB = 0x09;                    //  Tab key.
        public const byte VK_CLEAR = 0x0C;                  //  Clear key.
        public const byte VK_RETURN = 0x0D;                 //  Enter key.
        public const byte VK_SHIFT = 0x10;                  //  Shift key.
        public const byte VK_CONTROL = 0x11;                //  Ctrl key.
        public const byte VK_MENU = 0x12;                   //  Alt key.
        public const byte VK_PAUSE = 0x13;                  //  Pause key.
        public const byte VK_CAPITAL = 0x14;                //  CapsLock key.
        public const byte VK_KANA = 0x15;                   //  IME Kana mode.
        public const byte VK_HANGUL = 0x15;                 //  IME Hangul mode.
        public const byte VK_IME_ON = 0x16;                 //  IME On.
        public const byte VK_JUNJA = 0x17;                  //  IME Junja mode.
        public const byte VK_FINAL = 0x18;                  //  IME final mode.
        public const byte VK_HANJA = 0x19;                  //  IME Hanja mode.
        public const byte VK_KANJI = 0x19;                  //  IME Kanji mode.
        public const byte VK_IME_OFF = 0x1A;                //  IME Off.
        public const byte VK_ESCAPE = 0x1B;                 //  Escape key.
        public const byte VK_CONVERT = 0x1C;                //  IME convert.
        public const byte VK_NONCONVERT = 0x1D;             //  IME nonconvert.
        public const byte VK_ACCEPT = 0x1E;                 //  IME accept.
        public const byte VK_MODECHANGE = 0x1F;             //  IME mode change request.
        public const byte VK_SPACE = 0x20;                  //  Spacebar.
        public const byte VK_PRIOR = 0x21;                  //  PageUp key.
        public const byte VK_NEXT = 0x22;                   //  PageDown key.
        public const byte VK_END = 0x23;                    //  End key.
        public const byte VK_HOME = 0x24;                   //  Home key.
        public const byte VK_LEFT = 0x25;                   //  Left Arrow key.
        public const byte VK_UP = 0x26;                     //  Up Arrow key.
        public const byte VK_RIGHT = 0x27;                  //  Right Arrow key.
        public const byte VK_DOWN = 0x28;                   //  Down Arrow key.
        public const byte VK_SELECT = 0x29;                 //  Select key.
        public const byte VK_PRbyte = 0x2A;                 //  Prbyte key.
        public const byte VK_EXECUTE = 0x2B;                //  Execute key.
        public const byte VK_SNAPSHOT = 0x2C;               //  PrintScreen key.
        public const byte VK_INSERT = 0x2D;                 //  Insert key.
        public const byte VK_DELETE = 0x2E;                 //  Delete key.
        public const byte VK_HELP = 0x2F;                   //  Help key.
        public const byte VK_0 = 0x30;                      //  0 key.
        public const byte VK_1 = 0x31;                      //  1 key.
        public const byte VK_2 = 0x32;                      //  2 key.
        public const byte VK_3 = 0x33;                      //  3 key.
        public const byte VK_4 = 0x34;                      //  4 key.
        public const byte VK_5 = 0x35;                      //  5 key.
        public const byte VK_6 = 0x36;                      //  6 key.
        public const byte VK_7 = 0x37;                      //  7 key.
        public const byte VK_8 = 0x38;                      //  8 key.
        public const byte VK_9 = 0x39;                      //  9 key.
        public const byte VK_A = 0x41;                      //  A key.
        public const byte VK_B = 0x42;                      //  B key.
        public const byte VK_C = 0x43;                      //  C key.
        public const byte VK_D = 0x44;                      //  D key.
        public const byte VK_E = 0x45;                      //  E key.
        public const byte VK_F = 0x46;                      //  F key.
        public const byte VK_G = 0x47;                      //  G key.
        public const byte VK_H = 0x48;                      //  H key.
        public const byte VK_I = 0x49;                      //  I key.
        public const byte VK_J = 0x4A;                      //  J key.
        public const byte VK_K = 0x4B;                      //  K key.
        public const byte VK_L = 0x4C;                      //  L key.
        public const byte VK_M = 0x4D;                      //  M key.
        public const byte VK_N = 0x4E;                      //  N key.
        public const byte VK_O = 0x4F;                      //  O key.
        public const byte VK_P = 0x50;                      //  P key.
        public const byte VK_Q = 0x51;                      //  Q key.
        public const byte VK_R = 0x52;                      //  R key.
        public const byte VK_S = 0x53;                      //  S key.
        public const byte VK_T = 0x54;                      //  T key.
        public const byte VK_U = 0x55;                      //  U key.
        public const byte VK_V = 0x56;                      //  V key.
        public const byte VK_W = 0x57;                      //  W key.
        public const byte VK_X = 0x58;                      //  X key.
        public const byte VK_Y = 0x59;                      //  Y key.
        public const byte VK_Z = 0x5A;                      //  Z key.
        public const byte VK_LWIN = 0x5B;                   //  Left Windows key.
        public const byte VK_RWIN = 0x5C;                   //  Right Windows key.
        public const byte VK_APPS = 0x5D;                   //  Applications key.
        public const byte VK_SLEEP = 0x5F;                  //  Computer Sleep key.
        public const byte VK_NUMPAD0 = 0x60;                //  Numeric keypad 0 key.
        public const byte VK_NUMPAD1 = 0x61;                //  Numeric keypad 1 key.
        public const byte VK_NUMPAD2 = 0x62;                //  Numeric keypad 2 key.
        public const byte VK_NUMPAD3 = 0x63;                //  Numeric keypad 3 key.
        public const byte VK_NUMPAD4 = 0x64;                //  Numeric keypad 4 key.
        public const byte VK_NUMPAD5 = 0x65;                //  Numeric keypad 5 key.
        public const byte VK_NUMPAD6 = 0x66;                //  Numeric keypad 6 key.
        public const byte VK_NUMPAD7 = 0x67;                //  Numeric keypad 7 key.
        public const byte VK_NUMPAD8 = 0x68;                //  Numeric keypad 8 key.
        public const byte VK_NUMPAD9 = 0x69;                //  Numeric keypad 9 key.
        public const byte VK_MULTIPLY = 0x6A;               //  * key.
        public const byte VK_ADD = 0x6B;                    //  + key.
        public const byte VK_SEPARATOR = 0x6C;              //  Separator key.
        public const byte VK_SUBTRACT = 0x6D;               //  - key.
        public const byte VK_DECIMAL = 0x6E;                //  Decimal key.
        public const byte VK_DIVIDE = 0x6F;                 //  / key.
        public const byte VK_F1 = 0x70;                     //  F1 key.
        public const byte VK_F2 = 0x71;                     //  F2 key.
        public const byte VK_F3 = 0x72;                     //  F3 key.
        public const byte VK_F4 = 0x73;                     //  F4 key.
        public const byte VK_F5 = 0x74;                     //  F5 key.
        public const byte VK_F6 = 0x75;                     //  F6 key.
        public const byte VK_F7 = 0x76;                     //  F7 key.
        public const byte VK_F8 = 0x77;                     //  F8 key.
        public const byte VK_F9 = 0x78;                     //  F9 key.
        public const byte VK_F10 = 0x79;                    //  F10 key.
        public const byte VK_F11 = 0x7A;                    //  F11 key.
        public const byte VK_F12 = 0x7B;                    //  F12 key.
        public const byte VK_F13 = 0x7C;                    //  F13 key.
        public const byte VK_F14 = 0x7D;                    //  F14 key.
        public const byte VK_F15 = 0x7E;                    //  F15 key.
        public const byte VK_F16 = 0x7F;                    //  F16 key.
        public const byte VK_F17 = 0x80;                    //  F17 key.
        public const byte VK_F18 = 0x81;                    //  F18 key.
        public const byte VK_F19 = 0x82;                    //  F19 key.
        public const byte VK_F20 = 0x83;                    //  F20 key.
        public const byte VK_F21 = 0x84;                    //  F21 key.
        public const byte VK_F22 = 0x85;                    //  F22 key.
        public const byte VK_F23 = 0x86;                    //  F23 key.
        public const byte VK_F24 = 0x87;                    //  F24 key.
        public const byte VK_NUMLOCK = 0x90;                //  NumLock key.
        public const byte VK_SCROLL = 0x91;                 //  ScrollLock key.
        public const byte VK_LSHIFT = 0xA0;                 //  Left Shift key.
        public const byte VK_RSHIFT = 0xA1;                 //  Right Shift key.
        public const byte VK_LCONTROL = 0xA2;               //  Left Control key.
        public const byte VK_RCONTROL = 0xA3;               //  Right Control key.
        public const byte VK_LMENU = 0xA4;                  //  Left Alt key.
        public const byte VK_RMENU = 0xA5;                  //  Right Alt key.
        public const byte VK_BROWSER_BACK = 0xA6;           //  Browser back key.
        public const byte VK_BROWSER_FORWARD = 0xA7;        //  Browser forward key.
        public const byte VK_BROWSER_REFRESH = 0xA8;        //  Browser refresh key.
        public const byte VK_BROWSER_STOP = 0xA9;           //  Browser stop key.
        public const byte VK_BROWSER_SEARCH = 0xAA;         //  Browser search key.
        public const byte VK_BROWSER_FAVORITES = 0xAB;      //  Browser favorites key.
        public const byte VK_BROWSER_HOME = 0xAC;           //  Browser start/home key.
        public const byte VK_VOLUME_MUTE = 0xAD;            //  Volume mute key.
        public const byte VK_VOLUME_DOWN = 0xAE;            //  Volume down key.
        public const byte VK_VOLUME_UP = 0xAF;              //  Volume up key.
        public const byte VK_MEDIA_NEXT_TRACK = 0xB0;       //  Next track key.
        public const byte VK_MEDIA_PREV_TRACK = 0xB1;       //  Previous track key.
        public const byte VK_MEDIA_STOP = 0xB2;             //  Stop media key.
        public const byte VK_MEDIA_PLAY_PAUSE = 0xB3;       //  Play/Pause media key
        public const byte VK_LAUNCH_MAIL = 0xB4;            //  Start mail key.
        public const byte VK_LAUNCH_MEDIA_SELECT = 0xB5;    //  Select media key.
        public const byte VK_LAUNCH_APP1 = 0xB6;            //  Start application 1 key.
        public const byte VK_LAUNCH_APP2 = 0xB7;            //  Start application 2 key.
        public const byte VK_OEM_1 = 0xBA;                  //  For the US standard keyboard, the ;: key.
        public const byte VK_OEM_PLUS = 0xBB;               //  For any country/region, the + key.
        public const byte VK_OEM_COMMA = 0xBC;              //  For any country/region, the , key.
        public const byte VK_OEM_MINUS = 0xBD;              //  For any country/region, the - key.
        public const byte VK_OEM_PERIOD = 0xBE;             //  For any country/region, the . key.
        public const byte VK_OEM_2 = 0xBF;                  //  For the US standard keyboard, the /? key.
        public const byte VK_OEM_3 = 0xC0;                  //  For the US standard keyboard, the `~ key.
        public const byte VK_OEM_4 = 0xDB;                  //  For the US standard keyboard, the [{ key.
        public const byte VK_OEM_5 = 0xDC;                  //  For the US standard keyboard, the \| key.
        public const byte VK_OEM_6 = 0xDD;                  //  For the US standard keyboard, the ]} key.
        public const byte VK_OEM_7 = 0xDE;                  //  For the US standard keyboard, the '" key.
        public const byte VK_OEM_8 = 0xDF;                  //  Used for miscellaneous characters; it can vary by keyboard.
        public const byte VK_OEM_102 = 0xE2;                //  The<> keys on the US standard keyboard, or the \\| key on the non-US 102-key keyboard.
        public const byte VK_PROCESSKEY = 0xE5;             //  IME process key.
        public const byte VK_PACKET = 0xE7;                 //  Used to pass Unicode characters as if they were keystrokes. The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods.
        public const byte VK_ATTN = 0xF6;                   //  Attn key.
        public const byte VK_CRSEL = 0xF7;                  //  CrSel key.
        public const byte VK_EXSEL = 0xF8;                  //  ExSel key.
        public const byte VK_EREOF = 0xF9;                  //  Erase EOF key.
        public const byte VK_PLAY = 0xFA;                   //  Play key.
        public const byte VK_ZOOM = 0xFB;                   //  Zoom key.
        public const byte VK_NONAME = 0xFC;                 //  Reserved.
        public const byte VK_PA1 = 0xFD;                    //  PA1 key.
        public const byte VK_OEM_CLEAR = 0xFE;              //  Clear key.

        public const int KEYEVENTF_KEYDOWN = 0x0001;
        public const int KEYEVENTF_KEYUP = 0x0002;

    }
}
