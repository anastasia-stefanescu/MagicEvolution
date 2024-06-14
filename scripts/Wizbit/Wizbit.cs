using Godot;
using System;

public partial class Wizbit : CharacterBody2D
{
	public WizbitStats stats;
	public NeuralNetwork neuralNetwork;
	
	private int id;
	private static int id_generator = 0;
	public Label label;
	private double currentHp;
	private double currentMana;
	private static readonly double rotation_speed = 2;

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
	public override void _Ready()
	{
		id_generator++;
		this.id = id_generator;
		label = GetChild<Label>(0); 
		label.Text = "Wizbit "+this.id; 
		currentHp = stats.getMaxHp();
		currentMana = stats.getMaxMana();
		//AI_Output ai_output = apply_AI_Output();
		//GD.Print(ai_output.moveX, ai_output.moveY, ai_output.rotate, ai_output.reproduce);
	}
	
	public void addMana(double val){
		this.currentMana += val;
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

		ai_input.visionData = this.neuralNetwork.getVisionData().clone();
		GD.Print("Ai input: hp: ", ai_input.hpFraction, ", mana: ", ai_input.manaFraction, ", mvCost: ", ai_input.movementCost, ", random: ", ai_input.random);
		GD.Print("Ai vision data: raycount: ", ai_input.visionData.rayCount);
		for (int i= 0; i< ai_input.visionData.rayCount; i++)
			GD.Print("   Ray ", i, " : dist: ", ai_input.visionData.raysData[i].distance, " angle: ", ai_input.visionData.raysData[i].angle, " isMana: ", ai_input.visionData.raysData[i].isMana, "isW: ", ai_input.visionData.raysData[i].isWizbit);
		return ai_input;
	}
	
	public Wizbit reproduce()
	{
		Wizbit new_wizbit = new Wizbit((WizbitStatsGenome)stats.getGenomeCopy(), (NeuralNetworkGenome)neuralNetwork.getGenomeCopy());
		new_wizbit.mutate(); 

		PackedScene WizbitScene = GD.Load<PackedScene>("res://scenes/wizbit.tscn");
		Wizbit instance = WizbitScene.Instantiate<Wizbit>();

		instance.stats = new WizbitStats((WizbitStatsGenome)new_wizbit.stats.getGenomeCopy());
		instance.neuralNetwork = new NeuralNetwork((NeuralNetworkGenome)new_wizbit.neuralNetwork.getGenomeCopy());
		//GD.Print(instance2.neuralNetwork.inputNeuronCount);
		instance.Position = this.Position;
		GetTree().Root.CallDeferred("add_child", instance);
		return instance;
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
			GD.Print(this.id, " trebuie sa se reproduca, are : " , this.currentMana, ", ", this.stats.getMaxMana());
			if (this.currentMana >= 0.75 * this.stats.getMaxMana())
			{
				this.currentMana -= 0.75 * this.stats.getMaxMana();
				this.reproduce();
			}
		}

		//aici va fi si pt vraji
		// if (ai_output.cast_spell > 0.5 && this.currentHp >= 0.25 * this.stats.getMaxHp())
		// {
		// 	this.currentHp -= 0.25 * this.stats.getMaxHp()
		// 	this.currentMana -= this.stats.getWeighedUseCost(0.25)
		// 	this.reproduce();
		// }

		return ai_output;
	}

	public override void _Process(double delta)
	{
		//get biome for the current position of the wizibit
		double currentTemp = WorldGenerator.tempNoiseMap[(int)this.Position.X / 16, (int)this.Position.Y / 16];
		double currentAlt = WorldGenerator.altNoiseMap[(int)this.Position.X / 16, (int)this.Position.Y / 16];
		double currentVeg = WorldGenerator.vegNoiseMap[(int)this.Position.X / 16, (int)this.Position.Y / 16];
		
		//descrestem mana
		this.currentMana -= this.stats.getConstantCost() * (1 + (50 - currentVeg) / 200) * Math.Max((1 + (50 - Math.Abs(this.stats.getIdealTemp() - currentTemp)) / 50), 0.25);
 
		//GD.Print(this.currentMana);
		if (this.currentMana <= 0)
		{
			QueueFree();
		}
		AI_Output ai_output = apply_AI_Output();
		GD.Print("Wizbit ", id, ": ", ai_output.moveX, ", ", ai_output.moveY, ", ", ai_output.rotate, ", ", ai_output.reproduce);
		
		Vector2 movement = new Vector2((float)ai_output.moveX, (float)ai_output.moveY) * (float)stats.getMaxMovementSpeed() * (float)Math.Max((1 + (50 - Math.Abs(this.stats.getIdealAlt() - currentAlt)) / 50), 0.25) * (float)0.25;
		
		//GD.Print(movement);
		this.stats.mutateEnv(currentTemp, currentAlt);
		//GD.Print(this.stats.getIdealTemp());

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



