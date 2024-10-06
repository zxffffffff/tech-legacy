using Godot;
using System;
using System.Collections.Generic;

namespace Utils
{
    public enum KeyboardType
    {
        MiniKeyboard,
        FullKeyboard,
    }

    public class Keyboard
    {
        static public List<List<KeyboardKey>> MiniKeyboard()
        {
            var keys = new List<List<KeyboardKey>>
            {
                new List<KeyboardKey>()
                {
                    new KeyboardKey(Key.Escape, 1.5f),
                    Key.Key1, Key.Key2, Key.Key3, Key.Key4, Key.Key5, Key.Key6, Key.Key7, Key.Key8, Key.Key9, Key.Key0,
                    Key.Minus, Key.Equal,
                    new KeyboardKey(Key.Backspace, 2)
                },
                new List<KeyboardKey>()
                {
                    new KeyboardKey(Key.Tab, 2),
                    Key.Q, Key.W, Key.E, Key.R, Key.T, Key.Y, Key.U, Key.I, Key.O, Key.P,
                    Key.Bracketleft, Key.Bracketright,
                    new KeyboardKey(Key.Backslash, 1.5f)
                },
                new List<KeyboardKey>()
                {
                    new KeyboardKey(Key.Capslock, 2),
                    Key.A, Key.S, Key.D, Key.F, Key.G,Key.H, Key.J, Key.K, Key.L,
                    Key.Semicolon, Key.Apostrophe,
                    new KeyboardKey(Key.Enter, 2.5f)
                },
                new List<KeyboardKey>()
                {
                    new KeyboardKey(Key.Shift, 2.8f),
                    Key.Z, Key.X, Key.C, Key.V, Key.B, Key.N, Key.M,
                    Key.Comma, Key.Period, Key.Slash,
                    new KeyboardKey(Key.Shift, 2.8f)
                },
                new List<KeyboardKey>()
                {
                    new KeyboardKey(Key.Ctrl, 2f),
                    new KeyboardKey(Key.Alt, 1.5f),
                    new KeyboardKey(Key.Meta, 1.5f),
                    new KeyboardKey(Key.Space, 7.75f),
                    new KeyboardKey(Key.Alt, 1.5f),
                    new KeyboardKey(Key.Ctrl, 2f),
                }
            };
            return keys;
        }
    }
}
