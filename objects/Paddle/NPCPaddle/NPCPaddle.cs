using Godot;
using System;

public class NPCPaddle : KinematicBody2D
{
	[Export]
	private float _speed = 200f;

	private Puck _puck;


	public override void _Ready()
	{
		GetParent().Connect("PuckSpawned", this, "OnMainGamePuckSpawned");
	}


	public override void _PhysicsProcess(float delta)
	{
		if (! IsInstanceValid(_puck)) return;

		Vector2 toMe = GlobalPosition - _puck.GlobalPosition;

		if (_puck.GlobalTransform.y.Dot(new Vector2(toMe.x, 0f).Normalized()) < 0f) return;

		float frameSpeed = _speed * delta;
		float movement = Mathf.Clamp(-toMe.y, -frameSpeed, frameSpeed);
		MoveAndCollide(new Vector2(0f, movement));
	}


	private void OnMainGamePuckSpawned(Puck puck)
	{
		_puck = puck;
	}
}
