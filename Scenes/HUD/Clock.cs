using Godot;

public partial class Clock : Control
{
	private Label dayLabel;
	private Label timeLabel;
	private TextureRect arrow;

	public override void _Ready()
	{
		EventBus.Instance.OnTimeTick += HandleTimeTick;

		dayLabel = GetNode<Label>("DayContainer/DayLabel");
		timeLabel = GetNode<Label>("TimeContainer/TimeLabel");
		arrow = GetNode<TextureRect>("Arrow");
	}

	private void HandleTimeTick(int day, int hour, int minute, float rawTime)
	{
		dayLabel.Text = $"Day {day}";
		timeLabel.Text = $"{AmPmHour(hour)}:{minute:D2}";

		if (hour <= 12)
			arrow.RotationDegrees = ReMapRange(hour, 0, 12, -90, 90);
		else
			arrow.RotationDegrees = ReMapRange(hour, 13, 23, 90, -90);
	}

	private string AmPmHour(int hour)
	{
		if (hour == 0)
		{
			return "12";
		}

		if (hour > 12)
		{
			return (hour - 12).ToString();
		}

		return hour.ToString();
	}

	private float ReMapRange(float input, float minInput, float maxInput, float minOutput, float maxOutput)
	{
		return (float)(input - minInput) / (float)(maxInput - minInput) * (float)(maxOutput - minOutput) + minOutput;
	}
}
