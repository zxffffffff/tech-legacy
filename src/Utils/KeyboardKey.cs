using Godot;
using System;
using System.Runtime.InteropServices;

namespace Utils
{
    public readonly struct KeyboardKey
    {
        public KeyboardKey(Godot.Key KeyCode, float KeyWidth = 1)
        {
            this.KeyCode = KeyCode;
            this.KeyWidth = KeyWidth;
        }

        //隐式转换
        public static implicit operator KeyboardKey(Godot.Key KeyEnum)
        {
            return new KeyboardKey(KeyEnum);
        }

        public Godot.Key KeyCode { get; init; }
        public float KeyWidth { get; init; }

        public static string KeyEnumToString(Godot.Key KeyCode)
        {
            switch (KeyCode)
            {
                case Key.Escape: return "Esc";
                case Key.Key1: return "1!";
                case Key.Key2: return "2@";
                case Key.Key3: return "3#";
                case Key.Key4: return "4$";
                case Key.Key5: return "5%";
                case Key.Key6: return "6^";
                case Key.Key7: return "7&";
                case Key.Key8: return "8*";
                case Key.Key9: return "9(";
                case Key.Key0: return "0)";
                case Key.Minus: return "-_";
                case Key.Equal: return "=+";
                case Key.Backspace: return "Back";
                case Key.Bracketleft: return "[{";
                case Key.Bracketright: return "]}";
                case Key.Backslash: return "\\|";
                case Key.Capslock: return "Caps";
                case Key.Semicolon: return ";:";
                case Key.Apostrophe: return "'\"";
                case Key.Comma: return ",<";
                case Key.Period: return ".>";
                case Key.Slash: return "/?";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                switch (KeyCode)
                {
                    case Key.Alt: return "Opt";
                    case Key.Meta: return "Cmd";
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {

            }
            return KeyCode.ToString();
        }
    }
}
