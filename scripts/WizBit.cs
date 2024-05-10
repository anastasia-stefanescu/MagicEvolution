using Godot;
using System;

public class WizBit : Godot.CharacterBody2D {
    double rotation_dir = 0;
    double rotation_speed = 2;
    const double speed = 300;
    private AnimatedSprite2D animated_sprite_2d;
    double ai_x = 0;
    double ai_y = 0;
    double ai_rotation = 0;
    double frame_cnt = 0;
    VisionData vision;
//   var test1 = load("res://test.cs")
//  var test2 = load("res://test_2.cs")

    public override void _Ready()
    {
        // var test1_a = new Test1(); 
        var test2_a = new Test2();
        // Console.WriteLine(test1_a.Whatever()); 
        Console.WriteLine(test2_a.Whenever());
    }

    private void GetAIInput()
    {
        Vector2 velocity = Vector2.Zero;
        rotation_dir += ai_rotation;
        velocity.x += ai_x;
        velocity.y += ai_y;
        if (frame_cnt % 20 == 0)
        {
            ai_rotation = (float)GD.RandRange(-1, 1);
            GD.Print("R: " + ai_rotation);
        }
        if (frame_cnt % 60 == 0)
        {
            ai_x = (float)GD.RandRange(-1, 1);
            GD.Print("X: " + ai_x);
        }

        if (frame_cnt % 120 == 0)
        {
            ai_y = (float)GD.RandRange(-1, 1);
            GD.Print("Y: " + ai_y);
        }
        velocity *= speed;
    }

    public override void _PhysicsProcess(float delta)
    {
        GetAIInput();
        frame_cnt = (frame_cnt + 1) % 120;
        // if (Input.IsActionPressed("ui_right") || Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_down") || Input.IsActionPressed("ui_up"))
        if (Math.Abs(velocity.x) > 0 || Math.Abs(velocity.y) > 0)
            animated_sprite_2d.Play("running");
        else
            animated_sprite_2d.Play("idle");
        rotation += rotation_dir * rotation_speed * delta;
        velocity = velocity.Rotated(rotation);
        MoveAndSlide();
    }
}