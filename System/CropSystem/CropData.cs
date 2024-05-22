using Godot;

public record CropData
{
    public CropData(Vector2I tile)
    {
        Tile = tile;
    }

    public Vector2I Tile { get; set; }
    public CropTemplate CropGrowing { get; set; }
    public CropState State { get; set; }
    public int DaysGrowing { get; set; }
    public Vector2I? TileMap { get; set; }

    public void Reset()
    {
        CropGrowing = null;
        State = CropState.Plowed;
        DaysGrowing = 0;
        TileMap = null;
    }
}