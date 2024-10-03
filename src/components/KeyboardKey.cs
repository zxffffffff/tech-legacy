using Godot;
using System;

[Tool]
public partial class KeyboardKey : Area2D
{
	private AnimatedSprite2D keySprite;

	private Label keyLabel;

	private CollisionShape2D collisionShape2D;

	private string keyText;

	private float keyWidth;

	[Export(PropertyHint.MultilineText)]
	public string KeyText
	{
		get { return keyText; }
		set { keyText = value; UpdateKeyUI(); }
	}

	[Export]
	public float KeyWidth
	{
		get { return keyWidth; }
		set { keyWidth = value; UpdateKeyUI(); }
	}

	private bool isPressed = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InputEvent += KeyInputEvent;

		keySprite = GetNode<AnimatedSprite2D>("KeySprite");
		keyLabel = GetNode<Label>("KeyLabel");
		collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

		UpdateKeyUI();
	}

	private void UpdateKeyUI()
	{
		if (keySprite == null || keyLabel == null)
			return;
		var new_scale = new Vector2(KeyWidth, keySprite.Scale.Y);
		keySprite.Scale = new_scale;
		collisionShape2D.Scale = new_scale;
		keyLabel.Text = KeyText;
	}

	private void UpdateKeyAnim()
	{
		if (keySprite == null)
			return;
		var frame = isPressed ? 3 : 0;
		if (keySprite.Frame == frame)
			return;
		keySprite.Frame = frame;
		keyLabel.Position = new Vector2(keyLabel.Position.X, keyLabel.Position.Y + (isPressed ? 4 : -4));
	}

	private void KeyInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event.IsPressed())
		{
			GD.Print($"{KeyText} KeyInputEvent - IsPressed");

			isPressed = true;
			UpdateKeyAnim();
		}
		else if (isPressed && @event.IsReleased())
		{
			GD.Print($"{KeyText} KeyInputEvent - IsReleased");

			isPressed = false;
			UpdateKeyAnim();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
