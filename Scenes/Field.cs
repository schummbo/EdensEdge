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
		var result = CropManager.Instance.TryUseToolOnTile(tile);

		if (!result.Success)
		{
			return false;
		}

		RedrawTile(tile, result.NewTileResult.Value);

		return true;
	}

	private void RedrawTile(Vector2I tilePos, Vector2I newTile)
	{
		this.SetCell(
			(int)TileMapLayers.Field,
			tilePos,
			(int)TileSets.Ground,
			newTile);
	}
}
