using Godot;
using System;

public partial class StatsMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GetNode<RichTextLabel>("Text/Valori/Id").Text = "-";
		// GetNode<RichTextLabel>("Text/Valori/Hp").Text = "-";
		// GetNode<RichTextLabel>("Text/Valori/Mana").Text = "-";
		// GetNode<RichTextLabel>("Text/Valori/Gen").Text = "-";
		// GetNode<RichTextLabel>("Text/Valori/Neuron").Text = "-";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Your frame-by-frame logic here
	}

	public void UpdateMenu(int id, double hp, double maxHp, double mana, double maxMana, int gen, int neurons)
	{
		//GetNode<Label>("Text/Valori/Id").Text = $"[Center]{id}";

		RichTextLabel idLabel = GetNode<RichTextLabel>("Text/Valori/Id");
		idLabel.Text = $"{id}";
		GD.Print(idLabel.Text);
		RichTextLabel HpLabel = GetNode<RichTextLabel>("Text/Valori/Hp");
		HpLabel.Text = $"{hp}";
		GD.Print(HpLabel.Text);
		RichTextLabel ManaLabel = GetNode<RichTextLabel>("Text/Valori/Mana");
		ManaLabel.Text = $"{mana}";
		RichTextLabel genLabel = GetNode<RichTextLabel>("Text/Valori/Gen");
		genLabel.Text = $"{gen}";
		GD.Print(genLabel.Text);
		RichTextLabel nLabel = GetNode<RichTextLabel>("Text/Valori/Neuron");
		nLabel.Text = $"{neurons}";
	}
}

