public class Tenacity : Stat
{
    private float abilitiesTenacity;
    private float consumablesTenacity;
    private float itemsTenacity;

    protected override void UpdateTotal()
    {
        total = 1 - ((1 - (abilitiesTenacity * 0.01f)) * (1 - (consumablesTenacity * 0.01f)) * (1 - (itemsTenacity * 0.01f)) * (1 - (percentBonus * 0.01f))) - (percentMalus * 0.01f);
    }

    public void AddPercentTenacity(TenacitySource tenacitySource, float percentBonus)
    {
        switch (tenacitySource)
        {
            case TenacitySource.ABILITY:
                abilitiesTenacity += percentBonus;
                break;
            case TenacitySource.CONSUMABLE:
                consumablesTenacity += percentBonus;
                break;
            case TenacitySource.ITEM:
                itemsTenacity += percentBonus;
                break;
            case TenacitySource.OTHER:
                this.percentBonus = 100 - (100 - this.percentBonus) * (100 - percentBonus) * 0.01f;
                break;
        }
        UpdateTotal();
    }

    public void RemovePercentTenacity(TenacitySource tenacitySource, float percentBonus)
    {
        switch (tenacitySource)
        {
            case TenacitySource.ABILITY:
                abilitiesTenacity -= percentBonus;
                break;
            case TenacitySource.CONSUMABLE:
                consumablesTenacity -= percentBonus;
                break;
            case TenacitySource.ITEM:
                itemsTenacity -= percentBonus;
                break;
            case TenacitySource.OTHER:
                this.percentBonus = 100 - (100 - this.percentBonus) / ((100 - percentBonus) * 0.01f);
                break;
        }
        UpdateTotal();
    }

    protected override void CalculatePercentBonusIncrease(float percentBonus) { }
    protected override void CalculatePercentBonusReduction(float percentBonus) { }
}
