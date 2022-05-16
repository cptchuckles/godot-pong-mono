using Godot;
using System;

public class Goal : Area2D
{
	[Export(PropertyHint.Enum, "Player,NPC")]
	public string Whose;


	public override void _Ready()
	{
		Connect("body_entered", this, "OnBodyEntered");
	}


	private void OnBodyEntered(Node body)
	{
		if (body is Puck puck)
		{
			puck.QueueFree();
			GetNode("/root/EventBus").EmitSignal("GoalBreached", this);
		}
	}
}
