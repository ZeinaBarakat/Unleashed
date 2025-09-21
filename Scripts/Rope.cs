using Godot;
using System;
using System.Collections.Generic;

public partial class Rope : Node2D
{
	List<Node2D> nodes;
	public List<Node2D> Nodes => nodes;

	public Rope(List<Node2D> jointNodes)
	{
		this.nodes = jointNodes;
	}

	public void AssignTexture(Texture2D texture, float width = 20.0f)
	{
		if (GetChildCount() > 0 && GetChild(0) is Line2D line)
		{
			line.Width = width;
			line.Texture = texture;
			line.TextureMode = Line2D.LineTextureMode.Tile;
			line.TextureRepeat = TextureRepeatEnum.Enabled;
		}
	}

	/// <summary>
	/// Returns the nearest node just to the bottom and the top of the player. The first is closest to the rope bottom end.
	/// </summary>
	/// <param name="position"></param>
	/// <returns></returns>
	public (Node2D, Node2D) GetNearestNodesSorted(Vector2 position)
	{
		if (nodes == null || nodes.Count == 0)
			return (null, null);

		float nearestDist = float.MaxValue;
		int nearestIndex = -1;

		for (int i = 0; i < nodes.Count; i++)
		{
			float dist = nodes[i].GlobalPosition.DistanceTo(position);
			if (dist < nearestDist)
			{
				nearestIndex = i;
				nearestDist = dist;
			}
		}

		if (nearestIndex == -1)
			return (null, null);
		try
		{
			return (nearestIndex < nodes.Count - 1 ? nodes[nearestIndex + 1] : nodes[nearestIndex], nearestIndex > 0 ? nodes[nearestIndex - 1] : nodes[nearestIndex]);
		}
		catch (Exception e)
		{
			GD.PrintErr(nodes.Count, " ", nearestIndex);
		 
			return (null, null);
		}
	}

	override public void _Ready()
	{
		base._Ready();
		Line2D line = new Line2D();
		line.JointMode = Line2D.LineJointMode.Bevel;
		AddChild(line);
	}

	override public void _Process(double delta)
	{
		base._Process(delta);
		// Update line points if needed
		if (GetChildCount() > 0 && GetChild(0) is Line2D line)
		{
			line.Points = nodes.ConvertAll(node => node.Position).ToArray();
		}
	}
}
