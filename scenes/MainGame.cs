using System;
using Godot;

public class MainGame : Node2D
{
    [Export]
    private readonly PackedScene _puckScene;

    private Node2D _puckSpawnPoint;

    public override void _Ready()
    {
        GD.Randomize();

        _puckSpawnPoint = GetNode<Node2D>("PuckSpawnPoint");

        if (!IsInstanceValid(_puckSpawnPoint))
        {
            throw new Exception($"{GetPath()}: No PuckSpawnPoint:Node2D child");
        }

        if (_puckScene is null)
        {
            throw new Exception($"{GetPath()}: No puck scene selected");
        }

        GetNode<EventBus>("/root/EventBus")
            .Connect("GoalMade", this, nameof(OnGoalMade));

        SpawnPuck();
    }

    private void SpawnPuck()
    {
        if (GetTree().GetNodesInGroup("Puck").Count > 0)
            return;

        _puckSpawnPoint.AddChild(_puckScene.Instance<Puck>());
    }

    private void OnGoalMade(Goal goal)
    {
        GetTree().CreateTimer(5).Connect("timeout", this, nameof(SpawnPuck));
    }
}
