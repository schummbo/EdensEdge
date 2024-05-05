using Godot;
using System;

public partial class Interactable : CollisionShape2D
{
	public void Interact()
	{
		GD.Print("Interacted");
	}
}
