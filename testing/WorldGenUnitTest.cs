using Godot;
using System;
using NUnit.Framework;

namespace WorldGen.Tests
{
	[TestFixture]
	public class WorldGenTests
	{
		[Test]
		public void NormalGenTest()
		{
			WorldGenerator world = new WorldGenerator();
			world.worldPresetsGen();
			world.generateWorld(world.WorldPresets["Normal"]);
			double avgTemp = 0, avgAlt = 0, avgVeg = 0;
			for(int i = 0; i < WorldGenerator.height; i++)
				for(int j = 0; j < WorldGenerator.width; j++)
				{
					avgTemp += WorldGenerator.tempNoiseMap[i, j];
					avgAlt += WorldGenerator.altNoiseMap[i, j];
					avgVeg += WorldGenerator.vegNoiseMap[i, j];
				}
			avgTemp /= WorldGenerator.height * WorldGenerator.width;
			avgAlt /= WorldGenerator.height * WorldGenerator.width;
			avgVeg /= WorldGenerator.height * WorldGenerator.width;
			Assert.Multiple(() =>
			{
				Assert.Greater(avgTemp, 40);
				Assert.Greater(avgAlt, 40);
				Assert.Greater(avgVeg, 40);
				Assert.Less(avgTemp, 60);
				Assert.Less(avgAlt, 60);
				Assert.Less(avgVeg, 60);
			});
		}

		[Test]
		public void DesertedWorldGenTest()
		{
			WorldGenerator world = new WorldGenerator();
			world.worldPresetsGen();
			world.generateWorld(world.WorldPresets["DesertedWorld"]);
			double avgTemp = 0, avgAlt = 0, avgVeg = 0;
			for(int i = 0; i < WorldGenerator.height; i++)
				for(int j = 0; j < WorldGenerator.width; j++)
				{
					avgTemp += WorldGenerator.tempNoiseMap[i, j];
					avgAlt += WorldGenerator.altNoiseMap[i, j];
					avgVeg += WorldGenerator.vegNoiseMap[i, j];
				}
			avgTemp /= WorldGenerator.height * WorldGenerator.width;
			avgAlt /= WorldGenerator.height * WorldGenerator.width;
			avgVeg /= WorldGenerator.height * WorldGenerator.width;
			Assert.Multiple(() =>
			{
				Assert.Greater(avgTemp, 77);
				Assert.Greater(avgAlt, 55);
				Assert.Greater(avgVeg, 40);
				Assert.Less(avgTemp, 87);
				Assert.Less(avgAlt, 65);
				Assert.Less(avgVeg, 60);
			});
		}
		
		[Test]
		public void AtlantisGenTest()
		{
			WorldGenerator world = new WorldGenerator();
			world.worldPresetsGen();
			world.generateWorld(world.WorldPresets["Atlantis"]);
			double avgTemp = 0, avgAlt = 0, avgVeg = 0;
			for(int i = 0; i < WorldGenerator.height; i++)
				for(int j = 0; j < WorldGenerator.width; j++)
				{
					avgTemp += WorldGenerator.tempNoiseMap[i, j];
					avgAlt += WorldGenerator.altNoiseMap[i, j];
					avgVeg += WorldGenerator.vegNoiseMap[i, j];
				}
			avgTemp /= WorldGenerator.height * WorldGenerator.width;
			avgAlt /= WorldGenerator.height * WorldGenerator.width;
			avgVeg /= WorldGenerator.height * WorldGenerator.width;
			Assert.Multiple(() =>
			{
				Assert.Greater(avgTemp, 55);
				Assert.Greater(avgAlt, 15);
				Assert.Greater(avgVeg, 40);
				Assert.Less(avgTemp, 65);
				Assert.Less(avgAlt, 25);
				Assert.Less(avgVeg, 60);
			});
		}
	}
}

/*public partial class WorldGenUnitTest : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}*/
