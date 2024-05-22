using EdensEdge.Scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FarmingManager : Node2D
{
	public static FarmingManager Instance;

	private PlayerController player;
	private Field field;

	private Vector2I? previousMarker;

	private Dictionary<string, Dictionary<Vector2I, CropData>> cropsByField = new();

	private Dictionary<Vector2I, CropData> currentCrops;

	public override void _Ready()
	{
		Instance = this;

		player = this.GetTree().GetFirstNodeInGroup("Player") as PlayerController;
		field = this.GetTree().GetFirstNodeInGroup("Field") as Field;

		EventBus.Instance.OnFieldLoaded += HandleFieldLoaded;
	}

	private void HandleFieldLoaded(string fieldName)
	{
		field = this.GetTree().GetNodesInGroup("Field")
								 .Select(n => n as Field)
								 .Where(n => n != null && n.Name == fieldName)
								 .FirstOrDefault();

		if (cropsByField.TryGetValue(fieldName, out Dictionary<Vector2I, CropData> crops))
		{
			currentCrops = crops;
			return;
		}

		var fieldCells = field.GetUsedCells((int)TileMapLayers.Field);

		Dictionary<Vector2I, CropData> cropData = new Dictionary<Vector2I, CropData>();

		for (int i = 0; i < fieldCells.Count; i++)
		{
			var position = fieldCells[i];

			var tileData = field.GetCellTileData((int)TileMapLayers.Field, position);
			var isFarmable = (bool)tileData.GetCustomData("farmable");

			if (isFarmable)
			{
				cropData.Add(position, new CropData(position));
			}
		}

		cropsByField.Add(fieldName, cropData);

		currentCrops = cropData;
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
				return true;
			}
		}

		return false;
	}

	private void RenderTile(Vector2I tile)
	{
		var crop = currentCrops[tile];
		field.UpdateCrop(crop);
	}

	// TODO: Pass tool and apply tool rules to states
	public bool TendCrop(Vector2I tile, ItemResource itemResource)
	{
		if (itemResource == null)
		{
			return false;
		}

		if (!currentCrops.TryGetValue(tile, out CropData crop))
		{
			return false;
		}

		var stateMachine = new CropStateMachine(crop);

		if (stateMachine.CanPlow() && itemResource.Name == "Hoe")
		{
			stateMachine.Plow();
			return true;
		}

		if (stateMachine.CanSeed() && itemResource.ProducesWhenGrown != null)
		{
			var item = itemResource.ProducesWhenGrown;
			stateMachine.Seed(item);
			return true;
		}

		if (stateMachine.CanHarvest())
		{
			var (grown, amount) = stateMachine.Harvest();
			EventBus.Instance.InventoryItemAdded(grown, amount);
			return true;
		}

		return false;
	}

	//TODO: Make private and handle tick event from time keeper
	public void HandleTick()
	{
		foreach (var cropData in cropsByField.Values)
		{
			foreach (CropData crop in cropData.Values)
			{
				if (crop.State != CropState.Growing)
				{
					continue;
				}

				var sm = new CropStateMachine(crop);
				sm.Grow();

				field.UpdateCrop(crop);
			}
		}
	}
}
