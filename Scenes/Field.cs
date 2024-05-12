using Godot;
using System;
using System.Collections.Generic;

public partial class Field : TileMap
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	internal void SetMarker(Vector2I closestToMouse)
	{
		this.SetCell(2, closestToMouse, 1, new Vector2I(0, 0));
	}

	internal void RemoveMarker(Vector2I previousMarker)
	{
		this.SetCell(2, previousMarker, 1);
	}
}
