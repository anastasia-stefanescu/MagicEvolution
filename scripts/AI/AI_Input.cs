using Godot;
using System;
using System.Reflection.Metadata;

public struct AI_Input {
    public double hpPercent;
    public double manaPercent;
    public double movementCost; // relative to max mana
    public static readonly double constant=1;
    public double random;
    public VisionData visionData;
}