using Godot;
using System;

namespace Utils
{
    public partial class EventBus : Node
    {
        // 单例模式 Godot autoload
        public static EventBus Instance { get; private set; }

        public override void _Ready()
        {
            Instance = this;
        }

        public delegate void KeyPressEvent(bool isPressed, Key keyCode);

        public event KeyPressEvent KeyPress;

        public void EmitKeyPress(bool isPressed, Key keyCode)
        {
            KeyPress(isPressed, keyCode);
        }
    }
}
