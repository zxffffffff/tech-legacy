using Godot;
using System;
using System.Collections.Generic;
using Utils;

[Tool]
public partial class Keyboard : Node2D
{
	private CanvasGroup canvasGroup;

	private List<List<KeyboardKey>> keyboardKeys;

	private KeyboardType keyboardType;

	private PackedScene keyboardKeyTscn;

	[Export]
	public KeyboardType KeyboardType
	{
		get { return keyboardType; }
		set { keyboardType = value; ResetKeyboard(); }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		keyboardKeyTscn = GD.Load<PackedScene>("res://scenes/components/KeyboardKey.tscn");

		ResetKeyboard();
	}

	public void ResetKeyboard()
	{
		if (keyboardKeyTscn == null)
			return;

		if (canvasGroup != null)
			canvasGroup.QueueFree();
		canvasGroup = new CanvasGroup();
		AddChild(canvasGroup);

		switch (keyboardType)
		{
			case KeyboardType.MiniKeyboard:
				ResetKeyboard(canvasGroup, KeyboardMgr.MiniKeyboard());
				break;
			case KeyboardType.FullKeyboard:
				ResetKeyboard(canvasGroup, KeyboardMgr.FullKeyboard());
				break;
			case KeyboardType.LettersOnly:
				ResetKeyboard(canvasGroup, KeyboardMgr.LettersOnly());
				break;
			case KeyboardType.LettersAndPunctuation:
				ResetKeyboard(canvasGroup, KeyboardMgr.LettersAndPunctuation());
				break;
		}
	}

	public void ResetKeyboard(Node2D parent, List<List<KeyboardKeyMgr>> list)
	{
		keyboardKeys = new List<List<KeyboardKey>>();
		int rows = list.Count;
		for (int row = 0; row < rows; ++row)
		{
			var keys = new List<KeyboardKey>();
			int cols = list[row].Count;
			for (int col = 0; col < cols; ++col)
			{
				var key = keyboardKeyTscn.Instantiate() as KeyboardKey;
				key.KeyCode = list[row][col].KeyCode;
				key.KeyWidth = list[row][col].KeyWidth;
				parent.AddChild(key);
				keys.Add(key);
			}
			keyboardKeys.Add(keys);
		}
		for (int row = 0; row < rows; ++row)
		{
			LayoutRowKeys(keyboardKeys[row], row - rows / 2);
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

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			GD.Print($"按下 {keyEvent.Keycode}");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
