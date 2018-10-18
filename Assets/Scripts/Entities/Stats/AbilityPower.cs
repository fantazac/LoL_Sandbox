public class AbilityPower : Stat
{
    public override void UpdateTotal()
    {
        total = flatBonus * (1 + (percentBonus * 0.01f));
    }
}
