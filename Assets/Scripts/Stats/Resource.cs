﻿using UnityEngine;

public class Resource : Stat
{
    protected float currentValue;

    public delegate void OnResourceReducedHandler();
    public event OnResourceReducedHandler OnResourceReduced;

    public delegate void OnCurrentResourceChangedHandler(float currentValue);
    public event OnCurrentResourceChangedHandler OnCurrentResourceChanged;

    public Resource(float initialBaseValue) : base(initialBaseValue) { }
    public Resource(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

    public float GetCurrentValue()
    {
        return currentValue;
    }

    public void Reduce(float amount)
    {
        currentValue = Mathf.Clamp(currentValue - amount, 0, total);
        OnResourceReduced?.Invoke();
        OnCurrentResourceChanged?.Invoke(currentValue);
    }

    public void Restore(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0, total);
        OnCurrentResourceChanged?.Invoke(currentValue);
    }

    public float GetCurrentPercentResource()
    {
        return currentValue / total;
    }
    
    public float GetMissingPercentResource()
    {
        return 1 - currentValue / total;
    }

    protected override void UpdateTotal()
    {
        float previousTotal = total;

        total = Mathf.Clamp((currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f)), 0, float.MaxValue);

        float difference = total - previousTotal;
        currentValue = Mathf.Clamp(currentValue + (difference > 0 ? difference : 0), 0, total);

        OnCurrentResourceChanged?.Invoke(currentValue);
    }
}
