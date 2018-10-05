using UnityEngine;

public class Resource : Stat
{
    protected float currentValue;

    public delegate void OnCurrentResourceValueChangedHandler();
    public event OnCurrentResourceValueChangedHandler OnCurrentResourceValueChanged;

    public delegate void OnMaxResourceValueChangedHandler();
    public event OnMaxResourceValueChangedHandler OnMaxResourceValueChanged;

    public Resource(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

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

        total = Mathf.Clamp((currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f)), 0, float.MaxValue);

        float difference = total - previousTotal;
        currentValue = Mathf.Clamp(currentValue + (difference > 0 ? difference : 0), 0, total);

        if (OnMaxResourceValueChanged != null)
        {
            OnMaxResourceValueChanged();
        }
        if (OnCurrentResourceValueChanged != null)
        {
            OnCurrentResourceValueChanged();
        }
    }
}
