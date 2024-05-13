using System;
using Godot;

public partial class NeuralNetwork : ManaConsumer/*, IEvolvable*/ {

	protected Vision visionNode=null;

	public override void _EnterTree() {
		base._EnterTree();
		visionNode = new Vision(new VisionGenome(0.25, 3, 100));
		AddChild(visionNode);
	}

	public AI_Output run(AI_Input input) {
		throw new NotImplementedException();
	}

	public VisionData getVisionData() {
        if(visionNode == null)
            throw new AppException("Error in AI: visionNode is null. Make sure the vision node was constructed before call (preferably in child class' constructor and/or in its _enter_tree function).");
        return visionNode.getVisionData();
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
