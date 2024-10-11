using Godot;
using System;
using Utils;

[Tool]
public partial class KeyboardKey : Area2D
{
	[Export]
	public AnimatedSprite2D KeySprite;

	[Export]
	public Label KeyLabel;

	[Export]
	public CollisionShape2D KeyShape;

	private Godot.Key _keyCode;

	[Export]
	public Godot.Key KeyCode
	{
		get { return _keyCode; }
		set { _keyCode = value; UpdateKeyUI(); }
	}

	private float _keyWidth;

	[Export]
	public float KeyWidth
	{
		get { return _keyWidth; }
		set { _keyWidth = value; UpdateKeyUI(); }
	}

	private bool _isPressed = false;

	[Export]
	public bool IsPressed
	{
		get { return _isPressed; }
		set { _isPressed = value; UpdateKeyAnim(); }
	}

	[Export]
	public bool EnableInput { get; set; } = true;

	public event EventMgr.KeyPressEvent KeyPressCbk;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		InputEvent += CollisionInput;
		UpdateKeyUI();
	}

	private void UpdateKeyUI()
	{
		if (KeySprite == null)
			return;
		var new_scale = new Vector2(KeyWidth, KeySprite.Scale.Y);
		KeySprite.Scale = new_scale;
		KeyShape.Scale = new_scale;
		KeyLabel.Text = KeyboardKeyMgr.KeyCodeToString(KeyCode);
		this.Visible = KeyCode != Key.None;
	}

	private void UpdateKeyAnim()
	{
		if (KeySprite == null)
			return;
		var frame = IsPressed ? 3 : 0;
		if (KeySprite.Frame == frame)
			return;
		KeySprite.Frame = frame;
		KeyLabel.Position = new Vector2(KeyLabel.Position.X, KeyLabel.Position.Y + (IsPressed ? 4 : -4));
	}

	private void CollisionInput(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (!EnableInput)
			return;

		if (@event is InputEventMouse mouseEvent)
		{
			if (!_isPressed && mouseEvent.IsPressed())
			{
				// GD.Print($"{KeyCode} mouseEvent.IsPressed");
				IsPressed = true;
				if (KeyPressCbk != null)
					KeyPressCbk(IsPressed, KeyCode);
			}
			else if (_isPressed && mouseEvent.IsReleased())
			{
				// GD.Print($"{KeyCode} mouseEvent.IsReleased");
				IsPressed = false;
				if (KeyPressCbk != null)
					KeyPressCbk(IsPressed, KeyCode);
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (!EnableInput)
			return;

		if (@event is InputEventKey keyEvent && keyEvent.Keycode == KeyCode)
		{
			if (!_isPressed && keyEvent.IsPressed())
			{
				// GD.Print($"{KeyCode} keyEvent.IsPressed");
				IsPressed = true;
				if (KeyPressCbk != null)
					KeyPressCbk(IsPressed, KeyCode);
			}
			else if (_isPressed && keyEvent.IsReleased())
			{
				// GD.Print($"{KeyCode} keyEvent.IsReleased");
				IsPressed = false;
				if (KeyPressCbk != null)
					KeyPressCbk(IsPressed, KeyCode);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
