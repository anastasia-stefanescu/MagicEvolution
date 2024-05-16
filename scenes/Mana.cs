using Godot;
using System;

public partial class Mana : Area2D
{
	private void _on_body_entered(Node2D body)
	{
		
		//body.date_wizbit.manaFraction += SimulationStatistics.
		//GD.Print(customBody.rotation);
		QueueFree();
	}
	
	
}



