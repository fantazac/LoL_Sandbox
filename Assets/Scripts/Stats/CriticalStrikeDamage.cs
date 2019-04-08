public class CriticalStrikeDamage : Stat
{
    public CriticalStrikeDamage() : base(2) { }

    protected override void UpdateTotal()
    {
        total = currentBaseValue + percentBonus * 0.01f;
    }
}
