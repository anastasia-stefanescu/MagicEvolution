using Godot;
using System;

public partial class WizbitStats : Godot.Node{

	public double hpFraction;
	public double manaFraction;
	public double movementCost; // relative to max mana
	public static readonly double constant=1;
	public double random;
	public VisionData visionData;
}
