using System;
using System.Linq;
using Godot;

public partial class InventoryUI : Control
{
	[Export] public Inventory PlayerInventory { get; set; }

	private InventorySlot[] slots;

	private bool isOpen = true;

	public override void _Ready()
	{
		var ninePatch = this.GetNode<NinePatchRect>("NinePatchRect");
		slots = ninePatch.GetChild(0).GetChildren().Select(c => c as InventorySlot).ToArray();

		foreach (var slot in slots)
		{
			slot.OnSelected += HandleSelected;
		}

		Close();
		UpdateSlots();

		EventBus.Instance.OnInventoryItemUsed += HandleInventoryItemUsed;
		EventBus.Instance.OnInventoryItemAdded += HandleInventoryItemAdded;
	}

	private void HandleInventoryItemAdded(ItemResource resource, int amountToAdd)
	{
		foreach (var slot in slots)
		{
			if (slot.TryAddInventory(resource, amountToAdd))
			{
				return;
			}
		}
	}

	private void HandleInventoryItemUsed(ItemResource resource)
	{
		foreach (var slot in slots)
		{
			if (slot.TryReduceInventory(resource, 1))
			{
				return;
			}
		}
	}

	private void HandleSelected(InventorySlot selectedSlot)
	{
		foreach (var slot in slots)
		{
			if (slot != selectedSlot)
			{
				slot.SetSelected(false);
			}
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(("inventory")))
		{
			if (isOpen)
			{
				Close();
			}
			else
			{
				Open();
			}
		}
	}

	public void UpdateSlots()
	{
		for (int i = 0; i < PlayerInventory.Items.Count(); i++)
		{
			var slot = slots[i];
			var item = PlayerInventory.Items[i];

			slot.TryAddInventory(item, 1);
		}
	}

	public void Open()
	{
		this.Visible = true;
		isOpen = true;
	}

	public void Close()
	{
		this.Visible = false;
		isOpen = false;
	}
}
