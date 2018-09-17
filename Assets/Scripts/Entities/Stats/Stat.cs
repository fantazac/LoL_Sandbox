public abstract class Stat
{
    protected float baseValue;
    protected float perLevelValue;
    protected float total;
    protected float flatBonus;
    protected float percentBonus;
    protected float flatMalus;
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
        SetBaseValue(((StatValues.BASE_PERCENTAGE_ON_LEVEL_UP + (StatValues.ADDITIVE_PERCENTAGE_PER_LEVEL * level)) * perLevelValue) + baseValue);
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

    // TODO FIXME: the UI is not this class' concern. Move these methods somewhere else.
    public string GetUIText(bool getSimpleText)
    {
        return getSimpleText ? GetSimpleUIText() : GetUIText();
    }
    protected abstract string GetSimpleUIText();
    protected abstract string GetUIText();
}
