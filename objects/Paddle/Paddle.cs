using Godot;
using System;

public class Paddle : KinematicBody2D
{
	[Export]
	protected float _speed = 200f;

	private Vector2 _startingPosition;

	public override void _Ready()
	{
		_startingPosition = GlobalPosition;
		GetNode("/root/EventBus").Connect("PuckSpawned", this, "OnPuckSpawned");
	}

	private void OnPuckSpawned(Puck puck)
	{
		GlobalPosition = new Vector2(_startingPosition.x, GlobalPosition.y);
	}
}
