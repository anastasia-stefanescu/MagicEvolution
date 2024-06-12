using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	Vector2 newPosition;
	Vector2 newZoom;
	int cameraLevel = 4;
	
	Vector2[] cameraLevels = {new Vector2(0.5f, 0.5f), new Vector2(0.6f, 0.6f), new Vector2(0.75f, 0.75f), new Vector2(0.8f, 0.8f),
							new Vector2(1, 1), new Vector2(1.5f, 1.5f), new Vector2(2, 2), new Vector2(2.5f, 2.55f), new Vector2(3.25f, 3.25f),
							new Vector2(4, 4)};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		newPosition = this.Position;
		//GD.Print(newPosition);
		newZoom = cameraLevels[cameraLevel];
		
		if(Input.IsActionPressed("move_right") && this.Position.X < 4096 - 400 / this.Zoom.X)
			newPosition.X = (this.Position.X + 16);
		if(Input.IsActionPressed("move_left") && this.Position.X > 400 / this.Zoom.X)
			newPosition.X = (this.Position.X - 16);
		if(Input.IsActionPressed("move_down") && this.Position.Y < 4096 - 200 / this.Zoom.Y)
			newPosition.Y = (this.Position.Y + 16);
		if(Input.IsActionPressed("move_up") && this.Position.Y > 200 / this.Zoom.Y)
			newPosition.Y = (this.Position.Y - 16);
			
		if(Input.IsActionJustPressed("zoom_in") && cameraLevel < 9)
		{
			cameraLevel += 1;
			newZoom = cameraLevels[cameraLevel];
			//newZoom.X = (this.Zoom.X + 0.01f);
			//newZoom.Y = (this.Zoom.Y + 0.01f);
		}
		if(Input.IsActionJustPressed("zoom_out") && cameraLevel > 0)
		{
			cameraLevel -= 1;
			newZoom = cameraLevels[cameraLevel];
			//newZoom.X = (this.Zoom.X - 0.01f);
			//newZoom.Y = (this.Zoom.Y - 0.01f);
		}
	
		this.Position = newPosition;
		this.Zoom = newZoom;
	}
}
