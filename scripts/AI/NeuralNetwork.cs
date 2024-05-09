using System;
using Godot;

public partial class NeuralNetwork : AI/*, IEvolvable*/ {

	public override void _EnterTree() {
		base._EnterTree();
		visionNode = new Vision(new VisionGenome(0.25, 3, 100));
		AddChild(visionNode);
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
