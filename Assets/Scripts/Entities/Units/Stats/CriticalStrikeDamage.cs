public class CriticalStrikeDamage : Stat
{
    public CriticalStrikeDamage() : base(2f) { }

    public override void UpdateTotal()
    {
        total = currentBaseValue + percentBonus * 0.01f;
    }
}
