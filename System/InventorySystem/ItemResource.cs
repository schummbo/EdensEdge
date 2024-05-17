using Godot;

[GlobalClass]
public partial class ItemResource : Resource
{
    [Export] public string Name { get; set; }
    [Export] public Texture2D Texture { get; set; }
}
