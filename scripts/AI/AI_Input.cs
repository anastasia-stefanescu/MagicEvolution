using Godot;
using System;
using System.Reflection.Metadata;

public struct AI_Input {

    /// <summary>
    /// This is used to calculate the number of input neurons and should 
    /// ALWAYS BE KEPT UP TO DATE.
    /// </summary>
    public static readonly uint nonVisionDataFieldCount=5;
    public double hpFraction;
    public double manaFraction;
    public double movementCost; // relative to max mana
    public static readonly double constant=1;
    public double random;
    public VisionData visionData;
}