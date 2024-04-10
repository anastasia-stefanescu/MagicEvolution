using Godot;
using System;

public partial class WorldGenerator : Node2D
{
	static int height = 256;
	static int width = 256;
	private double[,] tempNoiseMap = new double[height, width];
	private double[,] altNoiseMap = new double[height, width];
	private int[,]  biomeMap = new int[height, width];
	
	private double[,] noiseMapGen(float frequency, int octaves)
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
			for(int j = 0; j < height; j++)
				noiseMap[i, j] = noise.GetNoise2D(i, j) + 0.5;
		return noiseMap;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tempNoiseMap = noiseMapGen(0.1f, 5);
		altNoiseMap = noiseMapGen(0.1f, 5);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
