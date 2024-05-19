using Godot;
using System;

public partial class InventorySlot : Panel
{
	private Sprite2D itemVisual;
	private Label amountLabel;
	private int amount;
	private ItemResource itemResource;

	public bool IsSelected { get; set; }

	public Action<InventorySlot> OnSelected { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemVisual = GetNode<Sprite2D>("CenterContainer/Panel/ItemDisplay");
		amountLabel = GetNode<Label>("CenterContainer/Panel/Label");
		this.GuiInput += HandleInput;
	}

	private void HandleInput(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("useitem"))
		{
			IsSelected = true;
			EventBus.Instance.InventoryItemSelected(this.itemResource);
		}
	}

	public void Update(ItemResource newResource)
	{
		if (this.itemResource == null)
		{
			this.itemResource = newResource;
			amount = 1;
		}
		else if (newResource == this.itemResource)
		{
			amount++;
		}
		else if (newResource == null)
		{
			this.itemResource = null;
			amount = 0;
		}

		UpdateVisual();
	}

	private void UpdateVisual()
	{
		if (itemResource == null)
		{
			this.itemVisual.Visible = false;
			this.amountLabel.Visible = false;
		}
		else
		{
			this.itemVisual.Texture = itemResource.Texture;
			this.itemVisual.Visible = true;

			this.amountLabel.Text = amount.ToString();
			this.amountLabel.Visible = amount > 0;
		}
	}

}
