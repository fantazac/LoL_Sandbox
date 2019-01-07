public class ResistancePenetrationPercent : Stat
{
    public override void UpdateTotal()
    {
        total = percentBonus * 0.01f;
    }

    protected override void CalculatePercentBonusIncrease(float percentBonus)
    {
        this.percentBonus = 100 - (100 - this.percentBonus) * (100 - percentBonus) * 0.01f;
    }

    protected override void CalculatePercentBonusReduction(float percentBonus)
    {
        this.percentBonus = 100 - (100 - this.percentBonus) / ((100 - percentBonus) * 0.01f);
    }
}
