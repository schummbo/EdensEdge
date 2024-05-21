using System.Linq;
using Godot;

public class CropStateMachine(CropData crop)
{
    public bool CanPlow()
    {
        return crop.State == CropState.Unplowed;
    }

    public void Plow()
    {
        crop.State = CropState.Plowed;
    }

    public bool CanSeed()
    {
        return crop.State == CropState.Plowed;
    }

    public void Seed(CropTemplate cropTemplate)
    {
        crop.State = CropState.Growing;
        crop.CropGrowing = cropTemplate;
        crop.TileMap = cropTemplate.GrowthPhases.First().GrowthTile;
    }

    public bool CanHarvest()
    {
        return crop.State == CropState.ReadyForHarvest;
    }

    public (ItemResource, int) Harvest()
    {
        var itemGrown = crop.CropGrowing.ProducesWhenHarvested;
        var numProduced = crop.CropGrowing.NumberProduced;
        crop.Reset();
        return (itemGrown, numProduced);
    }

    public void Grow()
    {
        if (crop.CropGrowing == null)
        {
            GD.PushWarning("Attempt to grow crop but no template");
            return;
        }

        if (crop.State != CropState.Growing)
        {
            return;
        }

        // age it a day
        crop.DaysGrowing++;

        // check the template to see what growth phase we're in
        var growthPhase = crop.CropGrowing.GrowthPhases.First(gp =>
            crop.DaysGrowing >= gp.DaysStart &&
            crop.DaysGrowing <= gp.DaysEnd);

        // change the tile
        crop.TileMap = growthPhase.GrowthTile;

        // at the end of the growth phases, set to ready for harvest
        if (growthPhase.IsLast && crop.DaysGrowing == growthPhase.DaysEnd)
        {
            crop.State = CropState.ReadyForHarvest;
        }
    }
}