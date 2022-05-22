using Godot;
using System;

public class EventBus : Node
{
	[Signal]
	delegate void PuckSpawned(Puck puck);
	[Signal]
	delegate void GoalMade(Goal goal);
	[Signal]
	delegate void AwardPoints(string whom, uint points);
}
