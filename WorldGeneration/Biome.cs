using Godot;

//class that defines the values for a tile to be in a specific biome
//atributes: id, temperature range, altitude range, vegetation range, sprite (position in tileset.png)
//sprites are 8x8 pixels
class Biome 
{ 
	public int key;
	public double mintemp, maxtemp, minalt, maxalt, minveg, maxveg;
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