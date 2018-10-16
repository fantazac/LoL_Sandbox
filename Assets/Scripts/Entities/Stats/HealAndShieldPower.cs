public class HealAndShieldPower : Stat
{
    public HealAndShieldPower() : base(1f) { }

    public override void UpdateTotal()
    {
        total = currentBaseValue + percentBonus;
    }
}
