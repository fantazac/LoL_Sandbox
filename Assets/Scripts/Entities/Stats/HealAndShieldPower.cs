public class HealAndShieldPower : Stat
{
    public HealAndShieldPower() : base() { }

    public override void UpdateTotal()
    {
        total = percentBonus * 0.01f;
    }
}
