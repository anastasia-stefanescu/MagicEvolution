using Godot;
using System;

public partial class Wizbit : CharacterBody2D
{
	public WizbitStats stats;
	public NeuralNetwork neuralNetwork;
	private int id;
	private static int id_generator = 0;
	public int generation;
	public static int global_generation = 1;
	public Label label;
	private double currentHp;
	private double currentMana;
	private static readonly double rotation_speed = 2;

	public Wizbit() {
		
	}
	public override void _EnterTree()
	{   //adaugam in arbore manual "copii" nodului de Wizbit
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
		id = id_generator;
		label = GetChild<Label>(0); 
		label.Text = "Wizbit "+ id; 

		//am modificat ca sa nu se reproduca foarte mult la inceput
		currentHp = stats.getMaxHp();
		currentMana = stats.getMaxMana()*0.5;

		GD.Print("Wizbit ", id, " created");
		SimulationParameters.crtNoWizbits++;
		GD.Print(SimulationParameters.crtNoWizbits, " Wizbits exist");
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

	public int getId(){
		return id;
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
		/*GD.Print("Ai input: hp: ", ai_input.hpFraction, ", mana: ", ai_input.manaFraction, ", mvCost: ", ai_input.movementCost, ", random: ", ai_input.random);
		GD.Print("Ai vision data: raycount: ", ai_input.visionData.rayCount);
		for (int i= 0; i< ai_input.visionData.rayCount; i++)
			GD.Print("   Ray ", i, " : dist: ", ai_input.visionData.raysData[i].distance, " angle: ", ai_input.visionData.raysData[i].angle, " isMana: ", ai_input.visionData.raysData[i].isMana, "isW: ", ai_input.visionData.raysData[i].isWizbit);*/
		return ai_input;
	}
	
	public Wizbit reproduce()
	{
		PackedScene WizbitScene = GD.Load<PackedScene>("res://scenes/wizbit.tscn");
		Wizbit instance = WizbitScene.Instantiate<Wizbit>();

		GD.Print("Wizbit ", id, " cloned. Mutations made:");
		instance.stats = new WizbitStats((WizbitStatsGenome)stats.getGenomeCopy());
		instance.neuralNetwork = new NeuralNetwork((NeuralNetworkGenome)neuralNetwork.getGenomeCopy());
		instance.mutate();

		instance.generation = generation+1;
		if (instance.generation > global_generation)
		{
			global_generation = instance.generation;
			GD.Print(" We've reached ", global_generation, " generations");
		}

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
					if (minim > dist && w2 != this) //alege Wizbitului la distanta minima dintre cei pe care ii vede
					{
						minim = dist;
						w = w2;
					}
				} 
		}
		//ataca Wizbitul aflat la distanta minima
		if (w!= null)
		{
			GD.Print("Wizbit ", this.id, " attacked Wizbit ", w.getId());
			GD.Print(" - wizbit ", w.getId(), " had ", w.getCurrentHp(), " hp");
			w.decreaseHp(0.75 * w.stats.getMaxHp());
			
			GD.Print(" - now he has left: ", w.getCurrentHp(), " hp");
		}
	}
	
	//mutatiile genomurilor Wizbitului - au loc la reproducere
	public void mutate() {
		stats.mutate();
		neuralNetwork.mutate();
	}
	
	//aplicam outputul de ai, in afara de miscare (daca AI-ul returneaza ca Wizbitul trebuie sa se reproduca sau sa atace)
	private  AI_Output apply_AI_Output()
	{
		AI_Input ai_input = construct_AI_input();
		AI_Output ai_output = neuralNetwork.run(ai_input);

		if (ai_output.reproduce > 0.5)
		{ //se reproduce cu conditia sa aiba minim 75% din mana posibila
			if (currentMana >= 0.75 * stats.getMaxMana()) 
			{
				currentMana -= 0.75 * stats.getMaxMana();
				reproduce();
			}
		}
		//ataca cu conditia sa aiba minim 25% din hp-ul posibil
		if (ai_output.cast_spell > 0.5 && currentHp >= 0.25 * stats.getMaxHp()) 
		{
			decreaseHp(0.25 * stats.getMaxHp());
			cast_spell();
		}

		return ai_output;
	}

	public override void _Process(double delta)
	{
		//get biome for the current position of the wizibit
		int x = Math.Max(Math.Min((int)this.Position.X / 8, 511), 0);
		int y = Math.Max(Math.Min((int)this.Position.Y / 8, 511), 0);
		double currentTemp = WorldGenerator.tempNoiseMap[x, y];
		double currentAlt = WorldGenerator.altNoiseMap[x, y];
		double currentVeg = WorldGenerator.vegNoiseMap[x, y];
		
		//descrestem mana bazat pe temperatura curenta si cea adaptata
		double currentCost = this.stats.getConstantCost() * (1 - currentVeg / 200) * Math.Max((1 - (50 - Math.Abs(this.stats.getIdealTemp() - currentTemp)) / 50), 0.25);
		currentMana -= currentCost;
		if (currentHp <= stats.getMaxHp() - currentCost)
			currentHp += currentCost; 
		
		//daca i se termina Hp-ul sau mana
		if (currentMana <= 0 || currentHp <= 0)
		{
			//daca i se termina hp dar mai are mana (este atacat), reciclam mana pe care o avea
			if (currentHp <= 0 && currentMana > 0)
			{
				PackedScene ManaScene = GD.Load<PackedScene>("res://scenes/mana.tscn");
				int cate_mane = (int)(currentMana / SimulationParameters.ManaValue);
				for (int i = 0; i < cate_mane; i++)
				{
					Mana instance = ManaScene.Instantiate<Mana>();
					instance.Position = Position;
					GetTree().Root.CallDeferred("add_child", instance);
				}
				GD.Print("Wizbit ", id, " was killed by spell");
			} 
			else //altfel, a murit de de foame
			{
				if (this.currentMana <= 0)
					GD.Print("Wizbit ", id, " died of hunger");
			}
			QueueFree();
			SimulationParameters.crtNoWizbits--;
			GD.Print(SimulationParameters.crtNoWizbits, " Wizbits remaining");
		}
 
		AI_Output ai_output = apply_AI_Output();
		//GD.Print("Wizbit ", id, ": ", ai_output.moveX, ", ", ai_output.moveY, ", ", ai_output.rotate, ", ", ai_output.reproduce);
		
		stats.mutateEnv(currentTemp, currentAlt);

		//viteza bazata pe terenul curent si cel adaptat
		Vector2 movement = new Vector2((float)ai_output.moveX, (float)ai_output.moveY) * (float)stats.getMaxMovementSpeed() * (float)Math.Max((1 + (20 - Math.Abs(this.stats.getIdealAlt() - currentAlt)) / 20), 0.4) * (float)0.25;
		
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



