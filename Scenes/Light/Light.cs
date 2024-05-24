using Godot;
using System;

public partial class Light : Node2D
{
	private PointLight2D pointLight2D;

	private Timer timer;

	private Random random;

	public override void _Ready()
	{
		pointLight2D = this.GetNode<PointLight2D>("PointLight2D");
		timer = new Timer();
		random = new Random();

		AddChild(timer);
		timer.Timeout += OnTimerTimeout;

		if (pointLight2D.Enabled)
			timer.Start(0.1f);
	}

	public void TurnOn()
	{
		timer.Start();
		pointLight2D.Enabled = true;
	}

	public void TurnOff()
	{
		timer.Stop();
		pointLight2D.Enabled = false;
	}

	public void Toggle()
	{
		if (pointLight2D.Enabled)
		{
			TurnOff();
		}
		else
		{
			TurnOn();
		}
	}

	private void OnTimerTimeout()
	{
		// Change the energy property randomly to create a flickering effect
		pointLight2D.Energy = (float)(random.NextDouble() * 0.2 + 0.8);
	}
}
