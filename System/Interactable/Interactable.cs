using Godot;
using System;
using System.Buffers;

public partial class Interactable : Area2D
{
	public Action Interact { get; set; }

	public override void _Ready()
	{
		this.AreaEntered += HandleAreaEntered;
		this.AreaExited += HandleAreaExited;
	}

	private void HandleAreaExited(Area2D area)
	{
		InteractionManager.Instance.UnregisterArea(this);
	}


	private void HandleAreaEntered(Area2D area)
	{
		InteractionManager.Instance.RegisterArea(this);
	}
}
