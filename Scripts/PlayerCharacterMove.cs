using System.Collections.Generic;
using System.Linq;
using Godot;

namespace EdensEdge.Scripts;

[Tool]
public partial class PlayerCharacterMove : CharacterBody2D
{
    private bool isMoving;
    private Vector2 facingDirection;

    [Export]
    public float Speed { get; set; } = 150;

    [Export]
    public AnimationTree AnimationTree { get; set; }

    [Export]
    public InteractableManager InteractableManager { get; set; }

    public override string[] _GetConfigurationWarnings()
    {
        var warnings = base._GetConfigurationWarnings()?.ToList() ?? new List<string>();

        if (AnimationTree == null)
        {
            warnings.Add("An AnimationTree is required for this scene to work properly.");
        }

        if (Speed == 0)
        {
            warnings.Add("A speed of 0 will cause your player to not be able to move.");
        }

        if (InteractableManager == null)
        {
            warnings.Add("Interactable manager must be set to rotate the shapecast correctly.");
        }

        return warnings.ToArray();
    }

    public override void _Ready()
    {
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
        if (Engine.IsEditorHint()) return;

        ProcessAnimation();
        UpdateInteractableManager();
    }

    private void UpdateInteractableManager()
    {
        InteractableManager.Rotation = facingDirection.AngleTo(Vector2.Down) * -1;
    }


    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint()) return;

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
