using Godot;
using System;

public class MainGame : Node2D
{
	[Export]
	private PackedScene _puckScene;

	private Node2D _puckSpawnPoint;


	public override void _Ready()
	{
		GD.Randomize();

		_puckSpawnPoint = GetNode<Node2D>("PuckSpawnPoint");

		if (! IsInstanceValid(_puckSpawnPoint))
		{
			throw new ApplicationException($"{GetPath()}: No PuckSpawnPoint:Node2D child");
		}

		if (_puckScene == null)
		{
			throw new ApplicationException($"{GetPath()}: No puck scene selected");
		}

		var bus = GetNode<EventBus>("/root/EventBus");
		bus.Connect("GoalMade", this, "OnGoalMade");
		bus.Connect("AwardPoints", this, "OnPointsAwarded");

		SpawnPuck();
	}


	private void SpawnPuck()
	{
		if (GetTree().GetNodesInGroup("Puck").Count > 0) return;

		_puckSpawnPoint.AddChild((Puck)_puckScene.Instance());
	}


	private void OnGoalMade(Goal goal)
	{
		GD.Print($"{goal.Whose} made a goal!!!! OH MY GODDDDDDDD");

		GetTree().CreateTimer(5).Connect("timeout", this, "SpawnPuck");
	}


	private void OnPointsAwarded(string whom, uint points)
	{
		GD.Print($"{whom} gained {points} points!");
	}
}
