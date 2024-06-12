using Godot;
using System;

public partial class ManaGeneration : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Area2D newMana = new Area2D();
		//Sprite2D manaSprite = new Sprite2D();
		//manaSprite.SetTexture("res://.godot/imported/mana_sprite.png-db0d483bf8ae1ee812b24bb76b428ed7.ctex");
		//AddChild(newMana);
		
		var manaScene = GD.Load<PackedScene>("res://scenes/mana.tscn");
		var manaInstance = manaScene.Instantiate<Node2D>();
		Vector2 manaPosition = new Vector2(900, 900);
		manaInstance.Position = manaPosition;
		AddChild(manaInstance);
		
		var wizbitScene = GD.Load<PackedScene>("res://scenes/wizbit.tscn");
		var wizbitInstance = wizbitScene.Instantiate<Node2D>();
		Vector2 wizbitPosition = new Vector2(1200, 1200);
		wizbitInstance.Position = wizbitPosition;
		AddChild(wizbitInstance);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
