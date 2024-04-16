using Godot;
using System;

public struct VisionData {
    public uint rayCount;
    private VisionRayData[] raysData;

    public VisionData(uint n) {
        rayCount=Math.Max(0, 2*n-1);
        raysData = new VisionRayData[rayCount];
    }

    public VisionRayData[] getRaysData() {
        return (VisionRayData[])raysData.Clone();
    }

    public void setRaysData(VisionRayData[] raysData) {
        this.raysData=(VisionRayData[])raysData.Clone();
        rayCount=(uint)this.raysData.Length;
    }
}