using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

[Tool]
public partial class InteractableManager : Node2D
{
	[Export]
	public ShapeCast2D ShapeCast { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint()) return;

		if (Input.IsActionJustPressed("interact"))
		{
			for (int i = 0; i < ShapeCast.GetCollisionCount(); i++)
			{
				var collider = (Node2D)ShapeCast.GetCollider(i);
				GD.Print(collider.Name);
			}
		}
	}

	public override string[] _GetConfigurationWarnings()
	{
		var warnings = base._GetConfigurationWarnings()?.ToList() ?? new List<string>();

		if (ShapeCast == null)
		{
			warnings.Add("A ShapeCast2D is required for this scene to work properly.");
		}

		return warnings.ToArray();
	}
}
