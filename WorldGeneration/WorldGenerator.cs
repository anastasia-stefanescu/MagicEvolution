using Godot;
using System;
using System.Collections.Generic; 

public partial class WorldGenerator : Node2D
{
	static int height = 512;
	static int width = 512;
	private double[,] tempNoiseMap = new double[height, width];
	private double[,] altNoiseMap = new double[height, width];
	private double[,] vegNoiseMap = new double[height, width];
	private int[,]  biomeMap = new int[height, width];
	
	//class that defines the values for a tile to be in a specific biome
	private class Biome 
	{ 
		public int key;
		public double mintemp = 0, maxtemp = 0, minalt = 0, maxalt = 0, minveg, maxveg;
		public Vector2I tile;
		public Biome(int key, double mintemp, double maxtemp, double minalt, double maxalt, double minveg, double maxveg, Vector2I tile)
		{
			this.key = key;
			this.mintemp = mintemp;
			this.maxtemp = maxtemp;
			this.minalt = minalt;
			this.maxalt = maxalt;
			this.minveg = minveg;
			this.maxveg = maxveg;
			this.tile = tile;
		}
	}
	Dictionary<String, Biome> BiomeList = new Dictionary<String, Biome>();
	
	//function to add in dictionary biomes
	private void biomeListGen()
	{
		BiomeList.Add("DeepOcean", new Biome(0, 30, 90, -1000, 10, -1000, 1000, new Vector2I(2, 2)));
		
		BiomeList.Add("GlacierOcean", new Biome(1, -1000, 30, -1000, 40, -1000, 1000, new Vector2I(3, 2)));
		BiomeList.Add("Ocean", new Biome(2, 30, 70, 10, 40, -1000, 1000, new Vector2I(0, 2)));
		BiomeList.Add("WarmOcean", new Biome(3, 70, 90, 10, 40, -1000, 1000, new Vector2I(1, 2)));
		BiomeList.Add("LavaOcean", new Biome(4, 90, 1000, -1000, 40, -1000, 1000, new Vector2I(4, 2)));
		
		BiomeList.Add("StoneShore", new Biome(5, -1000, 30, 40, 45, -1000, 1000, new Vector2I(2, 1)));
		BiomeList.Add("GravelBeach", new Biome(6, 30, 40, 40, 45, -1000, 1000, new Vector2I(1, 1)));
		BiomeList.Add("GravelBeach2", new Biome(6, 40, 50, 40, 45, -1000, 70, new Vector2I(1, 1)));
		BiomeList.Add("Beach", new Biome(7, 50, 70, 40, 45, -1000, 70, new Vector2I(0, 1)));
		BiomeList.Add("Swamp", new Biome(8, 40, 70, 40, 45, 70, 1000, new Vector2I(3, 1)));
		
		BiomeList.Add("SnowPlains", new Biome(9, -1000, 30, 45, 70, -1000, 70, new Vector2I(3, 0)));
		BiomeList.Add("Tundra", new Biome(10, -1000, 30, 45, 70, 70, 1000, new Vector2I(5, 0)));
		BiomeList.Add("Plains", new Biome(11, 30, 70, 45, 70, -1000, 70, new Vector2I(0, 0)));
		BiomeList.Add("Forest", new Biome(12, 30, 70, 45, 70, 70, 1000, new Vector2I(1, 0)));
		BiomeList.Add("Desert", new Biome(13, 70, 90, 40, 70, -1000, 1000, new Vector2I(2, 0)));
		BiomeList.Add("MagmaPlains", new Biome(14, 90, 1000, 40, 70, -1000, 1000, new Vector2I(4, 0)));
		
		BiomeList.Add("SnowMountains", new Biome(15, -1000, 30, 70, 1000, -1000, 1000, new Vector2I(2, 3)));
		BiomeList.Add("Mountains", new Biome(16, 30, 70, 70, 1000, -1000, 70, new Vector2I(0, 3)));
		BiomeList.Add("GreenMountains", new Biome(17, 30, 70, 70, 1000, 70, 1000, new Vector2I(1, 3)));
		BiomeList.Add("Canyon", new Biome(18, 70, 90, 70, 1000, -1000, 1000, new Vector2I(3, 3)));
		BiomeList.Add("Vulcanoes", new Biome(19, 90, 1000, 70, 1000, -1000, 1000, new Vector2I(4, 3)));
	}
	
	//function that generates a noise map used to generate values for altitude, temperature, etc.
	private double[,] noiseMapGen(int height, int width, int octaves, int modifier = 50, double extreme = 1)
	{
		Random random = new Random();
		var noise = new FastNoiseLite();
		noise.Seed = random.Next();
		noise.DomainWarpFrequency = 0.1f;
		noise.FractalOctaves = octaves;
		noise.FractalType = FastNoiseLite.FractalTypeEnum.Fbm;
		noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
		double[,] noiseMap = new double[height, width];
		for(int i = 0; i < height; i++)
			for(int j = 0; j < width; j++)
				noiseMap[i, j] = noise.GetNoise2D(i, j) * extreme * 50 + modifier;
		return noiseMap;
	}
	
	private void setTile(int x, int y)
	{
		foreach(var biome in BiomeList.Values)
			if(tempNoiseMap[x, y] >= biome.mintemp && tempNoiseMap[x, y] <= biome.maxtemp && altNoiseMap[x, y] >= biome.minalt 
			&& altNoiseMap[x, y] <= biome.maxalt && vegNoiseMap[x, y] >= biome.minveg && vegNoiseMap[x, y] <= biome.maxveg)
			{
				biomeMap[x, y] = biome.key;
				GetNode<TileMap>("TileMap").SetCell(0, new Vector2I(x, y), 2, new Vector2I(biome.tile.X, biome.tile.Y), 0);
				return;
			}
		GetNode<TileMap>("TileMap").SetCell(0, new Vector2I(x, y), 2, new Vector2I(0, 0), 0);
		biomeMap[x, y] = 11;
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
		tempNoiseMap = noiseMapGen(height, width, 5, 50);
		altNoiseMap = noiseMapGen(height, width, 5, 50);
		vegNoiseMap = noiseMapGen(height, width, 5, 50);
		biomeListGen();
		makeMap(height, width);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
