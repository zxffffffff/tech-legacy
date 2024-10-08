using Godot;
using System;

namespace Utils
{
    public partial class Common : Node
    {
        [Signal]
        public delegate void CommonSignalEventHandler(string key, string value);

        // 单例模式 Godot autoload
        public static Common Instance { get; private set; }

        public override void _Ready()
        {
            Instance = this;
        }

        public void EmitCommonSignal(string key = "", string value = "")
        {
            EmitSignal(SignalName.CommonSignal, key, value);
        }
    }
}
