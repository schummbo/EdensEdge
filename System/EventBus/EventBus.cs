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
    public event Action<string> OnFieldLoaded;

    // Fires every in-game minute
    public event Action<int, int, int, float> OnTimeTick;

    // Fires when the phase of the day changes
    public event Action<TimePhase> OnTimePhaseTick;

    public event Action<int> OnDayTick;

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

    public void FieldLoaded(string fieldName)
    {
        this.OnFieldLoaded(fieldName);
    }

    public void TimeTick(int day, int hour, int minute, float rawTime)
    {
        this.OnTimeTick(day, hour, minute, rawTime);
    }

    public void TimePhaseTick(TimePhase timePhase)
    {
        this.OnTimePhaseTick(timePhase);
    }

    public void DayTick(int day)
    {
        this.OnDayTick(day);
    }
}