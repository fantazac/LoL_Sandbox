public class AbilityPower : Stat
{
    public AbilityPower() : base() { }

    public override void UpdateTotal()
    {
        total = (currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f));
    }
}
