using Godot;
using System;

public partial class WizbitStats : ManaConsumer, IEvolvable{
	public WizbitStatsGenome wizbitStatsGenome; //max mana, max hp, max movement speed
	public double hpFraction;
	public double manaFraction;
	public double movementCost; // relative to max mana
	public static readonly double constant=1;
	public double random;
	public VisionData visionData;
	
	public WizbitStats()
	{
		generate();
	}

	public void generate(){
		this.hpFraction = 1;
		this.manaFraction = 1;
		this.movementCost = 0.1;
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		this.random = rng.RandfRange(0, 1);
		this.visionData = new VisionData(100); //??
		
	}
	public void mutate(){
		wizbitStatsGenome.mutate();
	}
	public override void calculateCosts(){
		double weightedUseCost = base.getWeighedUseCost();
		this.movementCost = weightedUseCost * manaFraction;
	}

	protected override MC_WeightFunctionEnum getWeightFunction(){
		return MC_WeightFunctionEnum.Quadratic;
	}
	
	
}
