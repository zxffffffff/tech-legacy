using Godot;
using System;
using System.Collections.Generic;

namespace Utils
{
    public enum KeyboardType
    {
        MiniKeyboard,
        FullKeyboard,
        LettersOnly,
        LettersAndPunctuation,
    }

    public class KeyboardMgr
    {
        static public List<List<KeyboardKeyMgr>> MiniKeyboard()
        {
            var keys = new List<List<KeyboardKeyMgr>>
            {
                new List<KeyboardKeyMgr>()
                {
                    new KeyboardKeyMgr(Key.Escape, 1.5f),
                    Key.Key1, Key.Key2, Key.Key3, Key.Key4, Key.Key5, Key.Key6, Key.Key7, Key.Key8, Key.Key9, Key.Key0,
                    Key.Minus, Key.Equal,
                    new KeyboardKeyMgr(Key.Backspace, 2)
                },
                new List<KeyboardKeyMgr>()
                {
                    new KeyboardKeyMgr(Key.Tab, 1.8f),
                    Key.Q, Key.W, Key.E, Key.R, Key.T, Key.Y, Key.U, Key.I, Key.O, Key.P,
                    Key.Bracketleft, Key.Bracketright,
                    new KeyboardKeyMgr(Key.Backslash, 1.6f)
                },
                new List<KeyboardKeyMgr>()
                {
                    new KeyboardKeyMgr(Key.Capslock, 2f),
                    Key.A, Key.S, Key.D, Key.F, Key.G, Key.H, Key.J, Key.K, Key.L,
                    Key.Semicolon, Key.Apostrophe,
                    new KeyboardKeyMgr(Key.Enter, 2.5f)
                },
                new List<KeyboardKeyMgr>()
                {
                    new KeyboardKeyMgr(Key.Shift, 2.7f),
                    Key.Z, Key.X, Key.C, Key.V, Key.B, Key.N, Key.M,
                    Key.Comma, Key.Period, Key.Slash,
                    new KeyboardKeyMgr(Key.Shift, 2.9f)
                },
                new List<KeyboardKeyMgr>()
                {
                    new KeyboardKeyMgr(Key.Ctrl, 2f),
                    new KeyboardKeyMgr(Key.Alt, 1.5f),
                    new KeyboardKeyMgr(Key.Meta, 1.5f),
                    new KeyboardKeyMgr(Key.Space, 7.75f),
                    new KeyboardKeyMgr(Key.Alt, 1.5f),
                    new KeyboardKeyMgr(Key.Ctrl, 2f),
                }
            };
            return keys;
        }

        static public List<List<KeyboardKeyMgr>> FullKeyboard()
        {
            var keys = new List<List<KeyboardKeyMgr>>();
            // Todo
            return keys;
        }

        static public List<List<KeyboardKeyMgr>> LettersOnly()
        {
            var keys = new List<List<KeyboardKeyMgr>>
            {
                new List<KeyboardKeyMgr>()
                {
                    Key.Q, Key.W, Key.E, Key.R, Key.T, Key.Y, Key.U, Key.I, Key.O, Key.P,
                },
                new List<KeyboardKeyMgr>()
                {
                    Key.A, Key.S, Key.D, Key.F, Key.G, Key.H, Key.J, Key.K, Key.L,
                    new KeyboardKeyMgr(Key.None, 0.5f),
                },
                new List<KeyboardKeyMgr>()
                {
                    Key.Z, Key.X, Key.C, Key.V, Key.B, Key.N, Key.M,
                    new KeyboardKeyMgr(Key.None, 1.6f),
                }
            };
            return keys;
        }

        static public List<List<KeyboardKeyMgr>> LettersAndPunctuation()
        {
            var keys = new List<List<KeyboardKeyMgr>>
            {
                new List<KeyboardKeyMgr>()
                {
                    Key.Quoteleft,
                    Key.Key1, Key.Key2, Key.Key3, Key.Key4, Key.Key5, Key.Key6, Key.Key7, Key.Key8, Key.Key9, Key.Key0,
                    Key.Minus, Key.Equal,
                    new KeyboardKeyMgr(Key.Backspace, 2.4f)
                },
                new List<KeyboardKeyMgr>()
                {
                    new KeyboardKeyMgr(Key.Tab, 1.8f),
                    Key.Q, Key.W, Key.E, Key.R, Key.T, Key.Y, Key.U, Key.I, Key.O, Key.P,
                    Key.Bracketleft, Key.Bracketright,
                    new KeyboardKeyMgr(Key.Backslash, 1.6f)
                },
                new List<KeyboardKeyMgr>()
                {
                    new KeyboardKeyMgr(Key.Capslock, 2f),
                    Key.A, Key.S, Key.D, Key.F, Key.G, Key.H, Key.J, Key.K, Key.L,
                    Key.Semicolon, Key.Apostrophe,
                    new KeyboardKeyMgr(Key.Enter, 2.5f)
                },
                new List<KeyboardKeyMgr>()
                {
                    new KeyboardKeyMgr(Key.Shift, 2.7f),
                    Key.Z, Key.X, Key.C, Key.V, Key.B, Key.N, Key.M,
                    Key.Comma, Key.Period, Key.Slash,
                    new KeyboardKeyMgr(Key.Shift, 2.9f)
                }
            };
            return keys;
        }
    }
}
