using Godot;
using System;

public partial class WizStats : Window
{
	public int id, generation, neuroncount;
	public double maxHP, maxMana, hp, mana;

	public WizStats() : base()
	{
		
	}

	public WizStats(int i, int generation, int neuroncount, double hp, double maxHP, double mana, double maxMana)
	{
		id = i;
		this.generation = generation;
		this.neuroncount = neuroncount;
		this.maxHP = maxHP;
		this.maxMana = maxMana;
		this.hp = hp;
		this.mana = mana;

		// Calling the update_data method to initialize the stats
		UpdateData();
	}

	public override void _Ready()
	{
		var statsMenu = GetNode<Control>("StatsMenu");
		var closeButton = statsMenu.GetNode<Button>("Close");

		// Connect the button signal to the method
		//closeButton.Connect("pressed", this, nameof(OnClosePressed));
	}

	public override void _EnterTree()
	{
		GD.Print("Entered tree window!!!!!!!!!");
		base._EnterTree();
	}

	public override void _Process(double delta)
	{
		// Your frame-by-frame logic here
	}

	public void UpdateData()
	{
		StatsMenu menu = GetChild<StatsMenu>(0);
		menu.UpdateMenu(id, hp, maxHP, mana, maxMana, generation, neuroncount);
	}

	private void OnClosePressed()
	{
		GD.Print("Let's gooo");
	}
	
	private void _on_close_requested()
	{
	QueueFree();
	}
	
	private void _on_mouse_entered()
	{
		QueueFree();
	}
}





