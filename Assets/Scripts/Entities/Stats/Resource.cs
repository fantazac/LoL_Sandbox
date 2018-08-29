using UnityEngine;

public abstract class Resource : Stat
{
    [SerializeField]
    protected ResourceType type;
    [SerializeField]
    protected float currentValue;

    public delegate void OnCurrentResourceValueChangedHandler();
    public event OnCurrentResourceValueChangedHandler OnCurrentResourceValueChanged;

    public delegate void OnMaxResourceValueChangedHandler();
    public event OnMaxResourceValueChangedHandler OnMaxResourceValueChanged;

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
        if (OnCurrentResourceValueChanged != null)
        {
            OnCurrentResourceValueChanged();
        }
    }

    public void Restore(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0, total);
        if (OnCurrentResourceValueChanged != null)
        {
            OnCurrentResourceValueChanged();
        }
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

        if (OnMaxResourceValueChanged != null)
        {
            OnMaxResourceValueChanged();
        }
        if (OnCurrentResourceValueChanged != null)
        {
            OnCurrentResourceValueChanged();
        }
    }

    protected override string GetSimpleUIText()
    {
        return GetResourceType() + ": " + GetCurrentValue() + " / " + GetTotal();
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}

public enum ResourceType
{
    ENERGY,
    FURY,
    MANA,
}
