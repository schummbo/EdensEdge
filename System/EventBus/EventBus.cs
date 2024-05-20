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
    public event Action<ItemResource> OnInventoryItemUsed;
    public event Action<ItemResource, int> OnInventoryItemAdded;

    public void InventoryItemSelected(ItemResource itemResource)
    {
        this.OnInventoryItemSelected.Invoke(itemResource);
    }

    internal void InventoryItemUsed(ItemResource equippedItem)
    {
        this.OnInventoryItemUsed(equippedItem);
    }

    internal void InventoryItemAdded(ItemResource item, int amountToAdd)
    {
        this.OnInventoryItemAdded(item, amountToAdd);
    }
}