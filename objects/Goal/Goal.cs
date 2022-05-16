using Godot;
using System;

public class Goal : Area2D
{
	[Export(PropertyHint.Enum, "Player,NPC")]
	private string _whose;
	public string Whose => _whose;


	public override void _Ready()
	{
		Connect("body_entered", this, "OnBodyEntered");
	}


	private void OnBodyEntered(Node body)
	{
		if (body is Puck puck)
		{
			puck.QueueFree();
			EventBus bus = GetNode<EventBus>("/root/EventBus");
			bus.EmitSignal("GoalMade", this);
			bus.EmitSignal("AwardPoints", Whose, puck.Points);
		}
	}
}
