using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    protected const float BASE_PERCENTAGE_ON_LEVEL_UP = 0.65f;
    protected const float ADDITIVE_PERCENTAGE_PER_LEVEL = 0.035f;

    [SerializeField]
    protected float baseValue;
    [SerializeField]
    protected float perLevelValue;
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

    public float GetPerLevelValue()
    {
        return baseValue;
    }

    public void SetPerLevelValue(float perLevelValue)
    {
        this.perLevelValue = perLevelValue;
    }

    public virtual void OnLevelUp(int level)
    {
        SetBaseValue(((BASE_PERCENTAGE_ON_LEVEL_UP + (ADDITIVE_PERCENTAGE_PER_LEVEL * level)) * perLevelValue) + baseValue);
    }

    public virtual float GetTotal()
    {
        return total;
    }

    public virtual float GetBonus()
    {
        return GetTotal() - GetBaseValue();
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

    public void RemoveFlatBonus(float flatBonus)
    {
        this.flatBonus -= flatBonus;
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

    public void RemovePercentBonus(float percentBonus)
    {
        this.percentBonus -= percentBonus;
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

    public void RemoveFlatMalus(float flatMalus)
    {
        this.flatMalus -= flatMalus;
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

    public void RemovePercentMalus(float percentMalus)
    {
        this.percentMalus -= percentMalus;
        UpdateTotal();
    }

    public virtual void UpdateTotal()
    {
        total = (baseValue + flatBonus) * (1 + (percentBonus * 0.01f)) * (1 - (percentMalus * 0.01f)) - flatMalus;
    }

    // TODO FIXME: the UI is not this class' concern. Move this method somewhere else.
    public abstract string GetUIText();
}
