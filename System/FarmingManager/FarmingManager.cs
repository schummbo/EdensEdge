using EdensEdge.Scripts;
using Godot;
using System;
using System.Collections.Generic;

public partial class FarmingManager : Node2D
{
	public static FarmingManager Instance;

	private PlayerController player;
	private Field field;

	private Vector2I? previousMarker;

	private Dictionary<Vector2, CropData> crops;
	private AllCropsTemplate allCropTemplates;

	public override void _Ready()
	{
		Instance = this;

		player = this.GetTree().GetFirstNodeInGroup("Player") as PlayerController;
		field = this.GetTree().GetFirstNodeInGroup("Field") as Field;

		// initialize the fields dynamically somehow
		crops = new Dictionary<Vector2, CropData>
		{
			{ new Vector2I(19, 12), new CropData { Tile = new Vector2I(19, 12) } }
		};

		allCropTemplates = GD.Load<AllCropsTemplate>("res://System/CropSystem/CropTypes/AllCrops.tres");
	}

	public override void _Process(double delta)
	{
		PlaceFarmMarker();
	}

	private void PlaceFarmMarker()
	{
		// get player position
		var playerPosition = player.Position;
		Vector2I playerTile = field.LocalToMap(playerPosition);
		Vector2I mouseTile = field.LocalToMap(field.GetLocalMousePosition());

		Vector2I closestToMouse = GetTileAroundPlayerClosestToMouse(playerTile, mouseTile);

		if (previousMarker.HasValue)
			field.RemoveMarker(previousMarker.Value);

		field.SetMarker(closestToMouse);

		previousMarker = closestToMouse;
	}

	private Vector2I GetTileAroundPlayerClosestToMouse(Vector2I playerTile, Vector2I mouseTile)
	{
		float minDistance = float.MaxValue;
		Vector2 closestTilePos = new Vector2(-1, -1);

		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				Vector2 tilePos = playerTile + new Vector2(i, j);

				float distance = tilePos.DistanceTo(mouseTile);

				if (distance < minDistance)
				{
					minDistance = distance;
					closestTilePos = tilePos;
				}
			}
		}

		return (Vector2I)closestTilePos;
	}

	internal bool Interact(ItemResource item)
	{
		if (previousMarker.HasValue)
		{
			if (TendCrop(previousMarker.Value, item))
			{
				RenderTile(previousMarker.Value);
			}
		}

		return false;
	}

	private void RenderTile(Vector2I tile)
	{
		var crop = crops[tile];
		field.UpdateCrop(crop);
	}

	// TODO: Pass tool and apply tool rules to states
	public bool TendCrop(Vector2I tile, ItemResource itemResource)
	{
		if (itemResource == null)
		{
			return false;
		}

		if (!crops.TryGetValue(tile, out CropData crop))
		{
			return false;
		}

		CropTemplate cropTemplate = null;

		if (crop.Name != null)
		{
			cropTemplate = allCropTemplates.GetCropTemplate(crop.Name);
		}

		var stateMachine = new CropStateMachine(crop, cropTemplate);

		if (stateMachine.CanPlow() && itemResource.Name == "Hoe")
		{
			stateMachine.Plow();
			return true;
		}

		if (stateMachine.CanSeed())
		{
			var cabbage = allCropTemplates.GetCropTemplate("cabbage");
			stateMachine.Seed(cabbage);
			return true;
		}

		if (stateMachine.CanHarvest())
		{
			stateMachine.Harvest();
			return true;
		}

		return false;
	}

	//TODO: Make private and handle tick event from time keeper
	public void HandleTick()
	{
		foreach (CropData crop in crops.Values)
		{
			if (crop.State != CropState.Growing)
			{
				return;
			}

			var template = allCropTemplates.GetCropTemplate(crop.Name);

			var sm = new CropStateMachine(crop, template);
			sm.Grow();

			field.UpdateCrop(crop);
		}
	}
}
