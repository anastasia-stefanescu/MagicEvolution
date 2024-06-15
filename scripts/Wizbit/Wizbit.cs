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
	
	//ready se apeleaza dupa _EnterTree
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

	public void decreaseHp(double val){
		this.currentHp -= val;
	}

	public double getCurrentHp(){
		return currentHp;
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

		//GD.Print("Ai input: hp: ", ai_input.hpFraction, ", mana: ", ai_input.manaFraction, ", mvCost: ", ai_input.movementCost, ", random: ", ai_input.random);
		//GD.Print("Ai vision data: raycount: ", ai_input.visionData.rayCount);
		//for (int i= 0; i< ai_input.visionData.rayCount; i++)
			//GD.Print("   Ray ", i, " : dist: ", ai_input.visionData.raysData[i].distance, " angle: ", ai_input.visionData.raysData[i].angle, " isMana: ", ai_input.visionData.raysData[i].isMana, "isW: ", ai_input.visionData.raysData[i].isWizbit);
		//
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

	public void cast_spell()
	{
		//mai verifica o data ce vede wizbit 

		Vision v = this.neuralNetwork.getVision_to_reuse();
		uint rc = v.getRayCount();
		RayCast2D[] rays = v.getRays();
		Wizbit w = null;
		float minim = 1000000000;
		for (int i = 0; i< rc; i++)
			if(rays[i].IsColliding()) {
				if(rays[i].GetCollider() as Wizbit != null ) { 
					Wizbit w2 = rays[i].GetCollider() as Wizbit;
					Vector2 punct_coliziune = rays[i].GetCollisionPoint();
					float dist = this.Position.DistanceTo(punct_coliziune);
					if (minim > dist)
					{
						minim = dist;
						w = w2;
					}
				} 
			}
		if (w!= null)
		{
			GD.Print("Wizbit avea: ", w.getCurrentHp());
			w.decreaseHp(0.75 * w.stats.getMaxHp());
			this.decreaseHp(0.15 * this.stats.getMaxHp());
			GD.Print("A fost atacat, mai are: ", w.getCurrentHp());
			//trb spawnat un obiect 'efect vraja' care are durata de viata de cateva frameuri de la spawnare
		}
	}
	
	public void mutate() {
		stats.mutate();
		currentHp=stats.getMaxHp();
		currentMana=stats.getMaxMana();

		neuralNetwork.mutate();
	}
	
	private  AI_Output apply_AI_Output()
	{
		AI_Input ai_input = this.construct_AI_input();
		AI_Output ai_output = neuralNetwork.run(ai_input);

		if (ai_output.reproduce > 0.5)
		{
			//GD.Print(this.id, " trebuie sa se reproduca, are : " , this.currentMana, ", ", this.stats.getMaxMana());
			if (this.currentMana >= 0.75 * this.stats.getMaxMana())
			{
				this.currentMana -= 0.75 * this.stats.getMaxMana();
				this.reproduce();
			}
		}

		// aici va fi si pt vraji
		// if (ai_output.cast_spell > 0.5 && this.currentHp >= 0.15 * this.stats.getMaxMana())
		// {	
		// 	this.cast_spell();
		// }

		return ai_output;
	}

	public override void _Process(double delta)
	{
		//descrestem mana
		this.currentMana -= this.stats.getConstantCost(); 
		if (this.currentHp <= this.stats.getMaxHp() - this.stats.getConstantCost())
			this.currentHp += this.stats.getConstantCost(); 
		if (this.currentMana <= 0 || this.currentHp <= 0)
		{
			if (this.currentHp <= 0 && this.currentMana > 0)
			{
				PackedScene ManaScene = GD.Load<PackedScene>("res://scenes/mana.tscn");
				int cate_mane = (int)(this.currentMana / SimulationParameters.ManaValue);
				for (int i = 0; i < cate_mane; i++)
				{
					Mana instance = ManaScene.Instantiate<Mana>();
					instance.Position = this.Position;
					GetTree().Root.CallDeferred("add_child", instance);
				}

			} 
			QueueFree();
		}
		//cast_spell();

		AI_Output ai_output = apply_AI_Output();
		//GD.Print("Wizbit ", id, ": ", ai_output.moveX, ", ", ai_output.moveY, ", ", ai_output.rotate, ", ", ai_output.reproduce);
		
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



