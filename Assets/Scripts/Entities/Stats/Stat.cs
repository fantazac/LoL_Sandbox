public abstract class Stat
{
    private static float BASE_PERCENT_VALUE = 0.7025f;
    private static float BASE_PERCENT_PER_LEVEL_VALUE = 0.0175f;

    protected readonly float initialBaseValue;
    protected readonly float perLevelValue;

    protected float currentBaseValue;
    protected float total;
    protected float flatBonus;
    protected float percentBonus;
    protected float flatMalus;
    protected float percentMalus;

    public Stat() : this(0, 0) { }
    public Stat(float initialBaseValue) : this(initialBaseValue, 0) { }
    public Stat(float initialBaseValue, float perLevelValue)
    {
        this.initialBaseValue = initialBaseValue;
        this.perLevelValue = perLevelValue;
        OnLevelUp(1);
    }

    public abstract void UpdateTotal();

    public virtual void OnLevelUp(int level)
    {
        currentBaseValue = initialBaseValue + calculateStatTotalLevelBonus(level);
        UpdateTotal();
    }

    protected virtual float calculateStatTotalLevelBonus(int level)
    {
        return perLevelValue * (level - 1) * (BASE_PERCENT_VALUE + BASE_PERCENT_PER_LEVEL_VALUE * (level - 1));
    }

    public float GetBonus()
    {
        return GetTotal() - GetCurrentBaseValue();
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

    public void AddPercentBonus(float percentBonus)
    {
        CalculatePercentBonusIncrease(percentBonus);
        UpdateTotal();
    }

    protected virtual void CalculatePercentBonusIncrease(float percentBonus)
    {
        this.percentBonus += percentBonus;
    }

    public void RemovePercentBonus(float percentBonus)
    {
        CalculatePercentBonusReduction(percentBonus);
        UpdateTotal();
    }

    protected virtual void CalculatePercentBonusReduction(float percentBonus)
    {
        this.percentBonus -= percentBonus;
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

    public void AddPercentMalus(float percentMalus)
    {
        CalculatePercentMalusIncrease(percentMalus);
        UpdateTotal();
    }

    protected virtual void CalculatePercentMalusIncrease(float percentMalus)
    {
        this.percentMalus += percentMalus;
    }

    public void RemovePercentMalus(float percentMalus)
    {
        CalculatePercentMalusReduction(percentMalus);
        UpdateTotal();
    }

    protected virtual void CalculatePercentMalusReduction(float percentMalus)
    {
        this.percentMalus -= percentMalus;
    }

    public float GetInitialBaseValue()
    {
        return initialBaseValue;
    }

    public float GetCurrentBaseValue()
    {
        return currentBaseValue;
    }

    public float GetPerLevelValue()
    {
        return perLevelValue;
    }

    public virtual float GetTotal()
    {
        return total;
    }

    public float GetFlatBonus()
    {
        return flatBonus;
    }

    public float GetPercentBonus()
    {
        return percentBonus;
    }

    public float GetFlatMalus()
    {
        return flatMalus;
    }

    public float GetPercentMalus()
    {
        return percentMalus;
    }
}
