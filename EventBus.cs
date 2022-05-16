using Godot;
using System;

public class EventBus : Node
{
	[Signal]
	delegate void PuckSpawned(Puck puck);
	[Signal]
	delegate void GoalBreached(Goal goal);
}
