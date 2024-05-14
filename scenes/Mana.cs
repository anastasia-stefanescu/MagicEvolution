using Godot;
using System;

public partial class Mana : Area2D
{
	
	private void _on_body_entered(Node2D body)
	{
			GD.Print("Body has entered");
			//wizbit.date_wizbit.manaFraction += SimulationParameters.manaValue / wizbit.date_wizbit.manaFraction; 
			QueueFree();
	}
}



