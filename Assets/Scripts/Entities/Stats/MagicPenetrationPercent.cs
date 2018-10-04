public class MagicPenetrationPercent : Stat
{
    public override void SetBaseValue(float baseValue)
    {
        RemovePercentBonus(this.baseValue);
        this.baseValue = baseValue;
        AddPercentBonus(this.baseValue);
        UpdateTotal();
    }

    public override void UpdateTotal()
    {
        total = percentBonus * (100 - percentMalus);
    }

    public override void AddPercentBonus(float percentBonus)
    {
        this.percentBonus += (100 - this.percentBonus) * percentBonus * 0.01f;
        UpdateTotal();
    }

    public override void RemovePercentBonus(float percentBonus)
    {
        this.percentBonus -= (100 - this.percentBonus) * percentBonus * 0.01f;
        UpdateTotal();
    }
}
