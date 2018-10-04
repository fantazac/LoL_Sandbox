using UnityEngine;

public class Health : Stat
{
    protected float currentValue;

    public delegate void OnHealthReducedHandler();
    public event OnHealthReducedHandler OnHealthReduced;

    public delegate void OnCurrentHealthValueChangedHandler();
    public event OnCurrentHealthValueChangedHandler OnCurrentHealthValueChanged;

    public delegate void OnMaxHealthValueChangedHandler();
    public event OnMaxHealthValueChangedHandler OnMaxHealthValueChanged;

    public float GetCurrentValue()
    {
        return currentValue;
    }

    public void Reduce(float amount)
    {
        currentValue = Mathf.Clamp(currentValue - amount, 0, total);
        if (OnHealthReduced != null)
        {
            OnHealthReduced();
        }
        if (OnCurrentHealthValueChanged != null)
        {
            OnCurrentHealthValueChanged();
        }
    }

    public void Restore(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0, total);
        if (OnCurrentHealthValueChanged != null)
        {
            OnCurrentHealthValueChanged();
        }
    }

    public float GetPercentLeft()
    {
        return currentValue / total;
    }

    public bool IsDead()
    {
        return currentValue <= 0;
    }

    public override void UpdateTotal()
    {
        float previousTotal = total;

        base.UpdateTotal();
        total = Mathf.Clamp(total, 0, float.MaxValue);

        float difference = total - previousTotal;
        currentValue = Mathf.Clamp(currentValue + difference, 0, total);

        if (OnMaxHealthValueChanged != null)
        {
            OnMaxHealthValueChanged();
        }
        if (OnCurrentHealthValueChanged != null)
        {
            OnCurrentHealthValueChanged();
        }
    }
}
