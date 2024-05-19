using Godot;
using System;

public partial class Chest : Node2D
{
	private Interactable interactable;

	private bool isOpen = false;

	private AnimatedSprite2D animatedSprite2D;

	public override void _Ready()
	{
		interactable = GetNode<Interactable>("Interactable");
		interactable.Interact += HandleInteract;

		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	private void HandleInteract(ItemResource item)
	{
		if (!isOpen)
		{
			animatedSprite2D.Play();
			isOpen = true;
		}
		else
		{
			animatedSprite2D.PlayBackwards();
			isOpen = false;
		}
	}

}
