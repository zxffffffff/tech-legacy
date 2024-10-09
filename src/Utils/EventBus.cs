using Godot;
using System;

namespace Utils
{
    public partial class EventBus
    {
        // 单例模式
        private static readonly EventBus _instance = new EventBus();

        public static EventBus Instance { get { return _instance; } }

        public delegate void KeyPressEvent(bool isPressed, Key keyCode);

        public event KeyPressEvent KeyPress;

        public void EmitKeyPress(bool isPressed, Key keyCode)
        {
            KeyPress(isPressed, keyCode);
        }
    }
}
