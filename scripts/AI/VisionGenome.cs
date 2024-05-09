using System;

public class VisionGenome : IGenome {

    private double fov; //field of view, 0 for none, 1 for all-round
    private uint rayCountCode; // actual ray count will be max(0, 2*rayCountCode+1)
    private double range; //max viewing range

    public void mutate() {
        /// !!!! DO SOMETHING
    }

    public IGenome clone() {
        return new VisionGenome(fov, rayCountCode, range);
    }

    public double getFOV() { return fov; }
    public uint getRayCountCode() { return rayCountCode; }
    public double getRange() { return range; }

    public VisionGenome(double fov, uint rayCountCode, double range) {
        this.fov=fov;
        this.rayCountCode=rayCountCode;
        this.range=range;
    }
}