using Godot;

public partial class PlayerCharacterMove : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 150;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        velocity = direction * Speed;

        Velocity = velocity;

        MoveAndSlide();
    }
}
