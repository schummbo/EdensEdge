using Godot;
using System;

public partial class InventorySlot : Panel
{
	private Sprite2D itemVisual;
	private Label amountLabel;
	private int amount;
	private ItemResource itemResource;
	private ColorRect selectedHighlight;

	private bool isSelected;

	public Action<InventorySlot> OnSelected { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemVisual = GetNode<Sprite2D>("CenterContainer/Panel/ItemDisplay");
		amountLabel = GetNode<Label>("CenterContainer/Panel/Label");
		selectedHighlight = GetNode<ColorRect>("CenterContainer/SelectedHighlight");
		GetNode<TextureButton>("Button").Pressed += HandlePressed;
	}

	private void HandlePressed()
	{
		SetSelected(true);
		// propagate event up to whole inventory
		this.OnSelected(this);

		// propagate item selected to rest of game
		EventBus.Instance.InventoryItemSelected(this.itemResource);
	}

	public void SetSelected(bool isSelected)
	{
		this.isSelected = isSelected;
		UpdateVisual();
	}

	public bool TryAddInventory(ItemResource newResource, int amountToAdd)
	{
		bool added = false;

		if (this.itemResource == null)
		{
			this.itemResource = newResource;
			this.amount = amountToAdd;
			added = true;
		}
		else if (newResource == this.itemResource)
		{
			if (this.itemResource.Stackable)
			{
				this.amount += amountToAdd;
				added = true;
			}
		}
		else if (newResource == null)
		{
			this.itemResource = null;
			this.amount = 0;
			added = true;
		}

		if (added)
			UpdateVisual();

		return added;
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
			this.itemVisual.Texture = ImageUtilities.TextureFromTileset(itemResource.TileSet, itemResource.TileSetPosition);
			this.itemVisual.Visible = true;

			this.amountLabel.Text = amount.ToString();
			this.amountLabel.Visible = amount > 0 && itemResource.Stackable;
		}

		selectedHighlight.Visible = isSelected;
	}

	internal bool TryReduceInventory(ItemResource resource, int amountToReduce)
	{
		if (this.itemResource == resource)
		{
			this.amount -= amountToReduce;
			if (this.amount <= 0)
			{
				this.itemResource = null;
				this.amount = 0;

				EventBus.Instance.InventoryItemSelected(null);
			}

			UpdateVisual();
			return true;
		}

		return false;
	}
}
