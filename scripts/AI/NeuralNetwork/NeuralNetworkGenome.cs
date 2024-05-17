using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using Godot;

public class NeuralNetworkGenome : IGenome {
	private VisionGenome visionGenome;
	private uint hiddenNeuronCount;
	private Synapse[] synapses;
	private NN_ActivationFunctionEnum[] hiddenActivationFunctions;

	public void mutate() {
		GD.Print("mutating");
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		// mutate vision genome and adjust synapses
		uint oldInputNeuronCount = getInputNeuronCount();
		visionGenome.mutate();
		if(oldInputNeuronCount > getInputNeuronCount()) { // inputs were deleted
			uint difference = oldInputNeuronCount - getInputNeuronCount();
			List<Synapse> newSynapses = new List<Synapse>();
			for(uint i=0; i<synapses.Length; i++) {
				uint src = synapses[i].sourceIndex;
				uint dst = synapses[i].destinationIndex;
				double weight = synapses[i].weight;

				if(src>=oldInputNeuronCount)
					src-=difference;
				else if(src>=oldInputNeuronCount-difference)
					continue;
				
				//destination neurons cannot be input neurons, so no need for any checks
				dst-=difference;
				newSynapses.Append(new Synapse(src, dst, weight)); 
			}

			// update synapses
			synapses = newSynapses.ToArray();
		}
		else if(oldInputNeuronCount < getInputNeuronCount()) { // inputs were added
			uint difference = getInputNeuronCount() - oldInputNeuronCount;
			for(uint i=0; i<synapses.Length; i++) {
				if(synapses[i].sourceIndex >= oldInputNeuronCount)
					synapses[i].sourceIndex += difference;
				synapses[i].destinationIndex+=difference;
			}
		}

		// roll for synapse creation
		if(rng.RandfRange(0, 1) < SimulationParameters.AIParameters.MutationChances.createSynapse)
			mutate_createSynapse();

		// roll for synapse modification
		if(rng.RandfRange(0, 1) < SimulationParameters.AIParameters.MutationChances.modifySynapse)
			mutate_modifySynapse();

		// roll for synapse evolution	
		if(rng.RandfRange(0, 1) < SimulationParameters.AIParameters.MutationChances.evolveSynapse)
			mutate_evolveSynapse();
			

		// roll for synapse removal
		if(rng.RandfRange(0, 1) < SimulationParameters.AIParameters.MutationChances.removeSynapse)
			mutate_removeSynapse();

		// roll for (hidden) neuron modification
		if(rng.RandfRange(0, 1) < SimulationParameters.AIParameters.MutationChances.modifyNeuron)
			mutate_modifyNeuron();

		// roll for (hidden) neuron deletion 
		if(rng.RandfRange(0, 1) < SimulationParameters.AIParameters.MutationChances.removeNeuron)
			mutate_removeNeuron();
	}

	public IGenome clone() {
		return new NeuralNetworkGenome(visionGenome, synapses, hiddenActivationFunctions);
	}

