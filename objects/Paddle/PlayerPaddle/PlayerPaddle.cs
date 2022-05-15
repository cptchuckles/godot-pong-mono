using Godot;
using System;

public class PlayerPaddle : KinematicBody2D
{
	[Export]
	private float _speed = 200f;

	public override void _PhysicsProcess(float delta)
	{
		Vector2 movement = new Vector2(0f, Input.GetAxis("ui_up", "ui_down"));
		MoveAndSlide(movement * _speed);
	}
}
