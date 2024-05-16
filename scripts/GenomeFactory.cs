using System;
using Godot;

public static class GenomeFactory {

    public static VisionGenome getStarterVisionGenome() {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();

        double fov = rng.RandfRange(0, 1);
        uint rayCountCode = (uint)rng.RandiRange(0, 5);
        double range = rng.RandfRange(50, 200);

        return new VisionGenome(fov, rayCountCode, range);
    }

    public static NeuralNetworkGenome getStarterNNGenome(int synapseCount=-1) {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        
        if(synapseCount<0)
            synapseCount=rng.RandiRange(0, 10);
        
        VisionGenome visionGenome = getStarterVisionGenome();
        uint inputNeuronCount = visionGenome.calcRayCount()*VisionRayData.fieldCount + AI_Input.nonVisionDataFieldCount;
        uint outputNeuronCount = AI_Output.fieldCount;
        
        // create array of all possible synapses
        Synapse[] possibleSynapses = new Synapse[inputNeuronCount * outputNeuronCount];
        for(uint i=0; i<inputNeuronCount; i++)
            for(uint j=0; j<outputNeuronCount; j++) {
                possibleSynapses[i*inputNeuronCount+j].sourceIndex=i;
                possibleSynapses[i*inputNeuronCount+j].destinationIndex=inputNeuronCount+j;
                // roll weights when choosing to avoid unnecessary random calls
            }
        
        // pick synapseCount synapses
        Synapse[] synapses = new Synapse[synapseCount];
        int synapsesToSelect = synapseCount;
        for(int i=0; i<possibleSynapses.Length; i++) {
            if(rng.RandfRange(0, 1) < ((double)synapsesToSelect)/possibleSynapses.Length) {
                synapses[synapseCount-synapsesToSelect]=possibleSynapses[i];

                // roll weight
                double maxAbsWeight = SimulationParameters.AIParameters.synapseMaxAbsoluteWeight;
                synapses[synapseCount-synapsesToSelect].weight = rng.RandfRange((float)-maxAbsWeight, (float)maxAbsWeight);

                synapsesToSelect--;
            }
        }

        return new NeuralNetworkGenome(visionGenome, synapses);
    }

}