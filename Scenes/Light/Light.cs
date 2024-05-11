using Godot;

public partial class Light : Node2D
{
	private PointLight2D pointLight2D;

	public override void _Ready()
	{
		pointLight2D = this.GetNode<PointLight2D>("PointLight2D");
	}

	public void Toggle()
	{
		pointLight2D.Enabled = !pointLight2D.Enabled;
	}
}
