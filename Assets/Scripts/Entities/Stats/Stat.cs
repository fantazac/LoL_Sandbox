using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    [SerializeField]
    protected float baseValue;
    [SerializeField]
    protected float currentValue;
    [SerializeField]
    protected float flatBonus;
    [SerializeField]
    protected float percentBonus;
    [SerializeField]
    protected float flatMalus;
    [SerializeField]
    protected float percentMalus;
	
    public float GetBaseValue()
    {
        return baseValue;
    }

    public void SetBaseValue(float baseValue)
    {
        this.baseValue = baseValue;
        UpdateCurrentValue();
    }

    public float GetCurrentValue()
    {
        return currentValue;
    }

    public float GetFlatBonus()
    {
        return flatBonus;
    }

    public void SetFlatBonus(float flatBonus)
    {
        this.flatBonus = flatBonus;
        UpdateCurrentValue();
    }

    public float GetPercentBonus()
    {
        return percentBonus;
    }

    public void SetPercentBonus(float percentBonus)
    {
        this.percentBonus = percentBonus;
        UpdateCurrentValue();
    }

    public virtual void UpdateCurrentValue()
    {
        currentValue = (baseValue + flatBonus) * (1 + percentBonus) * (1 - percentMalus) - flatMalus;
    }
}
