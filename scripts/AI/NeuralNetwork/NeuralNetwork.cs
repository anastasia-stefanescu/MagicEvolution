using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Godot;

public partial class NeuralNetwork : ManaConsumer, IEvolvable {

	private Vision visionNode=null;
	private NeuralNetworkGenome genome;
	private uint inputNeuronCount;
	private uint outputNeuronCount;
	private uint hiddenNeuronCount;
	private Neuron[] neurons;
	private Synapse[] synapses;

	public NeuralNetwork(NeuralNetworkGenome genome) {
		this.genome=(NeuralNetworkGenome)genome.clone();
		generate();
	}

	public override void _EnterTree() {
		base._EnterTree();
		visionNode = new Vision(genome.getVisionGenomeCopy());
		AddChild(visionNode);
	}

	public AI_Output run(AI_Input input) {
		// set up input neurons
		neurons[0].stimulate(input.hpFraction);
		neurons[1].stimulate(input.manaFraction);
		neurons[2].stimulate(input.movementCost);
		neurons[3].stimulate(AI_Input.constant);
		neurons[4].stimulate(input.random);
		for(uint i=0; i<genome.getVisionNeuronCount(); i++) {
			neurons[AI_Input.nonVisionDataFieldCount + i*VisionRayData.fieldCount + 0].stimulate(input.visionData.raysData[i].distance);
			neurons[AI_Input.nonVisionDataFieldCount + i*VisionRayData.fieldCount + 1].stimulate(input.visionData.raysData[i].angle);
			neurons[AI_Input.nonVisionDataFieldCount + i*VisionRayData.fieldCount + 2].stimulate(input.visionData.raysData[i].isManaPellet);
			neurons[AI_Input.nonVisionDataFieldCount + i*VisionRayData.fieldCount + 3].stimulate(input.visionData.raysData[i].isWizbit);
		}
		for(uint i=0; i<inputNeuronCount; i++)
			neurons[i].step();

		// run the neural net
		for(uint i=0; i<synapses.Length; i++)
			neurons[synapses[i].destinationIndex].stimulate(neurons[synapses[i].sourceIndex].getOutput());
		for(uint i=0; i<neurons.Length; i++)
			neurons[i].step();
		
		// construct output
		AI_Output output = new AI_Output();
		output.moveX = neurons[inputNeuronCount + 0].getOutput();
		output.moveY = neurons[inputNeuronCount + 1].getOutput();
		output.rotate = neurons[inputNeuronCount + 2].getOutput();
		output.reproduce = neurons[inputNeuronCount + 3].getOutput();

		return output;
	}

	public void mutate() {
		genome.mutate();
		generate();
	}

	public void generate() {
		inputNeuronCount = genome.getInputNeuronCount();
		outputNeuronCount = AI_Output.fieldCount;
		hiddenNeuronCount = genome.getHiddenNeuronCount();
		synapses = genome.getSynapsesCopy();

		List<Neuron> tmpNeurons = new List<Neuron>();
		// add input neurons
		for(uint i=0; i<inputNeuronCount; i++)
			tmpNeurons.Add(new Neuron(NN_ActivationFunctionEnum.Identity));
		// add output neurons
		for(uint i=0; i<AI_Output.fieldCount; i++)
			tmpNeurons.Add(new Neuron(NN_ActivationFunctionEnum.UnitClamp));
		// add hidden neurons
		NN_ActivationFunctionEnum[] tmpHiddenNeuronAF = genome.getHiddenActivationFunctionsCopy();
		for(uint i=0; i<hiddenNeuronCount; i++)
			tmpNeurons.Add(new Neuron(tmpHiddenNeuronAF[i]));
		
		neurons = tmpNeurons.ToArray();
	}

	public IGenome getGenomeCopy() { return genome.clone(); }

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
