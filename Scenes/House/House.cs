using Godot;
using System;

public partial class House : StaticBody2D
{
	private Interactable interactable;
	private Light sconce;

	public override void _Ready()
	{
		interactable = this.GetNode<Interactable>("Interactable");
		interactable.Interact += HandleInteraction;

		sconce = GetNode<Light>("Light");

		EventBus.Instance.OnTimePhaseTick += HandlePhaseTick;
	}

	private void HandlePhaseTick(TimePhase phase)
	{
		if (phase == TimePhase.Dusk || phase == TimePhase.Night)
		{
			sconce.TurnOn();
		}

		if (phase == TimePhase.Dawn)
		{
			sconce.TurnOff();
		}
	}

	private void HandleInteraction(ItemResource item)
	{
		sconce.Toggle();
	}
}