	public NeuralNetworkGenome(VisionGenome visionGenome, Synapse[] synapses=null, NN_ActivationFunctionEnum[] hiddenActivationFunctions=null) {
		if(visionGenome==null)
			throw new AppException("Error in NeuralNetworkGenome constructor: visionGenome parameter may not be null.");
		this.visionGenome = (VisionGenome)visionGenome.clone();
		
		if(hiddenActivationFunctions!=null) {
			this.hiddenActivationFunctions = (NN_ActivationFunctionEnum[])hiddenActivationFunctions.Clone();
			hiddenNeuronCount=(uint)hiddenActivationFunctions.Length;
		}
		else {
			this.hiddenActivationFunctions = new NN_ActivationFunctionEnum[0];
			hiddenNeuronCount=0;
		}
		
		// check synapses validity
		if(synapses==null) {
			this.synapses = new Synapse[0];
			return;
		}
		uint totalNeuronCount = getInputNeuronCount()+hiddenNeuronCount+AI_Output.fieldCount;
 		for(uint i=0; i<synapses.Length; i++) {
			if(synapses[i].sourceIndex>=totalNeuronCount || synapses[i].destinationIndex>=totalNeuronCount) // invalid indices
				throw new AppException("Error in NeuralNetworkGenome constructor: parameter synapses array contains invalid indices. (synapse " + i +")");
			else if(synapses[i].destinationIndex < getInputNeuronCount())
				throw new AppException("Error in NeuralNetworkGenome constructor: parameter synapses array contains synapses with input neurons as destinations. (synapse " + i +")");
			else if(synapses[i].sourceIndex >= getInputNeuronCount() && synapses[i].sourceIndex < getInputNeuronCount()+AI_Output.fieldCount )
				throw new AppException("Error in NeuralNetworkGenome constructor: parameter synapses array contains synapses with output neurons as destinations. (synapse " + i +")");
			
			// clamp synapse weights
			double maxAbsWeight = SimulationParameters.AIParameters.synapseMaxAbsoluteWeight;
			if(synapses[i].weight < -maxAbsWeight || synapses[i].weight > maxAbsWeight)
				GD.Print("Warning! NeuralNetworkGenome constructor received synapse weights outside the specified limits. Clamping.");
			synapses[i].weight = Mathf.Clamp(synapses[i].weight, -maxAbsWeight, maxAbsWeight);
		}

		this.synapses = (Synapse[])synapses.Clone();
	}

	public Synapse[] getSynapsesCopy() { return (Synapse[])synapses.Clone(); }

	public NN_ActivationFunctionEnum[] getHiddenActivationFunctionsCopy() { return (NN_ActivationFunctionEnum[])hiddenActivationFunctions.Clone(); } 

	public uint getInputNeuronCount() { return visionGenome.calcRayCount() * VisionRayData.fieldCount + AI_Input.nonVisionDataFieldCount; }

	public uint getHiddenNeuronCount() { return hiddenNeuronCount; }

	public uint getVisionNeuronCount() { return visionGenome.calcRayCount(); }

	public VisionGenome getVisionGenomeCopy() { return (VisionGenome)visionGenome.clone(); }

	private void mutate_createSynapse() {
		uint inputNeuronCount=getInputNeuronCount();
		uint outputNeuronCount=AI_Output.fieldCount;
		
		// find missing synapses
		HashSet<Tuple<uint, uint>> missingSynapses = new HashSet<Tuple<uint, uint>>();
		for(uint dest=inputNeuronCount; dest<inputNeuronCount+outputNeuronCount+hiddenNeuronCount; dest++) {
			for(uint src=0; src<inputNeuronCount; src++) // input neuron sources
				missingSynapses.Add(new Tuple<uint, uint>(src, dest));
			for(uint src=inputNeuronCount+outputNeuronCount; src<inputNeuronCount+outputNeuronCount+hiddenNeuronCount; src++) // hidden neuron sources
				missingSynapses.Add(new Tuple<uint, uint>(src, dest));
		}
		for(uint i=0; i<synapses.Length; i++)
			missingSynapses.Remove(new Tuple<uint, uint>(synapses[i].sourceIndex, synapses[i].destinationIndex));
		
		// pick a random missing synapse, if one exists
		Tuple<uint, uint>[] missingArray = missingSynapses.ToArray();
		if(missingArray.Length==0)
			return;
		RandomNumberGenerator rng=new RandomNumberGenerator();
		rng.Randomize();
		int randIndex = rng.RandiRange(0, missingArray.Length-1);
		
		uint newSource = missingArray[randIndex].Item1;
		uint newDestination = missingArray[randIndex].Item2;
		double maxAbsWeight = SimulationParameters.AIParameters.synapseMaxAbsoluteWeight;
		double weight = rng.RandfRange((float)-maxAbsWeight, (float)maxAbsWeight);

		synapses = synapses.Append(new Synapse(newSource, newDestination, weight)).ToArray();
	}

