using Godot;
using System;

public partial class KeyboardKey : Node2D
{
	Area2D area2D;
	AnimatedSprite2D key;

	bool IsPressed = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		area2D = GetNode<Area2D>("area2D");

		key = GetNode<AnimatedSprite2D>("key");
		key.SetFrameAndProgress(3, 0);
	}

	public void KeyInputEvent(Viewport viewport, InputEvent inputEvent, int index)
	{
		// TODO mouse input and key input
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
