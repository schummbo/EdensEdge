using System;
using Godot;

public partial class Field : TileMap
{
	internal void SetMarker(Vector2I closestToMouse)
	{
		this.SetCell(2, closestToMouse, 1, new Vector2I(0, 0));
	}

	internal void RemoveMarker(Vector2I previousMarker)
	{
		this.SetCell(2, previousMarker, 1);
	}

	internal bool UseToolOnTile(Vector2I tile)
	{
		TileData tileData = this.GetCellTileData(1, tile);
		if (tileData == null)
			return false;

		Variant customData = tileData.GetCustomDataByLayerId(0);

		return customData.As<bool>();
	}
}
