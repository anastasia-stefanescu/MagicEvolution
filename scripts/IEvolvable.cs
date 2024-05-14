using Godot;
using System;

public interface IEvolvable {
	public void generate();
	public void mutate();
}
