public class PhysicalDamageIncreaseModifier : Stat
{
    public PhysicalDamageIncreaseModifier() : base(1f)
    {
        percentBonus = 1f;
        percentMalus = 1f;
    }

    public override void UpdateTotal()
    {
        total = currentBaseValue * percentBonus * percentMalus;
    }

    protected override void CalculatePercentBonusIncrease(float percentBonus)
    {
        this.percentBonus *= percentBonus;
    }

    protected override void CalculatePercentBonusReduction(float percentBonus)
    {
        this.percentBonus /= percentBonus;
    }

    protected override void CalculatePercentMalusIncrease(float percentMalus)
    {
        this.percentMalus *= percentMalus;
    }

    protected override void CalculatePercentMalusReduction(float percentMalus)
    {
        this.percentMalus /= percentMalus;
    }
}
