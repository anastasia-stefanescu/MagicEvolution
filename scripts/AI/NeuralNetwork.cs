using System;
using Godot;

public partial class NeuralNetwork : AI/*, IEvolvable*/ {
	public override void _EnterTree() {
		base._EnterTree();
		
	}

	public override void _Ready()
	{
		base._Ready();
		PackedScene prefab=GD.Load<PackedScene>("res://scenes/test_sprite_2d.tscn");
		if(prefab==null)
			GD.Print("Bag pl grandios");
		Node instance = prefab.Instantiate();
		AddChild(instance);
		instance.Owner=GetTree().CurrentScene;
		if(instance==null)
			GD.Print("Bag pl");
		GD.Print("Done");
		GD.Print("This mf is still not showing in the scene tree, wtf?!");
	}

	public override AI_Output run(AI_Input input) {
		throw new NotImplementedException();
	}

	public override void calculateCosts() {
		GD.Print("Warning! NeuralNetwork.calculateCosts() is still not implemented properly!");
		useCost=0;
		constantCost=0;
	}

	protected override MC_WeightFunctionEnum getWeightFunction() {
		return SimulationParameters.AIParameters.mc_weightFunction;
	}
}
