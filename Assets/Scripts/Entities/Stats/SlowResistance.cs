public class SlowResistance : Stat
{
    public delegate void OnSlowResistanceChangedHandler(float slowResistance);
    public event OnSlowResistanceChangedHandler OnSlowResistanceChanged;

    public override void UpdateTotal()
    {
        total = percentBonus * 0.01f;
        if (OnSlowResistanceChanged != null)
        {
            OnSlowResistanceChanged(total);
        }
    }
}
