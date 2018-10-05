public class MagicResistance : Stat
{
    public MagicResistance(float initialBaseValue) : base(initialBaseValue) { }
    public MagicResistance(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

    public override void UpdateTotal()
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

    private float GetMagicDamageTakenMultiplier()
    {
        if (total >= 0)
        {
            return 100 / (100 + total);
        }
        else
        {
            return 2 - 100 / (100 - total);
        }
    }

    public float GetMagicEffectiveHealthPercent()
    {
        return 1 + (GetMagicDamageReductionPercent() / GetMagicDamageTakenMultiplier());
    }

    public float GetMagicDamageReductionPercent()
    {
        return 1 - GetMagicDamageTakenMultiplier();
    }
}
