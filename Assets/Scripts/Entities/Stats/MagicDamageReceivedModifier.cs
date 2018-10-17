public class MagicDamageReceivedModifier : Stat
{
    public MagicDamageReceivedModifier() : base()
    {
        percentBonus = 100f;
        percentMalus = 100f;
        UpdateTotal();
    }

    public override void UpdateTotal()
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
