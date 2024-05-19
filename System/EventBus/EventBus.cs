using System;
using Godot;

public partial class EventBus : Node
{
    public static EventBus Instance { get; set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public event Action<ItemResource> OnInventoryItemSelected;

    public void InventoryItemSelected(ItemResource itemResource)
    {
        this.OnInventoryItemSelected.Invoke(itemResource);
    }
}