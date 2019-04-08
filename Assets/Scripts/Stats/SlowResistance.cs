public class SlowResistance : PercentBonusOnlyStat
{
    public delegate void OnSlowResistanceChangedHandler(float slowResistance);
    public event OnSlowResistanceChangedHandler OnSlowResistanceChanged;

    protected override void UpdateTotal()
    {
        base.UpdateTotal();
        OnSlowResistanceChanged?.Invoke(total);
    }
}
