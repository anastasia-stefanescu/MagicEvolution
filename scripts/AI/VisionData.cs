using Godot;
using System;
using System.Collections.Generic;

public struct VisionData {
	public uint rayCount;
	public VisionRayData[] raysData;
	
	public VisionData() 
	{
		this.rayCount=(uint)SimulationParameters.visionData_initialRayCount;
		raysData = new VisionRayData[rayCount];
	}

	public VisionData(uint rayCount) 
	{
		this.rayCount=rayCount;
		raysData = new VisionRayData[rayCount];
		
	}

	public VisionRayData[] cloneRaysData() {
		return (VisionRayData[])raysData.Clone();
	}

	public VisionData clone() {
		
		VisionData clone = new VisionData((rayCount+1)/2);
		clone.raysData = (VisionRayData[])raysData.Clone();
		return clone;
	}
}
