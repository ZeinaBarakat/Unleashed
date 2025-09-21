using Godot;
using System;
using System.Collections.Generic;

public partial class RopeSegment : RigidBody2D
{
    Rope rope;
    public Rope Rope => rope;
    public void init(Rope r)
    {
        rope = r;
    }
}