using Godot;
using System.Collections.Generic;

public partial class CropManager : Node2D
{
	public static CropManager Instance;

	private static readonly Vector2I PlowedTile = new Vector2I(8, 6);

	private Dictionary<Vector2I, CropData> fieldData = new Dictionary<Vector2I, CropData>();

	public override void _Ready()
	{
		Instance = this;

		for (int x = 19; x <= 29; x++)
		{
			for (int y = 12; y <= 14; y++)
			{
				fieldData.Add(new Vector2I(x, y), new CropData());
			}
		}
	}

	public ToolUseResult TryUseToolOnTile(Vector2I tile)
	{
		GD.Print(tile);

		if (!fieldData.TryGetValue(tile, out CropData tileCrop))
		{
			return new ToolUseResult(false, null);
		}

		GD.Print(tileCrop.State);

		if (tileCrop.State == CropState.Unplowed)
		{
			tileCrop.State++;
			return new ToolUseResult(true, PlowedTile);
		}
		else if (tileCrop.State == CropState.Plowed)
		{
			tileCrop.State++;
			return new ToolUseResult(true, PlowedTile);
		}
		else
		{
			return new ToolUseResult(false, null);
		}
	}
}
