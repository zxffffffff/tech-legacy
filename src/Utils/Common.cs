using Godot;
using System;

namespace Utils
{
    public partial class Common : Node
    {
        // 单例模式 Godot autoload
        public static Common Instance { get; private set; }

        public override void _Ready()
        {
            Instance = this;
        }

        [Signal]
        public delegate void CommonSignalEventHandler(string key, string value);

        public void EmitCommonSignal(string key = "", string value = "")
        {
            EmitSignal(SignalName.CommonSignal, key, value);
        }
    }
}
