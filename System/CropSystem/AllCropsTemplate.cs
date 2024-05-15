using System.Linq;
using Godot;

[GlobalClass]
public partial class AllCropsTemplate : Resource
{
    [Export] public CropTemplate[] AllCrops { get; set; }

    public CropTemplate GetCropTemplate(string name)
    {
        return AllCrops.First(c => string.Equals(c.CropName, name, System.StringComparison.InvariantCultureIgnoreCase));
    }
}