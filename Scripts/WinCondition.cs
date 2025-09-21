using Godot;
using System;

public partial class WinCondition : Area2D
{

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
        Level.instance.reachEnd();
	}
}
