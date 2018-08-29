using UnityEngine;

public abstract class ResourceRegeneration : Stat
{
    [SerializeField]
    protected ResourceType type;

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
