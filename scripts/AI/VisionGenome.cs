using System;
using Godot;

public class VisionGenome : IGenome {

    private double fov; //field of view, 0 for none, 1 for all-round
    private uint rayCountCode; // actual ray count will be max(0, 2*rayCountCode-1)
    private double range; //max viewing range

    public IGenome clone() {
        return new VisionGenome(fov, rayCountCode, range);
    }

    public void mutate() {
        // set up rng
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();

        // Roll for fov
        float random = rng.RandfRange(0, 1);
        if( random < SimulationParameters.AIParameters.MutationChances.fov )
            mutateFOV();
        
        // Roll for rayCountCode
        random = rng.RandfRange(0, 1);
        if( random < SimulationParameters.AIParameters.MutationChances.rayCountCode )
            mutateRayCountCode();
        
        // Roll for range
        random = rng.RandfRange(0, 1);
        if( random < SimulationParameters.AIParameters.MutationChances.range )
            mutateRange();
    }

    private void mutateFOV() {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        float param = (float)SimulationParameters.AIParameters.MutationParameters.visionFOVMaxChange;
        fov=fov*(1+rng.RandfRange(-param, param));
    }

    private void mutateRayCountCode() {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        if(rng.RandfRange(0, 1)<0.5)
            rayCountCode++;
        else if(rayCountCode>0)
            rayCountCode--;
    }

    private void mutateRange() {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        float param = (float)SimulationParameters.AIParameters.MutationParameters.visionRangeMaxChange;
        range=range*(1+rng.RandfRange(-param, param));
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