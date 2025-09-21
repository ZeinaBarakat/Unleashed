using Godot;
using System;

public partial class MainMenu : Node
{
    [Export]
    Button startB;

    [Export]
    PackedScene gameScene;

    public override void _Ready()
    {
        startB.Pressed += OnStartButtonPressed;
    }
    private void OnStartButtonPressed()
    {
        GetTree().ChangeSceneToPacked(gameScene);
    }
}
