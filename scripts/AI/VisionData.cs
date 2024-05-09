using System;
using System.Collections.Generic;

public struct VisionData {
    public uint rayCount;
    public VisionRayData[] raysData;

    public VisionData(uint n) {
        if(n==0)
            rayCount=0;
        else
            rayCount=2*n+1;
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