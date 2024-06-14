using Godot;
using System;
using System.Collections.Generic; 

public partial class WorldGenerator : Node2D
{
	static int height = 512;
	static int width = 512;
	static public double[,] tempNoiseMap = new double[height, width];
	static public double[,] altNoiseMap = new double[height, width];
	static public double[,] vegNoiseMap = new double[height, width];
	static public int[,]  biomeMap = new int[height, width];
	
	Dictionary<String, Biome> BiomeList = new Dictionary<String, Biome>();
	Dictionary<String, Preset> WorldPresets = new Dictionary<String, Preset>();
	
	//function to add biomes in dictionary
	//parameters for biomes: id, min_temp, max_temp, min_alt, max_alt, min_veg, max_veg, sprite
	//qualifications for biomes:
	
	//alt < 10 - deep ocean
	//10 < alt < 40 - ocean
	//40 < alt < 45 - seashore
	//45 < alt < 70 - plains
	//70 < alt - mountains
	
	//temp < 30 - cold
	//30 < temp < 70 - normal
	//70 < temp < 90 - hot
	//90 < temp - lava
	
	//veg < 70 - normal
	//70 < veg - lush version of certain biomes
	//exceptions: flower field (60 < veg < 70), oasis (85 < veg)
	private void biomeListGen()
	{
		BiomeList.Add("DeepOcean", new Biome(0, 0, 90, -1000, 10, -1000, 1000, new Vector2I(2, 2)));
		
		BiomeList.Add("GlacierOcean", new Biome(1, -1000, 30, 10, 40, -1000, 1000, new Vector2I(3, 2)));
		BiomeList.Add("Ocean", new Biome(2, 30, 70, 10, 40, -1000, 70, new Vector2I(0, 2)));
		BiomeList.Add("AlgaeOcean", new Biome(2, 30, 70, 10, 40, 70, 1000, new Vector2I(6, 2)));
		BiomeList.Add("WarmOcean", new Biome(3, 70, 90, 10, 40, -1000, 70, new Vector2I(1, 2)));
		BiomeList.Add("CoralReef", new Biome(3, 70, 90, 10, 40, 70, 1000, new Vector2I(5, 2)));
		BiomeList.Add("LavaOcean", new Biome(4, 90, 1000, -1000, 40, -1000, 1000, new Vector2I(4, 2)));
		
		BiomeList.Add("StoneShore", new Biome(5, -1000, 30, 40, 45, -1000, 1000, new Vector2I(2, 1)));
		BiomeList.Add("GravelBeach", new Biome(6, 30, 40, 40, 45, -1000, 1000, new Vector2I(1, 1)));
		BiomeList.Add("GravelBeach2", new Biome(6, 40, 50, 40, 45, -1000, 70, new Vector2I(1, 1)));
		BiomeList.Add("Beach", new Biome(7, 50, 70, 40, 45, -1000, 70, new Vector2I(0, 1)));
		BiomeList.Add("Swamp", new Biome(8, 40, 70, 40, 45, 70, 1000, new Vector2I(3, 1)));
		
		BiomeList.Add("SnowPlains", new Biome(9, -1000, 30, 45, 70, -1000, 70, new Vector2I(3, 0)));
		BiomeList.Add("Tundra", new Biome(10, -1000, 30, 45, 70, 70, 1000, new Vector2I(5, 0)));
		BiomeList.Add("Plains", new Biome(11, 30, 70, 45, 70, -1000, 60, new Vector2I(0, 0)));
		BiomeList.Add("FlowerField", new Biome(11, 30, 70, 45, 70, 60, 70, new Vector2I(6, 0)));
		BiomeList.Add("Forest", new Biome(12, 30, 70, 45, 70, 70, 1000, new Vector2I(1, 0)));
		BiomeList.Add("Desert", new Biome(13, 70, 90, 40, 70, -1000, 85, new Vector2I(2, 0)));
		BiomeList.Add("Oasis", new Biome(13, 70, 90, 40, 70, 85, 1000, new Vector2I(7, 0)));
		BiomeList.Add("MagmaPlains", new Biome(14, 90, 1000, 40, 70, -1000, 1000, new Vector2I(4, 0)));
		
		BiomeList.Add("SnowMountains", new Biome(15, -1000, 30, 70, 1000, -1000, 1000, new Vector2I(2, 3)));
		BiomeList.Add("Mountains", new Biome(16, 30, 70, 70, 1000, -1000, 70, new Vector2I(0, 3)));
		BiomeList.Add("GreenMountains", new Biome(17, 30, 70, 70, 1000, 70, 1000, new Vector2I(1, 3)));
		BiomeList.Add("Canyon", new Biome(18, 70, 90, 70, 1000, -1000, 1000, new Vector2I(3, 3)));
		BiomeList.Add("Vulcanoes", new Biome(19, 90, 1000, 70, 1000, -1000, 1000, new Vector2I(4, 3)));
	}
	
