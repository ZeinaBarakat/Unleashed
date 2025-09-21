using Godot;
using System;

public partial class Killzone : Area2D
{

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		Player player = (Player)body;
		player.DieAnim();

		Timer t = GetNode<Timer>("Timer");
		t.Timeout += OnTimerOver;
		t.Start();
	}

	private void OnTimerOver()
	{
		Level.instance.gameOver();
		
	}
}
