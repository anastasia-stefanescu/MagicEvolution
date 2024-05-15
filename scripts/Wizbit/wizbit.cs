using Godot;
using System;

public partial class wizbit : CharacterBody2D
{
	public WizbitStats date_wizbit;
	public NeuralNetwork neuralNetwork;

	public double rotation = 0;
	private double rotation_speed = 2;
	private double speed = 300.0f;
	private Sprite2D sprite_2d;
	
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
	
	
	public wizbit()
	{
		date_wizbit = new WizbitStats();
		date_wizbit.generate();
		AddChild(date_wizbit);
		var visGen = new VisionGenome(1, 0, 0);
		var nnGenome = new NeuralNetworkGenome(visGen);
		neuralNetwork = new NeuralNetwork(nnGenome);
		AddChild(neuralNetwork);
	}
	
	public wizbit(WizbitStats stats, NeuralNetwork nn, float rotation, float rotation_speed, float speed, Sprite2D sprite)
	{
		this.date_wizbit = stats;
		this.neuralNetwork = nn; //?? copiem doar - trb facut clone 
		this.rotation = rotation;
		this.rotation_speed = rotation_speed;
		this.speed = speed;
		this.sprite_2d = sprite;
	}
	
	public AI_Input construct_AI_input()
	{
		AI_Input ai_input = new AI_Input();

		ai_input.hpFraction = date_wizbit.hpFraction;
		ai_input.manaFraction = date_wizbit.manaFraction;
		ai_input.movementCost = date_wizbit.movementCost;
		//wizbit.constant = date_wizbit.constant; //constanta ar trebui initializata in constructor
		ai_input.random = date_wizbit.random;
		//GD.Print("intram getVisionData()");
		ai_input.visionData = this.neuralNetwork.getVisionData().clone();
		//GD.Print("returnam ai_input, intram in run");
		return ai_input;
	}
	
	public wizbit reproduce()
	{
		wizbit new_wizBit = new wizbit(this.date_wizbit, this.neuralNetwork, (float)this.rotation, (float)this.rotation_speed, (float)this.speed, this.sprite_2d);
		new_wizBit.mutate(); //aici trb apelat alt tip de mutate?
		return new_wizBit;
	}
	
	private void mutate()
	{
		
	}
	
	private Vector2 apply_AI_Output()
	{
		//GD.Print("apply Ai output");
		AI_Output ai_output = neuralNetwork.run(this.construct_AI_input());

		//output: movex, movey, rotate, reproduce
		
		this.rotation = ai_output.rotate; //sau += ??
		Vector2 movement = new Vector2((float)ai_output.moveX, (float)ai_output.moveY).Normalized();
		movement = movement * (float)speed;
		
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
		this.rotation = (int)ai_rotation;
		Velocity = new Vector2((float)ai_x, (float)ai_y) * (float)speed;

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



