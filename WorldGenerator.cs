using Godot;
using System;
using System.Collections.Generic; 

public partial class WorldGenerator : Node2D
{
	static int height = 256;
	static int width = 256;
	private double[,] tempNoiseMap = new double[height, width];
	private double[,] altNoiseMap = new double[height, width];
	private int[,]  biomeMap = new int[height, width];
	
	//class that defines the values for a tile to be in a specific biome
	private class Biome 
	{ 
		public int key = 0;
		public double mintemp = 0, maxtemp = 0, minalt = 0, maxalt = 0;
		public Biome(int key, double mintemp, double maxtemp, double minalt, double maxalt)
		{
			this.key = key;
			this.mintemp = mintemp;
			this.maxtemp = maxtemp;
			this.minalt = minalt;
			this.maxalt = maxalt;
		}
	}
	Dictionary<String, Biome> BiomeList = new Dictionary<String, Biome>();
	
	//function to add in dictionary biomes
	private void biomeListGen()
	{
		BiomeList.Add("Plains", new Biome(0, 0, 100, 45, 70));
		BiomeList.Add("Ocean", new Biome(1, 0, 100, 0, 40));
		BiomeList.Add("Beach", new Biome(3, 0, 100, 40, 45));
		BiomeList.Add("Mountain", new Biome(2, 0, 100, 70, 100));
	}
	
	//function that generates a noise map used to generate values for altitude, temperature, etc.
	private double[,] noiseMapGen(int height, int width, float frequency, int octaves)
	{
		Random random = new Random();
		var noise = new FastNoiseLite();
		noise.Seed = random.Next();
		noise.DomainWarpFrequency = frequency;
		noise.FractalOctaves = octaves;
		noise.FractalType = FastNoiseLite.FractalTypeEnum.Fbm;
		noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
		double[,] noiseMap = new double[height, width];
		for(int i = 0; i < height; i++)
			for(int j = 0; j < width; j++)
				noiseMap[i, j] = (noise.GetNoise2D(i, j) + 1) * 50;
		return noiseMap;
	}
	
	private void setTile(int x, int y)
	{
		foreach(var biome in BiomeList.Values)
			if(tempNoiseMap[x, y] >= biome.mintemp && tempNoiseMap[x, y] <= biome.maxtemp && altNoiseMap[x, y] >= biome.minalt && altNoiseMap[x, y] <= biome.maxalt)
			{
				biomeMap[x, y] = biome.key;
				GetNode<TileMap>("TileMap").SetCell(0, new Vector2I(x, y), 2, new Vector2I(biome.key, 1), 0);
				return;
			}
		GetNode<TileMap>("TileMap").SetCell(0, new Vector2I(x, y), 2, new Vector2I(0, 1), 0);
		biomeMap[x, y] = 1;
	}
	
	private void makeMap(int height, int width)
	{
		for(int i = 0; i < height; i++)
			for(int j = 0; j < width; j++)
				setTile(i, j);
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tempNoiseMap = noiseMapGen(height, width, 0.1f, 5);
		altNoiseMap = noiseMapGen(height, width, 0.1f, 5);
		biomeListGen();
		makeMap(height, width);
		GD.Print(height);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
