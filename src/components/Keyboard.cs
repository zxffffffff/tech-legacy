using Godot;
using System;
using System.Collections.Generic;
using Utils;

[Tool]
public partial class Keyboard : Node2D
{
	private KeyboardType _type;

	[Export]
	public KeyboardType Type
	{
		get { return _type; }
		set { _type = value; ResetKeyboard(); }
	}

	[Export]
	public PackedScene KeyboardKeyTscn { get; set; }

	public CanvasGroup CanvasGroup { get; private set; }
	public List<List<KeyboardKey>> Keys { get; private set; }

	public event EventMgr.KeyPressEvent KeyPressCbk;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		ResetKeyboard();
	}

	public void ResetKeyboard()
	{
		if (KeyboardKeyTscn == null)
			return;

		if (CanvasGroup != null)
			CanvasGroup.QueueFree();
		CanvasGroup = new CanvasGroup();
		AddChild(CanvasGroup);

		switch (Type)
		{
			case KeyboardType.MiniKeyboard:
				ResetKeyboard(CanvasGroup, KeyboardMgr.MiniKeyboard());
				break;
			case KeyboardType.FullKeyboard:
				ResetKeyboard(CanvasGroup, KeyboardMgr.FullKeyboard());
				break;
			case KeyboardType.LettersOnly:
				ResetKeyboard(CanvasGroup, KeyboardMgr.LettersOnly());
				break;
			case KeyboardType.LettersAndPunctuation:
				ResetKeyboard(CanvasGroup, KeyboardMgr.LettersAndPunctuation());
				break;
		}
	}

	public void ResetKeyboard(Node2D parent, List<List<KeyboardKeyMgr>> list)
	{
		Keys = new List<List<KeyboardKey>>();
		int rows = list.Count;
		for (int row = 0; row < rows; ++row)
		{
			var row_keys = new List<KeyboardKey>();
			int cols = list[row].Count;
			for (int col = 0; col < cols; ++col)
			{
				var key = KeyboardKeyTscn.Instantiate() as KeyboardKey;
				key.KeyCode = list[row][col].KeyCode;
				key.KeyWidth = list[row][col].KeyWidth;
				key.KeyPressCbk += (bool isPressed, Key keyCode) => KeyPressCbk(isPressed, keyCode);
				parent.AddChild(key);
				row_keys.Add(key);
			}
			Keys.Add(row_keys);
		}
		for (int row = 0; row < rows; ++row)
		{
			LayoutRowKeys(Keys[row], row - rows / 2);
		}
	}

	public void LayoutRowKeys(List<KeyboardKey> keys, float y_index)
	{
		Vector2 key_size = new Vector2(32, 32);
		Vector2 gap_size = new Vector2(3, 3);
		float keys_width = 0;
		foreach (var key in keys)
		{
			keys_width += key.KeyWidth;
		}

		float total_width = keys_width * key_size.X + (keys.Count - 1) * gap_size.X;
		float start_x = -total_width / 2 - gap_size.X;
		float start_y = y_index * (key_size.Y + gap_size.Y);
		for (int col = 0; col < keys.Count; ++col)
		{
			var key = keys[col] as KeyboardKey;
			key.Position = new Vector2(start_x + gap_size.X + key.KeyWidth * key_size.X / 2, start_y);
			start_x += gap_size.X + key.KeyWidth * key_size.X;
		}
	}

	// public override void _Input(InputEvent @event)
	// {
	// 	if (@event is InputEventKey keyEvent && keyEvent.Pressed)
	// 	{
	// 		GD.Print($"按下 {keyEvent.Keycode}");
	// 	}
	// }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
