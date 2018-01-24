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

    public void Reduce(float amount)
    {
        currentValue = Mathf.Clamp(currentValue - amount, 0, total);
    }

    public void Restore(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0, total);
    }

    public float GetPercentLeft()
    {
        return currentValue / total;
    }

    public override void UpdateTotal()
    {
        float previousTotal = total;

        base.UpdateTotal();
        total = Mathf.Clamp(total, 0, float.MaxValue);

        float difference = total - previousTotal;
        currentValue = Mathf.Clamp(currentValue + difference, 0, total);
    }

    public override string GetUIText()
    {
        return GetResourceType() + ": " + GetCurrentValue() + " / " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}

public enum ResourceType
{
    ENERGY,
    FURY,
    MANA,
}
