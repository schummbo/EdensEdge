using EdensEdge.Extensions;
using Godot;

namespace EdensEdge.Scripts;

public partial class PlayerCharacterMove : CharacterBody2D
{
    private bool isMoving;
    private Vector2 facingDirection;

    [Export]
    public float Speed { get; set; } = 150;

    [Export]
    public AnimationTree AnimationTree { get; set; }

    public override void _Ready()
    {
        this.CheckRequiredDependency(AnimationTree, this.Name);
    }

    private void ProcessAnimation()
    {
        this.AnimationTree.Set("parameters/conditions/isMoving", isMoving);
        this.AnimationTree.Set("parameters/conditions/isIdle", !isMoving);
        this.AnimationTree.Set("parameters/Idle/blend_position", facingDirection);
        this.AnimationTree.Set("parameters/Moving/blend_position", facingDirection);
    }

    public override void _Process(double delta)
    {
        ProcessAnimation();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");

        if (direction != Vector2.Zero)
        {
            facingDirection = direction;
        }

        isMoving = direction != Vector2.Zero;

        var velocity = direction * Speed;

        Velocity = velocity;

        MoveAndSlide();
    }
}
