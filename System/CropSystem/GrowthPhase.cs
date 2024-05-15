using Godot;


[GlobalClass]
public partial class GrowthPhase : Resource
{
	[Export] public int DaysStart { get; set; }
	[Export] public int DaysEnd { get; set; }
	[Export] public Vector2I GrowthTile { get; set; }
	[Export] public bool IsLast { get; set; }
}