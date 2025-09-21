using Godot;
using System;
using System.IO;

public partial class DawgEnemy : CharacterBody2D
{
    [Export]
    AnimatedSprite2D animatedSprite;
    [Export]
    Path2D path;
    [Export]
    Node2D player;
    const float MIN_DIST = 200.0f;
    const float SPEED = 400.0f;

private float currentOffset = 0f;

public override void _Ready()
{
    animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2Ddawg");
    var curve = path.Curve;
    currentOffset = curve.GetClosestOffset(GlobalPosition);
    GlobalPosition = curve.SampleBaked(currentOffset, true);
}

public override void _Process(double delta)
{
    var curve = path.Curve;
    float playerOffset = curve.GetClosestOffset(player.GlobalPosition);
    float dist = (player.GlobalPosition - GlobalPosition).Length();

    if (dist < MIN_DIST)
        return;

    // Move towards player along the path
    bool right = playerOffset > currentOffset;
    float direction = right ? 1f : -1f;
    float moveAmount = SPEED * (float)delta;

    // Calculate new offset
    float nextOffset = currentOffset + direction * moveAmount;

    // Clamp or wrap offset if needed (optional)
    nextOffset = Mathf.Clamp(nextOffset, 0, curve.GetBakedLength());

    // Move enemy
    GlobalPosition = curve.SampleBaked(nextOffset, true);
    currentOffset = nextOffset;

    animatedSprite.FlipH = !right;
    animatedSprite.Play("run");
}
}
