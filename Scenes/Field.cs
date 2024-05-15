using System;
using Godot;

public partial class Field : TileMap
{
	private enum TileMapLayers
	{
		Ground = 0,
		Field = 1,
		Crops = 2,
		Marker = 3
	}

	private enum TileSets
	{
		Ground = 0,
		Marker = 1,
		Crops = 2
	}

	private Vector2I UnplowedTile = new Vector2I(5, 6);
	private Vector2I PlowedTile = new Vector2I(8, 6);

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

	internal void UpdateCrop(CropData crop)
	{
		if (crop.State == CropState.Unplowed)
		{
			this.SetCell(
				(int)TileMapLayers.Field,
				crop.Tile,
				(int)TileSets.Ground,
				UnplowedTile);

			this.SetCell(
				(int)TileMapLayers.Crops,
				crop.Tile);

			return;
		}

		if (crop.State == CropState.Plowed)
		{
			this.SetCell(
				(int)TileMapLayers.Field,
				crop.Tile,
				(int)TileSets.Ground,
				PlowedTile);


			this.SetCell(
				(int)TileMapLayers.Crops,
				crop.Tile);

			return;
		}

		if (crop.State == CropState.Growing || crop.State == CropState.ReadyForHarvest)
		{
			this.SetCell(
				(int)TileMapLayers.Crops,
				crop.Tile,
				(int)TileSets.Crops,
				crop.TileMap);

			return;
		}
	}

}
