using Godot;

public partial class RotationPlatform : Node2D
{
    [Export]
    private AnimationPlayer animPlay;
    [Export]
    private Area2D area2D;
    [Export]
    private Timer timer;
    [Export]
    private AudioStreamPlayer2D dieSound;

    private bool timerRunning = false;

    public override void _Ready()
    {
        area2D.BodyEntered += OnBodyEntered;

        timer.Timeout += OpenDoor;

        animPlay.AnimationFinished += AnimDone;
    }


    private void OnBodyEntered(Node body)
    {
        if (timerRunning)
            return;
        timer.Start();
        timerRunning = true;
    }

    private void OpenDoor()
    {
        animPlay.Play("Open");
        dieSound.Play();
    }

    private void AnimDone(StringName animName)
    {
        timerRunning = false;
        animPlay.Play("Idle");
    }
}
