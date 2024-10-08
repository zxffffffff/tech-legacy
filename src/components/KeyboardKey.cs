using Godot;
using System;

[Tool]
public partial class KeyboardKey : Area2D
{
	private AnimatedSprite2D keySprite;

	private Label keyLabel;

	private CollisionShape2D collisionShape2D;

	private Godot.Key keyCode;

	private float keyWidth;

	private bool isPressed = false;

	[Export]
	public Godot.Key KeyCode
	{
		get { return keyCode; }
		set { keyCode = value; UpdateKeyUI(); }
	}

	[Export]
	public float KeyWidth
	{
		get { return keyWidth; }
		set { keyWidth = value; UpdateKeyUI(); }
	}

	[Export]
	public bool IsPressed
	{
		get { return isPressed; }
		set { isPressed = value; UpdateKeyAnim(); }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		InputEvent += KeyInputEvent;

		keySprite = GetNode<AnimatedSprite2D>("KeySprite");
		keyLabel = GetNode<Label>("KeyLabel");
		collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

		UpdateKeyUI();
	}

	private void UpdateKeyUI()
	{
		if (keySprite == null)
			return;
		var new_scale = new Vector2(KeyWidth, keySprite.Scale.Y);
		keySprite.Scale = new_scale;
		collisionShape2D.Scale = new_scale;
		keyLabel.Text = Utils.KeyboardKey.KeyCodeToString(KeyCode);
		this.Visible = keyCode != Key.None;
	}

	private void UpdateKeyAnim()
	{
		if (keySprite == null)
			return;
		var frame = IsPressed ? 3 : 0;
		if (keySprite.Frame == frame)
			return;
		keySprite.Frame = frame;
		keyLabel.Position = new Vector2(keyLabel.Position.X, keyLabel.Position.Y + (IsPressed ? 4 : -4));
	}

	private void KeyInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouse mouseEvent)
		{
			if (mouseEvent.IsPressed())
			{
				// GD.Print($"{KeyCode} mouseEvent.IsPressed");
				IsPressed = true;
			}
			else if (isPressed && mouseEvent.IsReleased())
			{
				// GD.Print($"{KeyCode} mouseEvent.IsReleased");
				IsPressed = false;
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Keycode == KeyCode)
		{
			if (keyEvent.IsPressed())
			{
				// GD.Print($"{KeyCode} keyEvent.IsPressed");
				IsPressed = true;
			}
			else if (isPressed && keyEvent.IsReleased())
			{
				// GD.Print($"{KeyCode} keyEvent.IsReleased");
				IsPressed = false;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
