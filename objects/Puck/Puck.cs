using Godot;
using System;

public class Puck : KinematicBody2D
{
	[Export]
	private float _speed = 200f;

	[Export]
	private float _acceleration = 1.02f;


	public override void _Ready()
	{
		GD.Randomize();
		Rotate((float)(GD.Randi() % 8) * Mathf.Pi/4 + Mathf.Pi/8);
		SetAsToplevel(true);
		GlobalPosition = (GetParent() as Node2D).GlobalPosition;
	}


	public override void _PhysicsProcess(float delta)
	{
		KinematicCollision2D collision = MoveAndCollide(GlobalTransform.y * _speed * delta);
		if (collision != null)
		{
			Rotate(GlobalTransform.y.AngleTo(GlobalTransform.y.Bounce(collision.Normal)));
			_speed *= _acceleration;
		}
	}
}
