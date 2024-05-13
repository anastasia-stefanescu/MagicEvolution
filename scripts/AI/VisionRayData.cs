using System;

public struct VisionRayData {

    /// <summary>
    /// This is used to calculate the number of input neurons and should 
    /// ALWAYS BE KEPT UP TO DATE.
    /// </summary>
    public static readonly uint fieldCount=4;
    public double distance; // relative to max raycast distance
    public double angle; // the angle in which the ray was shot relative to the forward direction (true angle)/(180 degrees)
    public bool isManaPellet;
    public bool isWizbit;
    //public bool isSpell;
}