using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class CropTemplate : Resource
{
	[Export] public string CropName { get; set; }

	[Export] public GrowthPhase[] GrowthPhases { get; set; }

	[Export] public ItemResource ProducesWhenHarvested { get; set; }

	[Export] public int NumberProduced { get; set; }
}

