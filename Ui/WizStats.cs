using Godot;
using System;

public partial class WizStats : Window
{
	private int id, generation, neuroncount;
	private double maxHP, maxMana, hp, mana;
	
	public override void _Ready()
	{
		var StatsMenu = GetNode<Control>("StatsMenu");
		var closeButton = StatsMenu.GetNode<Button>("Close");
	}
	
	public override void _EnterTree()
	{  
		base._EnterTree();
	}
	
	public WizStats(int id, int generation, int neuroncount, double hp, double maxHP, double mana, double maxMana)
	{
		this.id = id;
		this.generation = generation;
		this.neuroncount = neuroncount;
		this.maxHP = maxHP;
		this.maxMana = maxMana;
		this.hp = hp;
		this.mana = mana;
		
		update_data();
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public void update_data()
	{
		var menu = GetChild<Control>(0);
		menu.Call("update_menu", id, hp, maxHP, mana, maxMana, generation, neuroncount);
	}
	
	private void _on_close_pressed()
	{
		GD.Print("Lets gooo");
	}
}
