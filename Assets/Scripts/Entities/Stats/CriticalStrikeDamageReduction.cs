public class CriticalStrikeDamageReduction : Stat
{
    public CriticalStrikeDamageReduction() : base() { }

    public override void UpdateTotal()
    {
        total = currentBaseValue + percentBonus;
    }
}
