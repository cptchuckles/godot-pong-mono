using Godot;
using Dict = Godot.Collections.Dictionary;

public class Puck : KinematicBody2D
{
	[Export]
	private float _speed = 200f;

	[Export]
	private float _acceleration = 1.02f;

	private uint _bonus = 10;

	public uint Points { get; private set; } = 10;
	private Label _pointsLabel;

	public override void _Ready()
	{
		Rotate((float)(GD.Randi() % 8) * Mathf.Pi/4 + Mathf.Pi/8 + (float)GD.RandRange(-0.15f, 0.15f));

		SetAsToplevel(true);
		GlobalPosition = GetParent<Node2D>().GlobalPosition;

		GetNode<Node2D>("PointsPosition").SetAsToplevel(true);
		_pointsLabel = GetNode<Label>("PointsPosition/PointsLabel");

		GetNode("VisibilityNotifier2D").Connect("screen_exited", this, nameof(OnExitScreen));

		GetNode("/root/EventBus").EmitSignal("PuckSpawned", this);
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
				Points += _bonus;
				_bonus += 10;
			}
			else
			{
				Points += 1;
			}

			_pointsLabel.Text = $"+{Points}";
		}
	}

	private void OnExitScreen()
	{
		if (IsQueuedForDeletion()) return;
		GetTree().Connect("physics_frame", this, nameof(CheckGoalAndDie), flags: (uint)ConnectFlags.Oneshot);
	}

	private void CheckGoalAndDie()
	{
		Dict result = GetWorld2d().DirectSpaceState.IntersectRay(
				from: GlobalPosition,
				to: new Vector2(GetViewport().Size.x/2, GlobalPosition.y),
				collideWithBodies: false,
				collideWithAreas: true
				);

		if (result.Contains("collider") && result["collider"] is Goal goal)
		{
			goal.Score(this);
		}
		else
		{
			GD.Print($"{GetPath()}: No goal found! Dying.");
		}

		QueueFree();
	}
}
