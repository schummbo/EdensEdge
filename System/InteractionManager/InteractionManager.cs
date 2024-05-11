using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class InteractionManager : Node2D
{
	public static InteractionManager Instance;

	private Node player;

	private List<Interactable> interactableAreas = new List<Interactable>();

	private bool canInteract = true;

	public override void _Ready()
	{
		Instance = this;
		player = this.GetTree().GetFirstNodeInGroup("Player");
	}

	public void RegisterArea(Interactable interactable)
	{
		interactableAreas.Add(interactable);
	}

	public void UnregisterArea(Interactable interactable)
	{
		interactableAreas.Remove(interactable);
	}

	public void Interact()
	{
		if (interactableAreas.Any())
		{
			interactableAreas.First().Interact();
		}
	}
}
