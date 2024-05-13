using Godot;
using System;
using System.Collections.Generic;

public partial class Field : TileMap
{
	private enum TileMapLayers
	{
		Ground = 0,
		Field = 1,
		Marker = 2
	}

	private enum TileSets
	{
		Ground = 0,
		Marker = 1
	}

	private static readonly Vector2I PlowedTile = new Vector2I(8, 6);

	private Dictionary<Vector2I, CropData> fieldData = new Dictionary<Vector2I, CropData>();

	public override void _Ready()
	{
		for (int x = 19; x <= 29; x++)
		{
			for (int y = 12; y <= 14; y++)
			{
				fieldData.Add(new Vector2I(x, y), new CropData());
			}
		}
	}

	internal void SetMarker(Vector2I closestToMouse)
	{
		this.SetCell(
			(int)TileMapLayers.Marker,
			closestToMouse,
			(int)TileSets.Marker,
			new Vector2I(0, 0));
	}

	internal void RemoveMarker(Vector2I previousMarker)
	{
		this.SetCell(
			(int)TileMapLayers.Marker,
			previousMarker,
			(int)TileSets.Marker);
	}

	internal bool UseToolOnTile(Vector2I tile) //Tool tool
	{
		GD.Print(tile);

		if (!fieldData.TryGetValue(tile, out CropData tileCrop))
		{
			return false;
		}

		GD.Print(tileCrop.State);

		if (tileCrop.State == CropState.Unplowed)
		{
			tileCrop.State++;
		}
		else if (tileCrop.State == CropState.Plowed)
		{
			tileCrop.State++;
		}
		else if (tileCrop.State == CropState.Growing)
		{
			tileCrop.State++;
		}
		else
		{
			return false;
		}

		GD.Print(tileCrop.State);

		RedrawTile(tile);

		return true;
	}

	private void RedrawTile(Vector2I tile)
	{
		if (!fieldData.TryGetValue(tile, out CropData tileCrop))
		{
			return;
		}

		if (tileCrop.State == CropState.Plowed)
		{
			this.SetCell((int)TileMapLayers.Field, tile, (int)TileSets.Ground, PlowedTile);
		}
	}

}
