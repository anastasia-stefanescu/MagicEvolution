using System;
using Godot;

public struct Synapse {
    public uint sourceIndex;
    public uint destinationIndex;
    public double weight;

    public Synapse(uint sourceIndex, uint destinationIndex, double weight) {
        this.sourceIndex=sourceIndex;
        this.destinationIndex=destinationIndex;
        this.weight=weight;
    }
}