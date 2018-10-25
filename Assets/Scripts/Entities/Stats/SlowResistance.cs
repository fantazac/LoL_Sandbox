public class SlowResistance : PercentBonusOnlyStat
{
    public delegate void OnSlowResistanceChangedHandler(float slowResistance);
    public event OnSlowResistanceChangedHandler OnSlowResistanceChanged;

    public override void UpdateTotal()
    {
        base.UpdateTotal();
        if (OnSlowResistanceChanged != null)
        {
            OnSlowResistanceChanged(total);
        }
    }
}
