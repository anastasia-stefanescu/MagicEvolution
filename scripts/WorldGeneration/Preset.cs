using Godot;

//Example worlds that can be generated
public class Preset
{
	public int key;
	public int height, width, octaves;
	public float biomeSize;
		
	public int tempMod, altMod, vegMod;
	public float tempEx, altEx, vegEx;
		
	public Preset(int key, int height = 512, int width = 512, int octaves = 5, float biomeSize = 1,
				int tempMod = 50, float tempEx = 1, int altMod = 50, float altEx = 1, int vegMod = 50, float vegEx = 1)
	{
		this.key = key;
		this.height = height;
		this.width = width;
		this.octaves = octaves;
		this.tempMod = tempMod;
		this.tempEx = tempEx;
		this.altMod = altMod;
		this.altEx = altEx;
		this.vegMod = vegMod;
		this.vegEx = vegEx;
		this.biomeSize = biomeSize;
	}
}
