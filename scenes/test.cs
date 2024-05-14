using Godot;
using System;
using System.Collections.Generic;

public partial class test : Node2D
{
	
	public PackedScene ManaScene;
	public List<Mana> allMana = new List<Mana>();
	
	private int maxSize = (int)SimulationParameters.rangeOnWhichGenerated;
	
	 public PackedScene WizbitScene;
	public List<wizbit> allWizbits = new List<wizbit>();
	
	public override void _Ready()
	{
		ManaScene = GD.Load<PackedScene>("res://scenes/mana.tscn");
		WizbitScene = GD.Load<PackedScene>("res://scenes/wizbit.tscn");
		
		var rng = new RandomNumberGenerator();
		rng.Randomize(); 
		
		for (int i = 0; i < SimulationParameters.initialNoWizbits; i++)
		{
			Mana instance = ManaScene.Instantiate<Mana>();
			instance.Position = GetRandomPosition(maxSize, rng);
			AddChild(instance);
			//allMana.Add(instance);
			
			wizbit instance2 = WizbitScene.Instantiate<wizbit>();
			instance2.Position = GetRandomPosition(maxSize, rng);
			AddChild(instance2);
			allWizbits.Add(instance2);
		}
	}
	
	public void _Process(float delta)
	{
		//ReplenishMana(200);
		//ReplenishWizbits(200);
	}
	
	public Vector2 GetRandomPosition(int maxSize, RandomNumberGenerator rng)
	{
		Random random = new Random();
		int x = rng.RandiRange(-maxSize, maxSize);
		int y = rng.RandiRange(-maxSize, maxSize);
		return new Vector2(x, y);
	}
	
	public void ReplenishMana(int threshold)
	{
		var rng = new RandomNumberGenerator();
		rng.Randomize(); // Seed with current time

		while (allMana.Count < threshold)
		{
			Mana instance = ManaScene.Instantiate<Mana>();
			instance.Position = GetRandomPosition(maxSize, rng);
			AddChild(instance);
			allMana.Add(instance);
			
			
		}
	}
	
	public List<Mana> GetAllMana()
	{
		return allMana;
	}
	
	public void ReplenishWizbits(int threshold)
	{
		var rng = new RandomNumberGenerator();
		rng.Randomize(); // Seed with current time

		while (allWizbits.Count < threshold)
		{
			var instance = WizbitScene.Instantiate<wizbit>();
			instance.Position = GetRandomPosition(maxSize, rng);
			AddChild(instance);
			allWizbits.Add(instance);
		}
	}
	
	public List<wizbit> GetAllWizbits()
	{
		return allWizbits;
	}
	
	//private void _on_mana_body_entered(Node2D body)
//{
		//GD.Print("Body has entered");
		//QueueFree();
//}
}



