using System;
using EdensEdge.Scripts;
using Godot;

public partial class FarmingManager : Node2D
{
	public static FarmingManager Instance;

	private PlayerController player;
	private Field field;

	private Vector2I? previousMarker;

	public override void _Ready()
	{
		Instance = this;

		player = this.GetTree().GetFirstNodeInGroup("Player") as PlayerController;
		field = this.GetTree().GetFirstNodeInGroup("Field") as Field;
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

		Vector2I closestToMouse = ClosestTileToMouse(playerTile, mouseTile);

		if (previousMarker.HasValue)
			field.RemoveMarker(previousMarker.Value);

		field.SetMarker(closestToMouse);

		previousMarker = closestToMouse;
	}

	private Vector2I ClosestTileToMouse(Vector2I playerTile, Vector2I mouseTile)
	{
		// Initialize the minimum distance to a large value
		float minDistance = float.MaxValue;

		// Initialize the closest tile position to an invalid value
		Vector2 closestTilePos = new Vector2(-1, -1);

		// Loop over the 3x3 grid around the player
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				// Calculate the position of the current tile
				Vector2 tilePos = playerTile + new Vector2(i, j);

				// Calculate the distance from the current tile to the mouse
				float distance = tilePos.DistanceTo(mouseTile);

				// If the distance is smaller than the minimum distance, update the minimum distance and the closest tile position
				if (distance < minDistance)
				{
					minDistance = distance;
					closestTilePos = tilePos;
				}
			}
		}

		return (Vector2I)closestTilePos;
	}

	internal bool Interact()
	{
		if (previousMarker.HasValue)
		{
			return field.UseToolOnTile(previousMarker.Value);
		}

		return false;
	}

}
