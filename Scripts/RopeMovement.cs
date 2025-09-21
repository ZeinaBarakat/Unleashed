using Godot;
using System;
using System.Collections.Generic;

public partial class RopeMovement : Node2D
{
    /// <summary>
    /// returns the unit direction to move (up, down), if the player collides with a rope, null otherwise.
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    public (Vector2?, Vector2?) GetRopeMoveDir(KinematicCollision2D collision,  CharacterBody2D player)
    {
        var par = ((Node2D)collision.GetCollider()).GetParent();
        if (par == null)
            return (null, null);
        if (par.GetParent() is Rope rope)
        {
            var (nearest, secondNearest) = rope.GetNearestNodesSorted(player.Position);

            if (nearest == null || secondNearest == null)
                return (null, null);
            
            return ((nearest.Position - player.Position).Normalized(), (secondNearest.Position - player.Position).Normalized());
        }
        return (null, null);
    }
}