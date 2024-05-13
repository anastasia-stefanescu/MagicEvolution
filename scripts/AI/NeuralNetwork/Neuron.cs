using System;
using Godot;

public struct Neuron {
    private double newStimulation;
    private double stimulation;
    private NN_ActivationFunctionEnum activationFunction;

    public Neuron(NN_ActivationFunctionEnum activationFunction) {
        this.activationFunction=activationFunction;
        stimulation=newStimulation=0;
    }

    public void step() {
        stimulation=newStimulation;
        newStimulation=0;
    }

    public void stimulate(double input) { newStimulation+=input; }

    public double getOutput() {
        switch (activationFunction) {
            case NN_ActivationFunctionEnum.Identity:
                return activationFunction_Identity(stimulation);
            case NN_ActivationFunctionEnum.UnitClamp:
                return activationFunction_UnitClamp(stimulation);
            case NN_ActivationFunctionEnum.ReLU:
                return activationFunction_ReLU(stimulation);
            case NN_ActivationFunctionEnum.NeuralCos:
                return activationFunction_NeuralCos(stimulation);
            case NN_ActivationFunctionEnum.NeuralSin:
                return activationFunction_NeuralSin(stimulation);
            default:
                throw new AppException("Error in Neuron: corresponding activation function not added to getOutput().");
        }
    }

    public double getStimulation() { return stimulation; }

    public double getNewStimulation() { return newStimulation; }

    public static double activationFunction_Identity(double stimulation) { return stimulation; }
    public static double activationFunction_UnitClamp(double stimulation) { return Mathf.Clamp(stimulation, -1, 1); }
    public static double activationFunction_ReLU(double stimulation) { return Mathf.Max(0, stimulation); }
    public static double activationFunction_NeuralCos(double stimulation) { return Mathf.Cos(Mathf.Pi*stimulation); }
    public static double activationFunction_NeuralSin(double stimulation) { return Mathf.Sin(Mathf.Pi*stimulation); }
}