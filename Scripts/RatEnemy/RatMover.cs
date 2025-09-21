using Godot;

public partial class RatMover : Node2D
{
	[Export]
	private bool moveRight = true;
	[Export]
	private float walkSpeed = 100.0f;
	[Export]
	private float attackSpeed = 250.0f;
	[Export]
	private float lowerLimitX = -500.0f;
	[Export]
	private float upperLimitX = 500.0f;
	[Export]
	private float attackRange = 500.0f;

	private RayCast2D rightRay;
	private RayCast2D leftRay;
	private AnimatedSprite2D sprite;
	private Vector2 localStart;

	private CharacterBody2D player;

	private bool viewPlayer = false;


	public override void _Ready()
	{
		localStart = Position;

		sprite = GetNode<AnimatedSprite2D>("DawgSprite");
		rightRay = GetNode<RayCast2D>("Ray Right");
		leftRay = GetNode<RayCast2D>("Ray Left");
		player = GetParent().GetNode<CharacterBody2D>("player");;
	}

	public override void _Process(double delta)
	{
		Vector2 pos = Position;

		if (rightRay.IsColliding() || Position.X >= (localStart.X + upperLimitX))
		{
			moveRight = false;
		}
		else if (leftRay.IsColliding() || Position.X <= (localStart.X + lowerLimitX))
		{
			moveRight = true;
		}
		sprite.FlipH = !moveRight;

		bool near = Position.DistanceTo(player.Position) <= attackRange;
		if (near)
		{
			if (!viewPlayer)
			{
				viewPlayer = true;
				if (player.Position.X < Position.X)
					moveRight = false;
				else
					moveRight = true;
			}
			sprite.Play("attack");
		}
			
		else
		{
			viewPlayer = false;
			sprite.Play("walk");
		}

		pos.X += (near ? attackSpeed : walkSpeed) * (moveRight ? 1 : -1) * (float)delta;
		Position = pos;
	}
}
