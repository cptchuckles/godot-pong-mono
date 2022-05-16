using Godot;
using Dict = Godot.Collections.Dictionary;
using System;

public class Puck : KinematicBody2D
{
	[Export]
	private float _speed = 200f;

	[Export]
	private float _acceleration = 1.02f;

	private uint _bonus = 10;

	private uint _points = 10;
	public uint Points => _points;
	private Label _pointsLabel;


	public override void _Ready()
	{
		Rotate((float)(GD.Randi() % 8) * Mathf.Pi/4 + Mathf.Pi/8);

		SetAsToplevel(true);
		GlobalPosition = ((Node2D)GetParent()).GlobalPosition;

		GetNode<Node2D>("PointsPosition").SetAsToplevel(true);
		_pointsLabel = GetNode<Label>("PointsPosition/PointsLabel");

		GetNode("VisibilityNotifier2D").Connect("screen_exited", this, "OnExitScreen");
	}


	public override void _PhysicsProcess(float delta)
	{
		KinematicCollision2D collision = MoveAndCollide(GlobalTransform.y * _speed * delta);
		if (collision != null)
		{
			Rotate(GlobalTransform.y.AngleTo(GlobalTransform.y.Bounce(collision.Normal)));

			if (collision.Collider is Paddle)
			{
				_speed *= _acceleration;
				_points += _bonus;
				_bonus += 10;
			}
			else
			{
				_points += 1;
			}

			_pointsLabel.Text = $"+{_points}";
		}
	}


	private void OnExitScreen()
	{
		if (IsQueuedForDeletion()) return;
		GetTree().Connect("physics_frame", this, "CheckGoalAndDie", flags: (uint)ConnectFlags.Oneshot);
	}


	private void CheckGoalAndDie()
	{
		Dict result = GetWorld2d().DirectSpaceState.IntersectRay(
				from: GlobalPosition,
				to: new Vector2(GetViewport().Size.x/2, GlobalPosition.y),
				collideWithBodies: false,
				collideWithAreas: true
				);

		if (result["collider"] is Goal goal)
		{
			EventBus bus = GetNode<EventBus>("/root/EventBus");
			bus.EmitSignal("GoalMade", goal);
			bus.EmitSignal("AwardPoints", goal.Whose, Points);
		}

		QueueFree();
	}
}
