using System;

public enum NN_ActivationFunctionEnum {
	Identity,
	UnitClamp, // mandatory, do not erase
	ReLU, // probably very important
	NeuralCos, // takes a number n in [-1, 1] and return cos(pi*n)
	NeuralSin // takes a number n in [-1, 1] and returns sin(pi*n)
}
