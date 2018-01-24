using UnityEngine;

public abstract class Resource : Stat
{
    [SerializeField]
    protected ResourceType type;
    [SerializeField]
    protected float currentValue;

    public ResourceType GetResourceType()
    {
        return type;
    }

    public float GetCurrentValue()
    {
        return currentValue;
    }

    public void SetCurrentValue(float currentValue)
    {
        this.currentValue = currentValue;
        UpdateTotal();
    }

    public float GetResourcePercent()
    {
        return currentValue / total;
    }

    public override string GetUIText()
    {
        return GetResourceType() + ": " + GetTotal() + " / " + GetMaximumValue() + " (" + GetBaseResource() + " + " + GetBonusResource() + ")";
    }
}

public enum ResourceType
{
    ENERGY,
    FURY,
    MANA,
}
