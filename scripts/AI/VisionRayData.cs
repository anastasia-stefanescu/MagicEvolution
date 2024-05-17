using System;

public struct VisionRayData {

	/// <summary>
	/// This is used to calculate the number of input neurons and should 
	/// ALWAYS BE KEPT UP TO DATE.
	/// </summary>
	public static readonly uint fieldCount=4;
	
	/// <summary>
	/// relative to max raycast distance
	/// </summary>
	public double distance; 
	
	/// <summary>
	/// the angle in which the ray was shot relative to the forward direction (true angle)/(180 degrees)
	/// </summary>
	public double angle;
	
	/// <summary>
	/// should be either 1 or 0
	/// </summary>
	public double isMana;

	/// <summary>
	/// should be either 1 or 0
	/// </summary>
	public double isWizbit;
	//public bool isSpell;
}
