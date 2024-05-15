using Godot;

public record CropData
{
    public Vector2I Tile { get; set; }
    public string Name { get; set; }
    public CropState State { get; set; }
    public int DaysGrowing { get; set; }
    public Vector2I? TileMap { get; set; }

    public void Reset()
    {
        Name = null;
        State = CropState.Plowed;
        DaysGrowing = 0;
        TileMap = null;
    }
}