using Godot;
using System;

public partial class LaunchPad : Area2D
{
    [Export]
    private float launchVelocity = -1500;
    [Export]
    private AudioStreamPlayer2D dieSound;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        Player player = (Player)body;
        player.LaunchPlayer(launchVelocity);
        dieSound.Play();
	}
}
