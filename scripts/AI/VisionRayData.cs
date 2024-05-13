using System;

public struct VisionRayData {
    public double distance; // relative to max raycast distance
    public double angle; // the angle in which the ray was shot relative to the forward direction (true angle)/(180 degrees)
    public double isManaPellet;
    public double isWizbit;
    //public bool isSpell;
}