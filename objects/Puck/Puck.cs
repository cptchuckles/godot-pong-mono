using Godot;
using Dict = Godot.Collections.Dictionary;
using System;

public class Puck : KinematicBody2D
{
	[Export]
	private float _speed = 200f;

	[Export]
	private float _acceleration = 1.02f;


	public override void _Ready()
	{
		GD.Randomize();
		Rotate((float)(GD.Randi() % 8) * Mathf.Pi/4 + Mathf.Pi/8);

		SetAsToplevel(true);
		GlobalPosition = (GetParent() as Node2D).GlobalPosition;

		GetNode("VisibilityNotifier2D").Connect("screen_exited", this, "OnExitScreen");
	}


	public override void _PhysicsProcess(float delta)
	{
		KinematicCollision2D collision = MoveAndCollide(GlobalTransform.y * _speed * delta);
		if (collision != null)
		{
			Rotate(GlobalTransform.y.AngleTo(GlobalTransform.y.Bounce(collision.Normal)));
			_speed *= _acceleration;
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
			GetNode("/root/EventBus").EmitSignal("GoalBreached", goal);
		}

		QueueFree();
	}
}
