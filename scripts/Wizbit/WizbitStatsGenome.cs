using Godot;
using System;

public class WizbitStatsGenome : IGenome 
{
	private double maxHp;
	private double maxMana;
	private double maxMovementSpeed;
	
	//ideal temperature and altitude for a wizbit
	private double idealTemp;
	private double idealAlt;
	
	public WizbitStatsGenome(double hp, double mana, double speed)
	{
		this.maxHp = hp;
		this.maxMana = mana;
		this.maxMovementSpeed = speed;
		
		this.idealTemp = 50;
		this.idealAlt = 50;
	}

	public double getMaxHp() { return maxHp; }
	public double getMaxMana() { return maxMana; }
	public double getMaxMovementSpeed() { return maxMovementSpeed; }
	
	public double getIdealTemp() {return idealTemp;}
	public double getIdealAlt() {return idealAlt;}
	
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
	
	//mutate ideal temperature and altitude based on current enviroment
	public void mutateEnv(double currentTemp, double currentAlt) {
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		float random = rng.RandfRange(0, 1);
		if(random < SimulationParameters.WizbitStatsParameters.MutationChances.envChance) {
			mutateIdealTemp(currentTemp);
			mutateIdealAlt(currentAlt);
		}
	}
	
	private void mutateIdealTemp(double currentTemp) {
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		float maxChange = (float)SimulationParameters.WizbitStatsParameters.MutationParameters.EnvMaxChangeMod;
		idealTemp=idealTemp+(currentTemp-idealTemp)*rng.RandfRange(-maxChange/2, maxChange);
	}
	
	private void mutateIdealAlt(double currentAlt) {
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		float maxChange = (float)SimulationParameters.WizbitStatsParameters.MutationParameters.EnvMaxChangeMod;
		idealAlt=idealAlt+(currentAlt-idealAlt)*rng.RandfRange(-maxChange/2, maxChange);
	}
	
}
