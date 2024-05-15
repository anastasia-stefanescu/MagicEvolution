using Godot;
using System;

public partial class Wizbit : CharacterBody2D
{
	public WizbitStats stats;
	public NeuralNetwork neuralNetwork;

	private double currentHp;
	private double currentMana;

	private double rotation_speed = 2;
	
	//cele vechi, am pus double in loc de float
	private double ai_x = 0;
	private double ai_y = 0;
	private double ai_rotation = 0;
	private int frame_cnt = 0;

	//public WizbitStats getWizbitStats()
	//{
		//var ref = this.date_wizbit;
		//return ref;
	//}
	
	
	public Wizbit()
	{
		GD.Print("Warning! Wizbit default constructor should be eventually removed!");
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		if(stats==null)
			stats = new WizbitStats(new WizbitStatsGenome(100, 100, 100));
		AddChild(stats);

		if(neuralNetwork == null)
		{
			var visGen = new VisionGenome(1, 0, 0);
			var nnGenome = new NeuralNetworkGenome(visGen);
			neuralNetwork = new NeuralNetwork(nnGenome);
		}
		AddChild(neuralNetwork);
	}

	public Wizbit(WizbitStatsGenome stats, NeuralNetworkGenome nnGenome)
	{
		this.stats = new WizbitStats(stats);
		this.neuralNetwork = new NeuralNetwork(nnGenome);
	}
	
	public AI_Input construct_AI_input()
	{
		AI_Input ai_input = new AI_Input();

		ai_input.hpFraction = currentHp/stats.getMaxHp();
		ai_input.manaFraction = currentMana/stats.getMaxMana();
		ai_input.movementCost = stats.getUseCost();
		
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		ai_input.random=rng.RandfRange(-1, 1);

		//GD.Print("intram getVisionData()");
		ai_input.visionData = this.neuralNetwork.getVisionData().clone();
		//GD.Print("returnam ai_input, intram in run");
		return ai_input;
	}
	
	public Wizbit reproduce()
	{
		Wizbit new_wizBit = new Wizbit((WizbitStatsGenome)stats.getGenomeCopy(), (NeuralNetworkGenome)neuralNetwork.getGenomeCopy());
		new_wizBit.mutate(); //aici trb apelat alt tip de mutate?
		return new_wizBit;
	}
	
	private void mutate()
	{
		throw new NotImplementedException();
	}
	
	private Vector2 apply_AI_Output()
	{
		//GD.Print("apply Ai output");
		AI_Output ai_output = neuralNetwork.run(this.construct_AI_input());

		//output: movex, movey, rotate, reproduce
		
		Vector2 movement = new Vector2((float)ai_output.moveX, (float)ai_output.moveY).Normalized();
		movement = movement * (float)stats.getMaxMovementSpeed();
		
		if (ai_output.reproduce > 0.5)
		{
			this.reproduce();
		}

		return movement;
	}

	//public override void _PhysicsProcess(double delta)
	//{
		//Vector2 movement = apply_AI_Output();
//
		//float rot = (float)ai_rotation * (float)rotation_speed * (float)delta;
		//movement = movement.Rotated(rot);
		//Velocity = movement;
		//MoveAndSlide();
	//}


	public override void _PhysicsProcess(double delta)
	{
		Velocity = new Vector2((float)ai_x, (float)ai_y) * (float)stats.getMaxMovementSpeed();

		if (frame_cnt % 20 == 0)
		{
			ai_rotation = GD.Randf() * 2 - 1;
		}

		if (frame_cnt % 60 == 0)
		{
			ai_x = GD.Randf() * 2 - 1;
		}

		if (frame_cnt % 120 == 0)
		{
			ai_y = GD.Randf() * 2 - 1;
		}
		
		float rotation = (float)ai_rotation * (float)rotation_speed * (float)delta;
		Velocity = Velocity.Rotated(rotation);

		frame_cnt = (frame_cnt + 1) % 120;
		
		MoveAndSlide();
	}
	
	
}



