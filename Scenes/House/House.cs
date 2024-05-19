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
	}


	private void HandleInteraction(ItemResource item)
	{
		sconce.Toggle();
	}
}
