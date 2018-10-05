public class Tenacity : Stat
{
    public Tenacity() : base() { }

    public override void UpdateTotal()
    {
        total = percentBonus * (1 - (percentMalus * 0.01f));
    }

    protected override void CalculatePercentBonusIncrease(float percentBonus)
    {
        this.percentBonus = 100 - (100 - this.percentBonus) * (100 - percentBonus) * 0.01f;
    }

    protected override void CalculatePercentBonusReduction(float percentBonus)
    {
        this.percentBonus = 100 - (100 - this.percentBonus) / ((100 - percentBonus) * 0.01f);
    }

    //TODO: Double check if it stacks multiplicatively when affected by 2 stacks of ornn brittle (30% each, so is that 60% or lower)
    protected override void CalculatePercentMalusIncrease(float percentMalus)
    {
        this.percentMalus = 100 - (100 - this.percentMalus) * (100 - percentMalus) * 0.01f;
    }

    protected override void CalculatePercentMalusReduction(float percentMalus)
    {
        this.percentMalus = 100 - (100 - this.percentMalus) / ((100 - percentMalus) * 0.01f);
    }
}
