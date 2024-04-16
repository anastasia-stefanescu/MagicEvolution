using Godot;
using System;

public partial class new_script : Node2D
{
	public override void _Ready() {
		AI_Input input = new AI_Input();
		GD.Print(input.hpPercent);
	}
}
