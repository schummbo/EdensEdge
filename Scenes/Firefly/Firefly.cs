using Godot;
using System;

public partial class Firefly : PointLight2D
{
    private Vector2 _targetPosition;
    private Vector2 _startPosition;
    private double _moveTime;
    private double _currentTime;
    private Random _random = new Random();

    [Export]
    public float Speed = 100.0f;

    [Export]
    public float MinDistance = 50.0f;

    [Export]
    public float MaxDistance = 200.0f;

    private Timer randomTimer;

    public override void _Ready()
    {
        randomTimer = new Timer();

        AddChild(randomTimer);

        SetNewTarget();

        EventBus.Instance.OnTimePhaseTick += HandleTimePhaseTick;
    }

    private void HandleTimePhaseTick(TimePhase phase)
    {
        if (phase == TimePhase.Dawn)
        {
            var time = _random.NextInt64(1, 10);
            randomTimer.WaitTime = time;
            randomTimer.Timeout += TurnOff;
            randomTimer.Start();
        }

        if (phase == TimePhase.Night)
        {
            var time = _random.NextInt64(1, 10);
            randomTimer.WaitTime = time;
            randomTimer.Timeout += TurnOn;
            randomTimer.Start();
        }
    }

    private void TurnOff()
    {
        this.Enabled = false;
    }

    private void TurnOn()
    {
        this.Enabled = true;
    }

    public override void _Process(double delta)
    {
        _currentTime += delta;

        if (_currentTime > _moveTime)
        {
            SetNewTarget();
        }
        else
        {
            double t = _currentTime / _moveTime;
            t = t * t * (3f - 2f * t); // Smoothstep interpolation
            GlobalPosition = _startPosition.Lerp(_targetPosition, (float)t);
        }
    }

    private void SetNewTarget()
    {
        _startPosition = GlobalPosition;
        _targetPosition = GetRandomPosition();
        _moveTime = (float)_random.NextDouble() * 2 + 1; // Move time between 1 and 3 seconds
        _currentTime = 0;
    }

    private Vector2 GetRandomPosition()
    {
        float angle = (float)_random.NextDouble() * Mathf.Pi * 2;
        float distance = (float)_random.NextDouble() * (MaxDistance - MinDistance) + MinDistance;
        return _startPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
    }
}
