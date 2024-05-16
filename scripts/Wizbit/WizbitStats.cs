using Godot;
using System;

public partial class WizbitStats : ManaConsumer, IEvolvable {
	private WizbitStatsGenome genome; //max mana, max hp, max movement speed
	
	private double maxHp;
	private double maxMana;
	private double maxMovementSpeed;
	
	public WizbitStats(WizbitStatsGenome genome)
	{
		this.genome=(WizbitStatsGenome)genome.clone();
		generate();
	}

	public double getMaxHp() { return maxHp; }
	public double getMaxMana() { return maxMana; }
	public double getMaxMovementSpeed() { return maxMovementSpeed; }

	public void generate() {
		maxHp=genome.getMaxHp();
		maxMana=genome.getMaxMana();
		maxMovementSpeed=genome.getMaxMovementSpeed();
	}
	public void mutate(){
		genome.mutate();
		generate();
	}

	public IGenome getGenomeCopy() { return genome.clone(); }

	public override void calculateCosts(){
		throw new NotImplementedException();
	}

	protected override MC_WeightFunctionEnum getWeightFunction(){
		return SimulationParameters.WizbitParameters.statsWeightFunction;
	}
	
	
}
