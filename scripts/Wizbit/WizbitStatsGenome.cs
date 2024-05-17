using Godot;
using System;

public class WizbitStatsGenome : IGenome 
{
	private double maxHp;
	private double maxMana;
	private double maxMovementSpeed;
	
	public WizbitStatsGenome(double hp, double mana, double speed)
	{
		this.maxHp = hp;
		this.maxMana = mana;
		this.maxMovementSpeed = speed;
	}

	public double getMaxHp() { return maxHp; }
	public double getMaxMana() { return maxMana; }
	public double getMaxMovementSpeed() { return maxMovementSpeed; }
	
	public void mutate(){
		var rng = new RandomNumberGenerator();
		rng.Randomize();

		float random = rng.RandfRange(0, 1);
		if( random < SimulationParameters.WizbitStatsParameters.MutationChances.maxMana )
			mutateMaxMana();
		
		random = rng.RandfRange(0, 1);
		if( random < SimulationParameters.WizbitStatsParameters.MutationChances.maxHp )
			mutateMaxHp();

		random = rng.RandfRange(0, 1);
		if( random < SimulationParameters.WizbitStatsParameters.MutationChances.maxMovementSpeed )
			mutateMaxSpeed();
	}

	public IGenome clone(){
		return new WizbitStatsGenome(maxMana, maxHp, maxMovementSpeed);
	}
	
	private void mutateMaxMana() {
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		float maxChange = (float)SimulationParameters.WizbitStatsParameters.MutationParameters.ManaMaxChange;
		maxMana=maxMana*(1+rng.RandfRange(-maxChange, maxChange));
	}
	
	private void mutateMaxHp() {
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		float maxChange = (float)SimulationParameters.WizbitStatsParameters.MutationParameters.HpMaxChange;
		maxHp=maxHp*(1+rng.RandfRange(-maxChange, maxChange));
	}
	
	private void mutateMaxSpeed() {
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		float maxChange = (float)SimulationParameters.WizbitStatsParameters.MutationParameters.SpeedMaxChange;
		maxMovementSpeed=maxMovementSpeed*(1+rng.RandfRange(-maxChange, maxChange));
	}
	
}
