using Godot;
using System;

public partial class WizbitStats : ManaConsumer, IEvolvable{

	public double hpFraction;
	public double manaFraction;
	public double movementCost; // relative to max mana
	public static readonly double constant=1;
	public double random;
	public VisionData visionData;

	public void generate(){
		GD.Print("Not implemented yet");
	}
	public void mutate(){
		GD.Print("Not implemented yet");
	}
	public override void calculateCosts(){
		GD.Print("Not implemented yet");
	}

	protected override MC_WeightFunctionEnum getWeightFunction(){
		return MC_WeightFunctionEnum.Quadratic;
	}
	
	
}
