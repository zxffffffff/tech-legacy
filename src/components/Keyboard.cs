using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class Keyboard : Node2D
{
	private int rows;

	[Export]
	public int Rows
	{
		get { return rows; }
		set { rows = value; layout(); }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		layout();
	}

	public void layout()
	{
		if (!IsNodeReady())
			return;
		for (int i = 0; i < Rows; ++i)
		{
			var row = GetNode<Node2D>("Row" + i.ToString());
			if (row == null)
				break;
			var children = row.GetChildren();
			float keys_width = 0;
			foreach (var child in children)
			{
				var key = child as KeyboardKey;
				keys_width += key.KeyWidth;
			}
			int key_width = 32;
			int gap_width = 3;
			float total_width = keys_width * key_width + (children.Count - 1) * gap_width;
			float start_x = -total_width / 2 - gap_width;
			for (int j = 0; j < children.Count; ++j)
			{
				var key = children[j] as KeyboardKey;
				key.Position = new Vector2(start_x + gap_width + key.KeyWidth * key_width / 2, 0);
				start_x += gap_width + key.KeyWidth * key_width;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
