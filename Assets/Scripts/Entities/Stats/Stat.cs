using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    [SerializeField]
    protected float baseValue;
    [SerializeField]
    protected float total;
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
        UpdateTotal();
    }

    public virtual float GetTotal()
    {
        return total;
    }

    public float GetFlatBonus()
    {
        return flatBonus;
    }

    public void AddFlatBonus(float flatBonus)
    {
        this.flatBonus += flatBonus;
        UpdateTotal();
    }

    public float GetPercentBonus()
    {
        return percentBonus;
    }

    public void AddPercentBonus(float percentBonus)
    {
        this.percentBonus += percentBonus;
        UpdateTotal();
    }

    public float GetFlatMalus()
    {
        return flatMalus;
    }

    public void AddFlatMalus(float flatMalus)
    {
        this.flatMalus += flatMalus;
        UpdateTotal();
    }

    public float GetPercentMalus()
    {
        return percentMalus;
    }

    public void AddPercentMalus(float percentMalus)
    {
        this.percentMalus += percentMalus;
        UpdateTotal();
    }

    public virtual void UpdateTotal()
    {
        total = (baseValue + flatBonus) * (1 + percentBonus) * (1 - percentMalus) - flatMalus;
    }

    // FIXME: the UI is not this class' concern. Move this method somewhere else.
    public abstract string GetUIText();
}
