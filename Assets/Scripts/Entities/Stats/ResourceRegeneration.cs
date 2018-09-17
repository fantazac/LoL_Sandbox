public class ResourceRegeneration : Stat
{
    protected ResourceType type;

    public ResourceRegeneration(ResourceType resourceType)
    {
        type = resourceType;
    }

    public ResourceType GetResourceType()
    {
        return type;
    }

    protected override string GetSimpleUIText()
    {
        return GetResourceType() + " REGENERATION: " + GetTotal();
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
