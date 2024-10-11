using Godot;
using System;

namespace Utils
{
    /// <summary>
    /// 单例模式 Godot autoload
    /// </summary>
    public partial class Common : Node
    {
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
