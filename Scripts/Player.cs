using Godot;

public partial class Player : CharacterBody2D
{
    // Constants
    [Export]
    private float Speed = 800.0f;
    [Export]
    private float smoothSpeed = 2500.0f;
    [Export]
    private float JumpVelocity = -900.0f;
    [Export]
    private float NormalLaunchPadSpeed = -2000.0f;
    [Export]
    private AudioStreamPlayer2D jumpSound;
    [Export]
    private AudioStreamPlayer2D dieSound;

    private bool flip = false;
    [Export]
    public bool Flip
    {
        get => flip;
        set
        {
            flip = value;
            if (animatedSprite != null)
                animatedSprite.FlipH = value;
        }
    }

    // References
    private AnimatedSprite2D animatedSprite;
    [Export]
    public Area2D area2D;

    private bool die = false;
    private bool launch = false;
    private float launchSpeed;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    /// <summary>
    /// returns the unit direction to move (up, down), if the player collides with a rope, null otherwise. Last is position of nearest node.
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    public (Vector2?, Vector2?, Vector2?, RopeSegment ropeSegment1) GetRopeMoveDir(Node2D col, CharacterBody2D player)
    {
        if (col == null)
            return (null, null, null, null);
        if (col is RopeSegment ropeSegment)
        {
            Rope rope = ropeSegment.Rope;
            var (bottom, top) = rope.GetNearestNodesSorted(player.GlobalPosition);

            if (bottom == null || top == null)
                return (null, null, null, null);

            return ((bottom.GlobalPosition - player.GlobalPosition).Normalized(), (top.GlobalPosition - player.GlobalPosition).Normalized(), 0.5f * (bottom.GlobalPosition + top.GlobalPosition), ropeSegment);
        }
        return (null, null, null, null);
    }

    bool attached = false;
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Get the input direction: -1, 0, 1
        float direction = Input.GetAxis("move_left", "move_right");
        if (die)
            direction = 0;

        area2D.GetOverlappingBodies();
        Vector2? up = null;
        Vector2? down = null;
        Vector2? towards = null;
        Rope rope = null;
        foreach (var body in area2D.GetOverlappingBodies())
        {
            (up, down, towards, RopeSegment ropeSegment) = GetRopeMoveDir(body as Node2D, this);
            if (up != null && down != null)
            {
                rope = ropeSegment.Rope;
                break;
            }
        }


        // a && j -> detach
        // !a && j && r-> attach

        if (!die && Input.IsActionJustPressed("detach"))
        {
            if (attached)
            {
                attached = false;
            }
            else if (up != null && down != null)
            {
                attached = true;
                // transfer momentum
                foreach (var n in rope.Nodes)
                {
                    if (n is RopeSegment rs)
                    {
                        rs.ApplyImpulse(velocity, velocity * rs.Mass);
                    }
                }
            }
        }

        if (up != null && down != null && attached)
        {
            if (!die && Input.IsActionPressed("up"))
            {
                velocity = down.Value * Speed * .5f;
                if (!die)
                    animatedSprite.Play("climb");
            }
            else if (!die && Input.IsActionPressed("down"))
            {
                velocity = up.Value * Speed * .5f;
                if (!die)
                    animatedSprite.Play("climb");
            }
            else
            {
                this.GlobalPosition = towards.Value;
                if (!die)
                    animatedSprite.Play("climbIdle");
            }
        }
        else
        {
            // Add gravity
            if (!IsOnFloor())
            {
                velocity += 2.0f * GetGravity() * (float)delta;
            }

            // Handle jump
            if (!die && Input.IsActionJustPressed("jump") && IsOnFloor())
            {
                velocity.Y = JumpVelocity;
                jumpSound.Play();
            }
            if (launch)
            {
                launch = false;
                velocity.Y = launchSpeed;
            }

            // Apply movement
            if (direction != 0)
            {
                velocity.X = direction * Speed;
            }
            else if (IsOnFloor())
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            }


            // Flip the sprite
            if (direction > 0)
                animatedSprite.FlipH = false;
            else if (direction < 0)
                animatedSprite.FlipH = true;

            // Play animations
            if (die)
                animatedSprite.Play("die");
            else if (IsOnFloor())
            {
                if (direction == 0)
                    animatedSprite.Play("idle");
                else
                    animatedSprite.Play("run");
            }
            else
            {
                animatedSprite.Play("jump");
            }
        }



        //Velocity = velocity;
        Velocity = new Vector2(Mathf.MoveToward(Velocity.X, velocity.X, smoothSpeed * (float)delta), velocity.Y);
        MoveAndSlide();
    }

    public void LaunchPlayer(float speed = 0f)
    {
        if (speed == 0f)
            launchSpeed = NormalLaunchPadSpeed;
        else
            launchSpeed = speed;
        launch = true;
    }
    public void DieAnim()
    {
        dieSound.Play();
        die = true;
        animatedSprite.Play("die");
    }
}
