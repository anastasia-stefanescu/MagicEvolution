using Godot;
using System;
using System.Reflection.Metadata;

public struct AI_Input {
    public double hpFraction;
    public double manaFraction;
    public double movementCost; // relative to max mana
    public static readonly double constant=1;
    public double random;
    public VisionData visionData;
}