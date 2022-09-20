using Godot;

public class PlayerPaddle : Paddle
{
    public override void _PhysicsProcess(float delta)
    {
        var movement = new Vector2(0f, Input.GetAxis("ui_up", "ui_down"));
        MoveAndCollide(movement * _speed * delta);
    }
}
