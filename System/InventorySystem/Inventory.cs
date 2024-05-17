using Godot;

[GlobalClass]
public partial class Inventory : Resource
{
    [Export] public ItemResource[] Items { get; set; }
}
