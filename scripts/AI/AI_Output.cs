using System;

public struct AI_Output {

	/// <summary>
	/// This is used to calculate the number of output neurons and should
	/// ALWAYS BE KEPT UP TO DATE
	/// </summary>
	public static readonly uint fieldCount=5;
	public double moveX;
	public double moveY;
	public double rotate;
	public double reproduce;
	public double cast_spell;
}
