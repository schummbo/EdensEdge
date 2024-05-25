using Godot;
using System;

public partial class Light : Node2D
{
	private PointLight2D pointLight2D;

	private Timer flickerTimer;

	private Timer randomOffTimer;

	private Random random;

	public override void _Ready()
	{
		pointLight2D = this.GetNode<PointLight2D>("PointLight2D");
		flickerTimer = new Timer();
		randomOffTimer = new Timer();
		random = new Random();

		AddChild(randomOffTimer);
		AddChild(flickerTimer);
		flickerTimer.Timeout += OnTimerTimeout;

		if (pointLight2D.Enabled)
			flickerTimer.Start(0.1f);
	}

	public void TurnOn()
	{
		flickerTimer.Start();
		pointLight2D.Enabled = true;
	}

	public void TurnOnRandom()
	{
		// generate random timer time
		var timeSeconds = (int)this.random.NextInt64(1, 10);
		// on tick, turn on

		randomOffTimer.WaitTime = timeSeconds;
		randomOffTimer.Timeout += TurnOn;
		randomOffTimer.Start();
	}

	public void TurnOff()
	{
		flickerTimer.Stop();
		pointLight2D.Enabled = false;
	}

	public void TurnOffRandom()
	{
		// generate random timer time
		var timeSeconds = (int)this.random.NextInt64(1, 10);
		// on tick, turn on

		randomOffTimer.WaitTime = timeSeconds;
		randomOffTimer.Timeout += TurnOff;
		randomOffTimer.Start();
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
