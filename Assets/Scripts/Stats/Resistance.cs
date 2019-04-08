public class Resistance : Stat
{
    public Resistance(float initialBaseValue) : base(initialBaseValue) { }
    public Resistance(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

    protected override void UpdateTotal()
    {
        total = ((currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f)) - flatMalus) * (1 - (percentMalus * 0.01f));
    }

    protected override void CalculatePercentMalusIncrease(float percentMalus)
    {
        this.percentMalus = 100 - (100 - this.percentMalus) * (100 - percentMalus) * 0.01f;
    }

    protected override void CalculatePercentMalusReduction(float percentMalus)
    {
        this.percentMalus = 100 - (100 - this.percentMalus) / ((100 - percentMalus) * 0.01f);
    }

    private float GetResistanceDamageReceivedModifier()
    {
        if (total >= 0)
        {
            return 100 / (100 + total);
        }
        else
        {
            return 2 - (100 / (100 - total));
        }
    }

    public float GetEffectiveHealthPercent()
    {
        return 1 + (GetDamageReductionPercent() / GetResistanceDamageReceivedModifier());
    }

    public float GetDamageReductionPercent()
    {
        return 1 - GetResistanceDamageReceivedModifier();
    }
}
