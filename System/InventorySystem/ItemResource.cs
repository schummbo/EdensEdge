using System;
using Godot;

[GlobalClass]
public partial class ItemResource : Resource
{
    [Export] public string Name { get; set; }

    [Export] public TileSets TileSet { get; set; }

    [Export] public Vector2I TileSetPosition { get; set; }

    [Export] public CropTemplate ProducesWhenGrown { get; set; }

    [Export] public bool Stackable { get; set; }

    [Export] public bool DestroyedOnUse { get; set; }
}
