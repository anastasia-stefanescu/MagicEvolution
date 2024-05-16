using Godot;
using System;

public abstract partial class ManaConsumer : Godot.Node2D {
	protected double constantCost;
	protected double useCost;

	public double getConstantCost() { return constantCost; }
	public double getUseCost() { return useCost; }

	public double getWeighedUseCost(double weight=1) { return useCost * applyWeightFunction(weight); }

	public abstract void calculateCosts();

	protected abstract MC_WeightFunctionEnum getWeightFunction();

	protected double applyWeightFunction(double weight) {
		switch (getWeightFunction()) {
			case MC_WeightFunctionEnum.Identity:
				return weightFunction_Identity(weight);
			case MC_WeightFunctionEnum.Quadratic:
				return weightFunction_Quadratic(weight);
			default:
				throw new AppException("Error in ManaConsumer: corresponding weight function not added to applyWeightFunction.");
		}
	}

	// Weight functions
	protected static double weightFunction_Identity(double weight) { return weight; }
	protected static double weightFunction_Quadratic(double weight) { return weight*weight; }
}
