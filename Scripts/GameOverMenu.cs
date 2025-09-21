using Godot;
using System;

public partial class GameOverMenu : Node
{
    [Export]
    Button restartButton;

    public override void _Ready()
    {
        base._Ready();
        restartButton.Pressed += OnRestartButtonPressed;
    }

    private void OnRestartButtonPressed()
    {
        Level.instance.restartLast();
    }
}
