using Godot;
using System;

public class Goal : Area2D
{
	[Export(PropertyHint.Enum, "Player,NPC")]
	public string Whose { get; private set; } = "Player";

	private bool _active = true;


	public override void _Ready()
	{
		Connect("body_entered", this, nameof(OnBodyEntered));
		GetNode("/root/EventBus").Connect("PuckSpawned", this, nameof(OnPuckSpawned));
	}


	private void OnBodyEntered(Node body)
	{
		if (body is Puck puck)
		{
			Score(puck);
		}
	}


	public void Score(Puck puck)
	{
		puck.QueueFree();

		if (! _active) return;
		_active = false;

		GD.Print($"{Whose} made a goal!!!! OH MY GODDDDDDDD");

		var bus = GetNode<EventBus>("/root/EventBus");
		bus.EmitSignal("GoalMade", this);
		bus.EmitSignal("AwardPoints", Whose, puck.Points);
	}


	private void OnPuckSpawned(Puck puck)
	{
		_active = true;
	}
}
