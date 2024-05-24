using System;
using Godot;

public partial class DayNightCycle : CanvasModulate
{
	[Export] public GradientTexture1D DayNightGradient { get; set; }

	public override void _Ready()
	{
		EventBus.Instance.OnTimeTick += HandleTimeTick;
	}

	private void HandleTimeTick(int day, int hour, int minute, float rawTime)
	{
		float value = (float)((Math.Sin(rawTime - Math.PI / 2.0) + 1.0) / 2.0);
		var color = DayNightGradient.Gradient.Sample(value);

		this.Color = color;
	}
}