using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	Vector2 newPosition;
	Vector2 newZoom;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		newPosition = this.Position;
		newZoom = this.Zoom;
		
		if(Input.IsActionPressed("move_right") && this.Position.X < 4096 - 400 / this.Zoom.X)
			newPosition.X = (this.Position.X + 16);
		if(Input.IsActionPressed("move_left") && this.Position.X > 400 / this.Zoom.X)
			newPosition.X = (this.Position.X - 16);
		if(Input.IsActionPressed("move_down") && this.Position.Y < 4096 - 200 / this.Zoom.Y)
			newPosition.Y = (this.Position.Y + 16);
		if(Input.IsActionPressed("move_up") && this.Position.Y > 200 / this.Zoom.Y)
			newPosition.Y = (this.Position.Y - 16);
			
		if(Input.IsActionPressed("zoom_in") && this.Zoom.X <= 3.99)
		{
			newZoom.X = (this.Zoom.X + 0.01f);
			newZoom.Y = (this.Zoom.Y + 0.01f);
		}
		if(Input.IsActionPressed("zoom_out") && this.Zoom.X >= 0.51)
		{
			newZoom.X = (this.Zoom.X - 0.01f);
			newZoom.Y = (this.Zoom.Y - 0.01f);
		}
	
		this.Position = newPosition;
		this.Zoom = newZoom;
	}
}
