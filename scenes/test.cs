using Godot;
using System;
using System.Collections.Generic;

public partial class test : Node2D
{
	
	public PackedScene ManaScene;
	private int maxSize = (int)SimulationParameters.rangeOnWhichGenerated;
	private List<Wizbit> allWizbits = new List<Wizbit>();
	private List<Mana> allMana = new List<Mana>();
	public PackedScene WizbitScene;
	public override void _Ready()
	{
		ManaScene = GD.Load<PackedScene>("res://scenes/mana.tscn");
		WizbitScene = GD.Load<PackedScene>("res://scenes/wizbit.tscn");
		
		var rng = new RandomNumberGenerator();
		rng.Randomize(); 
		
		for (int i = 0; i < SimulationParameters.initialNoMana; i++)
		{
			Mana instance = ManaScene.Instantiate<Mana>();
			instance.Position = GetRandomPosition(maxSize, rng);
			AddChild(instance);
			allMana.Add(instance);
		}
		SimulationParameters.crtNoMana = SimulationParameters.initialNoMana;
		
		for (int i = 0; i < SimulationParameters.initialNoWizbits; i++)
		{
			Wizbit instance2 = WizbitScene.Instantiate<Wizbit>();
			instance2.Position = GetRandomPosition(maxSize, rng);
			//GD.Print(instance2.neuralNetwork.inputNeuronCount);
			AddChild(instance2);
			allWizbits.Add(instance2);
		}
	}
	
	public override void _Process(double delta)
	{
		double cat_se_consuma_pe_frame = SimulationParameters.crtNoWizbits * SimulationParameters.WizbitStatsParameters.constantCost;
		ReplenishMana((int)SimulationParameters.initialNoMana); //am pus asa deocamdata
		//ReplenishWizbits(5);
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

		if (SimulationParameters.crtNoMana < threshold)
		{
			Mana instance = ManaScene.Instantiate<Mana>();
			instance.Position = GetRandomPosition(maxSize, rng);
			AddChild(instance);
			//GD.Print("replenished mana by 1");
			SimulationParameters.crtNoMana++;
			allMana.Add(instance);
		}
	}
	
	public void ReplenishWizbits(int threshold)
	{
		var rng = new RandomNumberGenerator();
		rng.Randomize(); // Seed with current time

		if (SimulationParameters.crtNoWizbits < threshold)
		{
			var instance = WizbitScene.Instantiate<Wizbit>();
			instance.Position = GetRandomPosition(maxSize, rng);
			AddChild(instance);
		}
	}
}