	private void worldPresetsGen()
	{
		WorldPresets.Add("Normal", new Preset(0));
		WorldPresets.Add("DesertedWorld", new Preset(1, tempMod: 82, tempEx: 0.35f, altMod: 60, altEx: 0.7f));
		WorldPresets.Add("FireAndIce", new Preset(2, tempEx: 5));
		WorldPresets.Add("LushFields", new Preset(3, tempEx: 0.7f, vegMod: 75, vegEx: 0.5f));
		WorldPresets.Add("Atlantis", new Preset(4, tempMod: 60, tempEx: 0.7f, altMod: 20, altEx: 0.7f));
		WorldPresets.Add("Siberia", new Preset(5, tempMod: 20, tempEx: 0.3f, altMod: 60, altEx: 0.8f, vegMod: 70));
		WorldPresets.Add("Contemporary", new Preset(6, octaves: 1));
		WorldPresets.Add("HugeBiomes", new Preset(7, octaves: 8, biomeSize: 2));
	}
	
	//function that generates a noise map used to generate values for altitude, temperature and vegetation levels for a certain point on the map
	//levels are normally between 0 and 100, can go between -1000 and 1000
	//height, width - size of the map, 512x512 normal
	//octaves - how detailed the generation is, normally 5, 8 for big biomes, 1 for smooth world
	//modifier - average level for the generated atribute
	//extreme - how much do the levels variate, normally 1
	//lower for more consistent world, higher for more chaotic, more cold/lava and ocean/mountain biomes and closer to each other
	//biomeSize - how much do the biomes extend in the world, normally 1, 2 for big biomes
	private double[,] noiseMapGen(int height = 512, int width = 512, int octaves = 5, int modifier = 50, float extreme = 1, float biomeSize = 1)
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
				//GetNoise2D generates a number between [-1, 1] for level
				//biomeSize influences how close the positions for the levels are, closer are more similar, further more diferent
				//extreme is a multiplier for GetNoise2D, higher extreme will generate way bigger positive numbers and smaller negative numbers
				//modifier is average level
				noiseMap[i, j] = noise.GetNoise2D((float)i/biomeSize, (float)j/biomeSize) * extreme * 50 + modifier;
		return noiseMap;
	}
	
	//function for asigning a biome for a certain tile
	//goes through all biomes in the dictionary
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
	
	//for each tile on the map, the function for asigning a biome is called
	private void makeMap(int height, int width)
	{
		for(int i = 0; i < height; i++)
			for(int j = 0; j < width; j++)
				setTile(i, j);
	}
	
	private void generateWorld(Preset world)
	{
		tempNoiseMap = noiseMapGen(world.height, world.width, world.octaves, world.tempMod, world.tempEx, world.biomeSize);
		altNoiseMap = noiseMapGen(world.height, world.width, world.octaves, world.altMod, world.altEx, world.biomeSize);
		vegNoiseMap = noiseMapGen(world.height, world.width, world.octaves, world.vegMod, world.vegEx, world.biomeSize);
		makeMap(world.height, world.width);
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		biomeListGen();
		worldPresetsGen();
		//generate map
		generateWorld(WorldPresets["DesertedWorld"]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
