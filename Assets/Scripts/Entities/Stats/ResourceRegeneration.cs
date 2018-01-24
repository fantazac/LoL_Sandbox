using UnityEngine;

public abstract class ResourceRegeneration : Stat
{
    [SerializeField]
    protected ResourceType type;

    public ResourceType GetResourceType()
    {
        return type;
    }

    public override string GetUIText()
    {
        return GetResourceType() + " REGENERATION: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
