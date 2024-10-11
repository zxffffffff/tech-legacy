using Godot;
using System;

namespace Utils
{
    /// <summary>
    /// 单例模式 static new
    /// </summary>
    public partial class EventMgr
    {
        private static readonly EventMgr _instance = new EventMgr();

        public static EventMgr Instance { get { return _instance; } }

        public delegate void KeyPressEvent(bool isPressed, Key keyCode);
    }
}
