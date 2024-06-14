using Godot;
using System;

public partial class Mana : Area2D
{
	private void _on_body_entered(Node2D body)
	{
		SimulationParameters.crtNoMana--;
		//body.date_wizbit.manaFraction += SimulationStatistics.
		//GD.Print(customBody.rotation);
		Wizbit w = body as Wizbit;
		w.addMana(SimulationParameters.ManaValue);
		QueueFree();
	}
	
	
}



