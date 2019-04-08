public class AbilityPower : Stat
{
    protected override void UpdateTotal()
    {
        total = flatBonus * (1 + (percentBonus * 0.01f));
    }
}
