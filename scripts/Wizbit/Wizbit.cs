using Godot;
using System;

public partial class Wizbit : CharacterBody2D
{
	public WizbitStats stats;
	public NeuralNetwork neuralNetwork;
	
	//private int id;
	//private int id_generator = 0;
	public Label label;

	private double currentHp;
	private double currentMana;
	private static readonly double rotation_speed = 2;
	
	//cat timp a trecut de la crearea wizbitului
	private int time_elapsed;
	
	//cele vechi, am pus double in loc de float
	private double ai_x = 0;
	private double ai_y = 0;
	private double ai_rotation = 0;
	private int frame_cnt = 0;

	
	public Wizbit() {
		
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		if(stats==null)
			stats = new WizbitStats(GenomeFactory.getStarterWizbitStatsGenome());
		AddChild(stats);

		if(neuralNetwork == null)
			neuralNetwork = new NeuralNetwork(GenomeFactory.getStarterNNGenome());
		AddChild(neuralNetwork);
	}
	
	////ready se apeleaza dupa _EnterTree
	//public override void _Ready()
	//{
		//this.id_generator++;
		//this.id = this.id_generator;
		//this.label.Text = "Wizbit " +  this.id; 
	//}

	

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
		new_wizBit.mutate(); //aici trb apelat alt tip de mutate? (nu, e bine)
		return new_wizBit;
	}
	
	public void mutate() {
		stats.mutate();
		currentHp=stats.getMaxHp();
		currentMana=stats.getMaxMana();

		neuralNetwork.mutate();
	}
	
	private  AI_Output apply_AI_Output()
	{
		AI_Output ai_output = neuralNetwork.run(this.construct_AI_input());

		if (ai_output.reproduce > 0.5)
		{
			this.reproduce();
		}

		return ai_output;
	}

	public override void _Process(double delta)
	{
		//descrestem mana
		//this.currentMana -= this.stats.base.constantCost * 
		
		AI_Output ai_output = apply_AI_Output();
		
		Vector2 movement = new Vector2((float)ai_output.moveX, (float)ai_output.moveY) * (float)stats.getMaxMovementSpeed();

		float rotation = (float)ai_output.rotate * (float)rotation_speed * (float)delta;
		movement = movement.Rotated(rotation);
		
		//movement = new Vector2(0, 0); // FOR TESTING ONLY
		Velocity = movement;
		MoveAndSlide();
		
	}


	//public override void _Process(double delta)
	//{
		//Velocity = new Vector2((float)ai_x, (float)ai_y) * (float)stats.getMaxMovementSpeed();
//
		//if (frame_cnt % 20 == 0)
		//{
			//ai_rotation = GD.Randf() * 2 - 1;
		//}
//
		//if (frame_cnt % 30 == 0)
		//{
			//ai_x = GD.Randf() * 2 - 1;
		//}
//
		//if (frame_cnt % 50 == 0)
		//{
			//ai_y = GD.Randf() * 2 - 1;
		//}
		//
		//float rotation = (float)ai_rotation * (float)rotation_speed * (float)delta;
		//Velocity = Velocity.Rotated(rotation);
//
		//frame_cnt = (frame_cnt + 1) % 120;
		//
		//MoveAndSlide();
		//}
	
}



