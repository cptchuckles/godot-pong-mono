using Godot;
using System;

public class Goal : Area2D
{
	[Export(PropertyHint.Enum, "Player,NPC")]
	public string Whose { get; private set; } = "Player";


	public override void _Ready()
	{
		Connect("body_entered", this, "OnBodyEntered");
	}


	private void OnBodyEntered(Node body)
	{
		if (body is Puck puck)
		{
			GD.Print($"{Whose} made a goal!!!! OH MY GODDDDDDDD");

			puck.QueueFree();

			var bus = GetNode<EventBus>("/root/EventBus");
			bus.EmitSignal("GoalMade", this);
			bus.EmitSignal("AwardPoints", Whose, puck.Points);
		}
	}
}
