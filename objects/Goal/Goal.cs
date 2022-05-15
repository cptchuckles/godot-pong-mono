using Godot;
using System;

public class Goal : Area2D
{
	[Export(PropertyHint.Enum, "Player,NPC")]
	public string Whose;
}
