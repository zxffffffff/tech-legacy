using Godot;
using System;
using System.Runtime.InteropServices;

namespace Utils
{
    public readonly struct KeyboardKeyMgr
    {
        public KeyboardKeyMgr(Key KeyCode, float KeyWidth = 1)
        {
            this.KeyCode = KeyCode;
            this.KeyWidth = KeyWidth;
        }

        // 隐式转换
        public static implicit operator KeyboardKeyMgr(Key keyCode)
        {
            return new KeyboardKeyMgr(keyCode);
        }

        public Key KeyCode { get; init; }
        public float KeyWidth { get; init; }

        public static string KeyCodeToString(Key KeyCode)
        {
            switch (KeyCode)
            {
                case Key.Escape: return "Esc";
                case Key.Quoteleft: return "`~";
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
                // Todo
            }
            return nameof(KeyCode);
        }

        public static Key StringToKeyCode(string key)
        {
            switch (key)
            {
                case "Esc": return Key.Escape;
                case "`~": return Key.Quoteleft;
                case "1!": return Key.Key1;
                case "2@": return Key.Key2;
                case "3#": return Key.Key3;
                case "4$": return Key.Key4;
                case "5%": return Key.Key5;
                case "6^": return Key.Key6;
                case "7&": return Key.Key7;
                case "8*": return Key.Key8;
                case "9(": return Key.Key9;
                case "0)": return Key.Key0;
                case "-_": return Key.Minus;
                case "=+": return Key.Equal;
                case "Back": return Key.Backspace;
                case "[{": return Key.Bracketleft;
                case "]}": return Key.Bracketright;
                case "\\|": return Key.Backslash;
                case "Caps": return Key.Capslock;
                case ";:": return Key.Semicolon;
                case "'\"": return Key.Apostrophe;
                case ",<": return Key.Comma;
                case ".>": return Key.Period;
                case "/?": return Key.Slash;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                switch (key)
                {
                    case "Opt": return Key.Alt;
                    case "Cmd": return Key.Meta;
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Todo
            }
            return Enum.Parse<Key>(key);
        }
    }
}
