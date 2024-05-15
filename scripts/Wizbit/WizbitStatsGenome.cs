using Godot;
using System;

public class WizbitStatsGenome : IGenome 
{
	public double maxMana;
	public double maxHp;
	public double maxMovementSpeed;
	
	public WizbitStatsGenome(double mana, double hp, double speed)
	{
		this.maxMana = mana;
		this.maxHp = hp;
		this.maxMovementSpeed = speed;
	}
	
	public void mutate(){
		var rng = new RandomNumberGenerator();
		rng.Randomize();

		float random = rng.RandfRange(0, 1);
		if( random < SimulationParameters.AIParameters.MutationChances.maxMana )
			mutateMaxMana();
		
		random = rng.RandfRange(0, 1);
		if( random < SimulationParameters.AIParameters.MutationChances.maxHp )
			mutateMaxHp();

		random = rng.RandfRange(0, 1);
		if( random < SimulationParameters.AIParameters.MutationChances.maxMovementSpeed )
			mutateMaxSpeed();
	}

	public IGenome clone(){
		return new WizbitStatsGenome(maxMana, maxHp, maxMovementSpeed);
	}
	
	private void mutateMaxMana() {
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		float maxChange = (float)SimulationParameters.AIParameters.MutationParameters.ManaMaxChange;
		maxMana=maxMana*(1+rng.RandfRange(-maxChange, maxChange));
	}
	
	private void mutateMaxHp() {
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		float maxChange = (float)SimulationParameters.AIParameters.MutationParameters.HpMaxChange;
		maxHp=maxHp*(1+rng.RandfRange(-maxChange, maxChange));
	}
	
	private void mutateMaxSpeed() {
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		float maxChange = (float)SimulationParameters.AIParameters.MutationParameters.SpeedMaxChange;
		maxMovementSpeed=maxMovementSpeed*(1+rng.RandfRange(-maxChange, maxChange));
	}
	
}
