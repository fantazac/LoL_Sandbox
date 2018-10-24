using UnityEngine;

public class Resource : Stat
{
    protected float currentValue;

    public delegate void OnResourceReducedHandler();
    public event OnResourceReducedHandler OnResourceReduced;

    public delegate void OnCurrentResourceChangedHandler();
    public event OnCurrentResourceChangedHandler OnCurrentResourceChanged;

    public delegate void OnMaxResourceValueChangedHandler();
    public event OnMaxResourceValueChangedHandler OnMaxResourceChanged;

    public Resource(float initialBaseValue) : base(initialBaseValue) { }
    public Resource(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

    public float GetCurrentValue()
    {
        return currentValue;
    }

    public void Reduce(float amount)
    {
        currentValue = Mathf.Clamp(currentValue - amount, 0, total);
        if (OnResourceReduced != null)
        {
            OnResourceReduced();
        }
        if (OnCurrentResourceChanged != null)
        {
            OnCurrentResourceChanged();
        }
    }

    public void Restore(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0, total);
        if (OnCurrentResourceChanged != null)
        {
            OnCurrentResourceChanged();
        }
    }

    public float GetPercentLeft()
    {
        return currentValue / total;
    }

    public override void UpdateTotal()
    {
        float previousTotal = total;

        total = Mathf.Clamp((currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f)), 0, float.MaxValue);

        float difference = total - previousTotal;
        currentValue = Mathf.Clamp(currentValue + (difference > 0 ? difference : 0), 0, total);

        if (OnMaxResourceChanged != null)
        {
            OnMaxResourceChanged();
        }
        if (OnCurrentResourceChanged != null)
        {
            OnCurrentResourceChanged();
        }
    }
}
