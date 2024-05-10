using System;

public interface IGenome {

    public abstract void mutate();

    public abstract IGenome clone();
}