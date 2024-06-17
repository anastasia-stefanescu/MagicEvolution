using Godot;
using System;


public partial class Stats : Window
{
	public int no_W, generation;

	public Stats() : base()
	{
		
	}

	public Stats(int i, int generation)
	{
		no_W = i;
		this.generation = generation;
		
		// Calling the update_data method to initialize the stats
		UpdateData();
	}

	public override void _Ready()
	{
	
	}

	public override void _EnterTree()
	{
		base._EnterTree();
	}

	public void UpdateData()
	{
		Menu menu = GetChild<Menu>(0);
		menu.UpdateMenu(no_W, generation);
	}
}



