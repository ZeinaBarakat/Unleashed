using Godot;
using System;
using System.Collections.Generic;

public partial class RopeGenerator : Node2D
{
	private Sprite2D dummy_sprite;
	private Texture2D rope_texture;

	private PackedScene rope_segment_scene;

	/// <summary>
	/// generates a rope and returns the joint nodes for line generation.
	/// </summary>
	/// <param name="startPosition"></param>
	/// <param name="endPosition"></param>
	/// <param name="segmentLength"></param>
	/// <param name="ropeSegmentScene"></param>
	/// <returns></returns>
	List<Node2D> GenerateRope(Vector2 startPosition, Vector2 endPosition, float segmentLength, PackedScene ropeSegmentScene)
	{
		List<Node2D> nodes = new List<Node2D>();

		var root_static_body = new StaticBody2D();
		root_static_body.Position = startPosition;
		AddChild(root_static_body);


		Vector2 direction = (endPosition - startPosition).Normalized();
		float distance = startPosition.DistanceTo(endPosition);
		int segmentCount = Mathf.CeilToInt(distance / segmentLength);

		Node2D previousSegment = root_static_body;
		nodes.Add(root_static_body);

		for (int i = 0; i <= segmentCount; i++)
		{
			Vector2 segmentPosition = startPosition + direction * segmentLength * i;
			Node2D segment = ropeSegmentScene.Instantiate<Node2D>();
			
			(((segment.GetChild(0) as CollisionShape2D).Shape) as CapsuleShape2D).Height = segmentLength;
			segment.Position = segmentPosition;
			AddChild(segment);

			nodes.Add(segment);
			(segment as RigidBody2D).Mass = 10.0f/segmentCount;
			(segment as RigidBody2D).AngularDamp = 0.54f;
			(segment as RigidBody2D).LinearDamp = 0.94f;

			if (previousSegment != null)
			{
				var joint = new PinJoint2D();
				joint.NodeA = previousSegment.GetPath();
				joint.NodeB = segment.GetPath();
				joint.Position = previousSegment.Position + (segment.Position - previousSegment.Position) / 2.0f;
				AddChild(joint);
			}

			previousSegment = segment;
		}
		return nodes;
	}

	public override void _Ready()
	{
		base._Ready();
		rope_segment_scene = GD.Load<PackedScene>("res://Scenes/rope_segment.tscn");
		rope_texture = GD.Load<Texture2D>("res://Textures/rope.png");
		dummy_sprite = GetChild<Sprite2D>(0);
		dummy_sprite.Hide();

		var nodes = GenerateRope(-new Vector2(10, this.Scale.Y * 50.0f), Vector2.Zero, 80/this.Scale.Y, rope_segment_scene);
		this.Scale = new Vector2(1, 1);
		var rope = new Rope(nodes);

		foreach(var n in nodes)
		{
			if (n is RopeSegment rs)
				rs.init(rope);
		}
		AddChild(rope);
		rope.AssignTexture(rope_texture);
	}
}
