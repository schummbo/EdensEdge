using Godot;
using System;

public partial class TimeKeeper : Node
{
	private const float MINUTES_PER_DAY = 1440;
	private const float MINUTES_PER_HOUR = 60;
	private const float INGAME_TO_REAL_MINUTE_DURATION = (2 * (float)Math.PI) / MINUTES_PER_DAY;

	[Export] public float INGAME_SPEED { get; set; } = 60;
	[Export] public int InitialHour { get; set; } = 1;

	private float time = 0.0f;
	private int pastMinute = -1;
	private int pastDay = -1;

	private TimePhase pastPhase;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		time = INGAME_TO_REAL_MINUTE_DURATION * MINUTES_PER_HOUR * InitialHour;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		time += (float)delta * INGAME_TO_REAL_MINUTE_DURATION * INGAME_SPEED;

		RecalculateTime();
	}

	private void RecalculateTime()
	{
		var total_minutes = (int)(time / INGAME_TO_REAL_MINUTE_DURATION);

		var day = (int)(total_minutes / MINUTES_PER_DAY);

		var current_day_minutes = total_minutes % MINUTES_PER_DAY;

		var hour = (int)(current_day_minutes / MINUTES_PER_HOUR);
		var minute = (int)(current_day_minutes % MINUTES_PER_HOUR);

		if (pastMinute != minute)
		{
			pastMinute = minute;
			EventBus.Instance.TimeTick(day + 1, hour, minute, time);

			TimePhase currentPhase = GetPhase(hour);

			if (pastPhase != currentPhase)
			{
				pastPhase = currentPhase;
				EventBus.Instance.TimePhaseTick(currentPhase);
			}

			if (pastDay != day)
			{
				pastDay = day;
				EventBus.Instance.DayTick(day);
			}
		}
	}

	private TimePhase GetPhase(int hour)
	{
		TimePhase phase = TimePhase.Night;

		if (hour >= 19 && hour < 5)
		{
			phase = TimePhase.Night;
		}

		if (hour >= 5 && hour < 12)
		{
			phase = TimePhase.Dawn;
		}

		if (hour >= 12 && hour < 17)
		{
			phase = TimePhase.Noon;
		}

		if (hour >= 17 && hour < 19)
		{
			phase = TimePhase.Dusk;
		}

		return phase;
	}

}
