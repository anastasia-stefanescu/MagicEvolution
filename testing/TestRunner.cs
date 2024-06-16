using Godot;
using System;
using System.IO;
using NUnit.Framework;
using NUnitLite;

public partial class TestRunner : Node
{
	public override void _Ready()
	{
		// Initialize Godot environment if necessary
		GD.Print("Initializing Godot environment...");

		// Run the tests
		RunTests();
	}

	private void RunTests()
	{
		var writer = new StringWriter();
		var extendedTextWriter = new NUnit.Common.ExtendedTextWrapper(writer);
		var result = new AutoRun().Execute(new string[] { }, extendedTextWriter, Console.In);

		GD.Print(writer.ToString());

		// Exit the application after running tests
		GetTree().Quit();
	}
}

