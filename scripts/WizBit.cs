using Godot;
using System;

public partial class WizBit : Godot.CharacterBody2D {
	WizBitStats date_wizbit = new WizBitStats();
	AI ai = new NeuralNetwork();

	public double rotation = 0;
	private double rotation_speed = 2
	private double speed = 300;
	private AnimatedSprite2D animated_sprite_2d;

	//trb si constructor fara parametri pt instantierea initiala??

	//constructorul pentru reproducere
	public WizBit(WizBitStats stats, AI ai, double rotation, double rotation_speed, double speed, AnimatedSprite2D animation)
	{
		this.date_wizbit = stats;
		this.ai = ai //?? copiem doar
		this.rotation = rotation;
		this.rotation_speed = rotation_speed
		this.speed = speed;
		this.animated_sprite_2d = animation
	}

	public AI_Input construct_AI_input()
	{
		AI_Input ai_input = new AI_Input();

		ai_input.hpPercent = date_wizbit.hpPercent;
		ai_input.manaPercent = date_wizbit.manaPercent;
		ai_input.movementCost = date_wizbit.movementCost;
		ai_input.constant = date_wizbit.constant //constanta ar trebui initializata in constructor
		ai_input.random = date_wizbit.random;
		ai_input.visionData = this.ai.getVisionData().clone();

		return ai_input;
	}

	public WizBit reproduce()
	{
		WizBit new_wizBit = new WizBit(this.date_wizbit, this.ai, this.rotation, this.rotation_speed, this.speed, this.animated_sprite_2d);
		new_wizBit.mutate(); //aici trb apelat alt tip de mutate?
		return new_wizBit;
	}

	private void mutate()
	{
		
	}

	public override void _Ready()
	{
		// var test1_a = new Test1(); 
		Test2 test2_a = new Test2();
		// Console.WriteLine(test1_a.Whatever()); 
		Console.WriteLine(test2_a.Whenever());
	}

	private Vector2 apply_AI_Output()
	{
		AI_Output ai_output = ai.run(this.construct_AI_input());

		//output: movex, movey, rotate, reproduce
		
		this.rotation = ai_output.rotate; //sau += ??
		Vector2 movement = new Vector2(ai_output.moveX, ai_output.moveY).Normalized();
		movement = movement.Rotated(this.rotation * rotation_speed) * this.speed;

		if (ai_output.reproduce > 0.5)
		{
			this.reproduce();
		}

		return movement;


		//////////COMPORTAMENT DEFAULT 
		// Vector2 velocity = Vector2.Zero;
		// rotation_dir += ai_rotation;
		// velocity.x += ai_x;
		// velocity.y += ai_y;

		// velocity *= speed;
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector2 movement = apply_AI_Output();

		if (Math.Abs(velocity.x) > 0 || Math.Abs(velocity.y) > 0)
			animated_sprite_2d.Play("running");
		else
			animated_sprite_2d.Play("idle");

		movement = movement.Rotated(this.rotation);
		MoveAndSlide(movement);
	}
}
