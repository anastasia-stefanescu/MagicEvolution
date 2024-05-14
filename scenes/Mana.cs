using Godot;
using System;

public partial class Mana : Area2D
{
	
	private void OnAreaEntered(Node body){
		if (body is CharacterBody2D characterBody)
		{
			GD.Print("Entered!");
			QueueFree();
		}
	}
}
