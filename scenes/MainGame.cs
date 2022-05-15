using Godot;
using Array = Godot.Collections.Array;
using System;

public class MainGame : Node2D
{
	[Export]
	private PackedScene _puckScene;

	private Node2D _puckSpawnPoint;

	[Signal]
	delegate void PuckSpawned(Puck puck);


	public override void _Ready()
	{
		_puckSpawnPoint = GetNode<Node2D>("PuckSpawnPoint");

		if (! IsInstanceValid(_puckSpawnPoint))
		{
			throw new ApplicationException($"{GetPath()}: No PuckSpawnPoint:Node2D child");
		}

		if (_puckScene == null)
		{
			throw new ApplicationException($"{GetPath()}: No puck scene selected");
		}

		foreach (Goal goal in GetTree().GetNodesInGroup("Goals"))
		{
			goal.Connect("body_entered",
			             this,
			             "OnGoalBodyEntered",
			             new Array { goal.Whose });
		}

		SpawnPuck();
	}


	private void SpawnPuck()
	{
		if (GetTree().GetNodesInGroup("Puck").Count > 0) return;

		Puck puck = _puckScene.Instance() as Puck;
		_puckSpawnPoint.AddChild(puck);

		EmitSignal(nameof(PuckSpawned), puck);
	}


	private void OnGoalBodyEntered(Node body, string whose)
	{
		var puck = body as Puck;
		if (puck == null) return;

		GD.Print($"{whose}'s goal has been breached!!!! OH MY GODDDDDDDD");

		puck.QueueFree();

		GetTree().CreateTimer(5).Connect("timeout", this, "SpawnPuck");
	}
}
