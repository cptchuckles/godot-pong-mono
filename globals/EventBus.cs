using Godot;

public class EventBus : Node
{
    [Signal]
    public delegate void PuckSpawned(Puck puck);
    [Signal]
    public delegate void GoalMade(Goal goal);
    [Signal]
    public delegate void AwardPoints(string whom, uint points);
}
