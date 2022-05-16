using Godot;
using System;

public class Scoreboard : Label
{
	private uint _playerScore = 0;
	private uint _npcScore = 0;

	public override void _Ready()
	{
		GetNode("/root/EventBus").Connect("AwardPoints", this, "OnPointsAwarded");
	}

	private void OnPointsAwarded(string whom, uint points)
	{
		switch(whom)
		{
			case "NPC":
				_npcScore += points;
				break;
			case "Player":
				_playerScore += points;
				break;
		}

		Text = $"{_npcScore} NPC    |    Player {_playerScore}";
	}
}
