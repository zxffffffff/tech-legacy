using Godot;
using System;

public partial class Main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Hello C# ~");

		var testLabel = GetNode<Label>("TestLabel");
		testLabel.Text = "Hello C# ~~";

		var testButton = GetNode<Button>("TestButton");
		testButton.Pressed += () => testLabel.Text += "~";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
