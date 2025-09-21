using Godot;
using System;

public partial class SimpleMovement : CharacterBody2D
{
	private float moveSpeed = 150.0f;
	private float jumpVelocity = 400.0f;

	private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}
		else if (Input.IsKeyPressed(Key.Space))
			velocity.Y = -jumpVelocity;

		velocity.X = 0;
		if (Input.IsKeyPressed(Key.Left))
			velocity.X = -moveSpeed;
		else if (Input.IsKeyPressed(Key.Right))
			velocity.X = moveSpeed;

		Velocity = velocity;
		MoveAndSlide();
	}

}
