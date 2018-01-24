using UnityEngine;

public class Health : Stat
{
    [SerializeField]
    protected float currentValue;

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
    }

    public override string GetUIText()
    {
        return "HEALTH: " + GetCurrentValue() + " / " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
