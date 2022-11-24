using Godot;

public class Paddle : KinematicBody2D
{
    [Export]
    protected float _speed = 200f;

    private Vector2 _startingPosition;

    public override void _Ready()
    {
        _startingPosition = GlobalPosition;
        GetNode("/root/EventBus").Connect("PuckSpawned", this, nameof(OnPuckSpawned));
    }

    private void OnPuckSpawned(Puck _)
    {
        GlobalPosition = new Vector2(_startingPosition.x, GlobalPosition.y);
    }
}
