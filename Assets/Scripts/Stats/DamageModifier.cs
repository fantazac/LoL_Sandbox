public class DamageModifier : Stat
{
    public DamageModifier()
    {
        percentBonus = 100;
        percentMalus = 100;
        UpdateTotal();
    }

    protected override void UpdateTotal()
    {
        total = percentBonus * percentMalus * 0.0001f;
    }

    protected override void CalculatePercentBonusIncrease(float percentBonus)
    {
        this.percentBonus *= percentBonus * 0.01f;
    }

    protected override void CalculatePercentBonusReduction(float percentBonus)
    {
        this.percentBonus /= (percentBonus * 0.01f);
    }

    protected override void CalculatePercentMalusIncrease(float percentMalus)
    {
        this.percentMalus *= percentMalus * 0.01f;
    }

    protected override void CalculatePercentMalusReduction(float percentMalus)
    {
        this.percentMalus /= (percentMalus * 0.01f);
    }
}
