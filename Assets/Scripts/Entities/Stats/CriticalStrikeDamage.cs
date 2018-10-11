public class CriticalStrikeDamage : Stat
{
    public CriticalStrikeDamage(float initialBaseValue) : base(initialBaseValue) { }

    public override void UpdateTotal()
    {
        total = currentBaseValue + percentBonus;
    }
}
