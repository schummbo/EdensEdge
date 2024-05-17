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

		Close();
		UpdateSlots();
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

			slot.Update(item);
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
