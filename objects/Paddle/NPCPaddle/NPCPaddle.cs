using Godot;

public class NPCPaddle : Paddle
{
    private Puck _puck;

    public override void _Ready()
    {
        GetNode("/root/EventBus").Connect("PuckSpawned", this, nameof(OnPuckSpawned));
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!IsInstanceValid(_puck))
            return;

        Vector2 toMe = GlobalPosition - _puck.GlobalPosition;

        if (_puck.GlobalTransform.y.Dot((toMe with { y = 0f }).Normalized()) < 0f)
            return;

        float frameSpeed = _speed * delta;
        float movement = Mathf.Clamp(-toMe.y, -frameSpeed, frameSpeed);
        MoveAndCollide(new Vector2(0f, movement));
    }

    private void OnPuckSpawned(Puck puck)
    {
        _puck = puck;
    }
}
