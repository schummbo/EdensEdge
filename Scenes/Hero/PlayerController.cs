using System;
using Godot;

namespace EdensEdge.Scripts;

public partial class PlayerController : CharacterBody2D
{
    private bool isMoving;
    private Vector2 facingDirection;

    private AnimationTree animationTree;

    [Export]
    public float Speed { get; set; } = 150;

    public override void _Ready()
    {
        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
    }

    private void ProcessAnimation()
    {
        this.animationTree.Set("parameters/conditions/isMoving", isMoving);
        this.animationTree.Set("parameters/conditions/isIdle", !isMoving);
        this.animationTree.Set("parameters/Idle/blend_position", facingDirection);
        this.animationTree.Set("parameters/Moving/blend_position", facingDirection);
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

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("interact"))
        {
            var interacted = InteractionManager.Instance.Interact();
            GD.Print("Interacted: " + interacted);
        }

        if (inputEvent.IsActionPressed("useitem"))
        {
            bool interacted = FarmingManager.Instance.Interact();
            GD.Print("UseItem: " + interacted);
        }

        if (inputEvent.IsActionPressed("tick"))
        {
            FarmingManager.Instance.HandleTick();
        }
    }
}
