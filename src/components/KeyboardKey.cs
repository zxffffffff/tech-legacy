using Godot;
using System;

public partial class KeyboardKey : Area2D
{
	private AnimatedSprite2D KeySprite;
	private Label KeyLabel;

	private bool IsPressed = false;

	private string keyText = "?";

	[Export]
	public string KeyText
	{
		get { return keyText; }
		set { keyText = value; UpdateKeyLabel(); }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InputEvent += KeyInputEvent;

		KeySprite = GetNode<AnimatedSprite2D>("KeySprite");
		KeyLabel = KeySprite.GetNode<Label>("KeyLabel");

		UpdateKeyLabel();
	}

	private void UpdateKeyLabel()
	{
		if (KeyLabel == null)
			return;
		KeyLabel.Text = keyText;
		GD.Print($"[{KeyText}] UpdateKeyLabel");
	}

	private void UpdateKeyAnim()
	{
		if (KeySprite == null)
			return;
		KeySprite.Frame = IsPressed ? 3 : 0;
		KeyLabel.Position = new Vector2(KeyLabel.Position.X, KeyLabel.Position.Y + (IsPressed ? 4 : -4));
		GD.Print($"[{KeyText}] UpdateKeyAnim");
	}

	private void KeyInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event.IsPressed())
		{
			IsPressed = true;
			GD.Print($"[{KeyText}] KeyInputEvent - IsPressed");

			UpdateKeyAnim();
		}
		else if (IsPressed && @event.IsReleased())
		{
			IsPressed = false;
			GD.Print($"[{KeyText}] KeyInputEvent - IsReleased");

			UpdateKeyAnim();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
