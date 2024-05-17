using Godot;
using System;

public partial class InventorySlot : Panel
{
	private Sprite2D itemVisual;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemVisual = GetNode<Sprite2D>("CenterContainer/Panel/ItemDisplay");
	}

	public void Update(ItemResource itemResource)
	{
		if (itemResource == null)
		{
			itemVisual.Visible = false;
		}
		else
		{
			itemVisual.Visible = true;
			itemVisual.Texture = itemResource.Texture;
		}
	}
}
