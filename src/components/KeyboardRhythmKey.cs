using Godot;
using System;
using Utils;

[Tool]
public partial class KeyboardRhythmKey : Area2D
{
    [Export]
    public AnimatedSprite2D KeySprite;

    [Export]
    public Label KeyLabel;

    [Export]
    public double Progress { get; set; } = 0;
}
