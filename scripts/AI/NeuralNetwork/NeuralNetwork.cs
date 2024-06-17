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
	
	public uint getInputNeuronCount(){
		return inputNeuronCount;
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
			//GD.Print("vision neuron: ", i, " dist: ", aux.distance, " unghi:", aux.angle, " Mana:", aux.isMana, " Wizbit:", aux.isWizbit);
			//GD.Print(input.visionData)
			neurons[AI_Input.nonVisionDataFieldCount + i*VisionRayData.fieldCount + 0].stimulate(input.visionData.raysData[i].distance);
			neurons[AI_Input.nonVisionDataFieldCount + i*VisionRayData.fieldCount + 1].stimulate(input.visionData.raysData[i].angle);
			neurons[AI_Input.nonVisionDataFieldCount + i*VisionRayData.fieldCount + 2].stimulate(input.visionData.raysData[i].isMana);
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
		output.cast_spell = neurons[inputNeuronCount + 4].getOutput();

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
		//GD.Print("Generarea NN-ului: input: ", inputNeuronCount, " output: ", outputNeuronCount, " hidden: ", hiddenNeuronCount);
		synapses = genome.getSynapsesCopy();

		neurons = new Neuron[inputNeuronCount+outputNeuronCount+hiddenNeuronCount];
		// add input neurons
		for(uint i=0; i<inputNeuronCount; i++)
			neurons[i] = new Neuron(NN_ActivationFunctionEnum.Identity);
		// add output neurons
		for(uint i=0; i<outputNeuronCount; i++)
			neurons[inputNeuronCount + i] = new Neuron(NN_ActivationFunctionEnum.UnitClamp);
		// add hidden neurons
		NN_ActivationFunctionEnum[] tmpHiddenNeuronAF = genome.getHiddenActivationFunctionsCopy();
		for(uint i=0; i<hiddenNeuronCount; i++) {
			if(inputNeuronCount + outputNeuronCount + i >= neurons.Length) {
				//GD.Print("neurons index: " + (inputNeuronCount + outputNeuronCount + i));
				//GD.Print("neurons.Length: " + neurons.Length);
				//GD.Print("\n");
			}
			if(i >= tmpHiddenNeuronAF.Length) {
				//GD.Print("tmpHiddenNeuronAF index: " + i);
				//GD.Print("tmpHiddenNeuronAF.Length: " + tmpHiddenNeuronAF.Length);
				//GD.Print("Hidden neuron count: " + hiddenNeuronCount);
				//GD.Print("\n");
			}

			neurons[inputNeuronCount + outputNeuronCount + i] = new Neuron(tmpHiddenNeuronAF[i]);
		}
	}

	public IGenome getGenomeCopy() { return genome.clone(); }

	public VisionData getVisionData() {
		if(visionNode == null)
			throw new AppException("Error in AI: visionNode is null. Make sure the vision node was constructed before call (preferably in child class' constructor and/or in its _enter_tree function).");
		return visionNode.getVisionData();
	}
	
	public Vision getVision_to_reuse(){
		return visionNode;
	}

	public override void calculateCosts() {
		//GD.Print("Warning! NeuralNetwork.calculateCosts() is still not implemented properly!");
		useCost=0;
		constantCost=0;
	}

	protected override MC_WeightFunctionEnum getWeightFunction() {
		return SimulationParameters.AIParameters.mc_weightFunction;
	}
	
	public uint getHiddenNeuronCount()
	{
		return hiddenNeuronCount;
	}
}
