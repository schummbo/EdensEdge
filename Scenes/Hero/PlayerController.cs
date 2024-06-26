using System;
using Godot;

namespace EdensEdge.Scripts;

public partial class PlayerController : CharacterBody2D
{
    private static ItemResource hands = GD.Load<ItemResource>("res://Tools/Hands.tres");

    private ItemResource equippedItem;
    private bool isMoving;
    private Vector2 facingDirection;
    private AnimationTree animationTree;

    [Export] public float Speed { get; set; } = 150;

    [Export] public Inventory PlayerInventory { get; set; }

    public override void _Ready()
    {
        this.equippedItem = hands;
        this.animationTree = this.GetNode<AnimationTree>("AnimationTree");
        EventBus.Instance.OnInventoryItemSelected += HandleInventoryItemSelected;
    }

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("interact"))
        {
            var interacted = InteractionManager.Instance.Interact(equippedItem);
            GD.Print("Interacted: " + interacted);
        }

        if (inputEvent.IsActionPressed("useitem"))
        {
            bool interacted = FarmingManager.Instance.Interact(equippedItem);

            if (interacted)
                if (equippedItem.DestroyedOnUse)
                {
                    EventBus.Instance.InventoryItemUsed(equippedItem);
                }
        }
    }

    private void HandleInventoryItemSelected(ItemResource resource)
    {
        this.equippedItem = resource ?? hands;
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
}