	private void mutate_modifySynapse() {
		if(synapses.Length == 0)
			return;
		
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		double maxChange = SimulationParameters.AIParameters.MutationParameters.synapseModificationMaxChange;
		double maxAbsWeight = SimulationParameters.AIParameters.synapseMaxAbsoluteWeight;
		int randIndex = rng.RandiRange(0, synapses.Length-1);
		synapses[randIndex].weight = (1+rng.RandfRange((float)-maxChange, (float)maxChange))*synapses[randIndex].weight;
		synapses[randIndex].weight = Mathf.Clamp(synapses[randIndex].weight, -maxAbsWeight, maxAbsWeight);
	}

	private void mutate_evolveSynapse() {
		if(synapses.Length == 0)
			return;
		
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		hiddenActivationFunctions = hiddenActivationFunctions.Append(NN_ActivationFunctionEnum.Identity).ToArray();
		hiddenNeuronCount++;
		int randIndex = rng.RandiRange(0, synapses.Length-1);
		uint dst=synapses[randIndex].destinationIndex;
		synapses[randIndex].destinationIndex=getInputNeuronCount()+AI_Output.fieldCount+hiddenNeuronCount-1;
		synapses = synapses.Append(new Synapse(getInputNeuronCount()+AI_Output.fieldCount+hiddenNeuronCount-1, dst, synapses[randIndex].weight)).ToArray();
	}

	private void mutate_removeSynapse() {
		if(synapses.Length == 0)
			return;
		
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		int removalIndex = rng.RandiRange(0, synapses.Length-1);
		Synapse[] newSynapses = new Synapse[synapses.Length-1];
		for(int i=0; i<synapses.Length; i++)
			if(i<removalIndex)
				newSynapses[i]=synapses[i];
			else if(i>removalIndex)
				newSynapses[i-1]=synapses[i];
		synapses=newSynapses;
	}

	private void mutate_modifyNeuron() {
		if(hiddenNeuronCount == 0)
			return;
		
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		int chosenHNeuron = rng.RandiRange(0, (int)hiddenNeuronCount-1);
		Array values = Enum.GetValues(typeof(NN_ActivationFunctionEnum));
		NN_ActivationFunctionEnum newActivationFunction = (NN_ActivationFunctionEnum)values.GetValue(rng.RandiRange(0, values.Length-1));
		hiddenActivationFunctions[chosenHNeuron]=newActivationFunction;
	}

	private void mutate_removeNeuron() {
		if(hiddenNeuronCount == 0)
			return;
		
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		uint chosenHNeuron = (uint)rng.RandiRange(0, (int)hiddenNeuronCount-1);
		NN_ActivationFunctionEnum[] newHAFunctions = new NN_ActivationFunctionEnum[hiddenNeuronCount-1];
		for(uint i=0; i<hiddenNeuronCount; i++)
			if(i<chosenHNeuron)
				newHAFunctions[i]=hiddenActivationFunctions[i];
			else if(i>chosenHNeuron)
				newHAFunctions[i-1]=hiddenActivationFunctions[i];
		hiddenActivationFunctions = newHAFunctions;

		List<Synapse> newSynapses = new List<Synapse>();
		chosenHNeuron = getInputNeuronCount() + AI_Output.fieldCount + chosenHNeuron; // convert to synapse index format
		for(uint i=0; i<synapses.Length; i++) {
			if(synapses[i].sourceIndex==chosenHNeuron || synapses[i].destinationIndex==chosenHNeuron)
				continue;
			
			uint src=synapses[i].sourceIndex;
			uint dst=synapses[i].destinationIndex;
			if(src>chosenHNeuron)
				src--;
			if(dst>chosenHNeuron)
				dst--;
			newSynapses.Append(new Synapse(src, dst, synapses[i].weight));
		}
		hiddenNeuronCount--;
		synapses=newSynapses.ToArray();
	}
}
