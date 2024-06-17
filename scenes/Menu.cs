using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GetNode<RichTextLabel>("Text/Valori/NoWizbits").BbcodeEnabled = true;
		// GetNode<RichTextLabel>("Text/Valori/Gen").BbcodeEnabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Your frame-by-frame logic here
	}

	public void UpdateMenu(int no, int gen)
	{
		
		GetNode<RichTextLabel>("Text/Valori/NoWizbits").Text = $"{no}";
		GetNode<RichTextLabel>("Text/Valori/Gen").Text = $"{gen}";
	}
}

