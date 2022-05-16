using Godot;
using System;

public class PositionResetter : Node
{
	private Vector2 _startingPosition;

	public override void _Ready()
	{
		_startingPosition = ((Node2D)Owner).GlobalPosition;
		GetNode("/root/EventBus").Connect("PuckSpawned", this, "OnPuckSpawned");
	}

	private void OnPuckSpawned(Puck puck)
	{
		((Node2D)Owner).GlobalPosition = new Vector2(_startingPosition.x, ((Node2D)Owner).GlobalPosition.y);
	}
}
