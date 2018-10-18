public class CriticalStrikeDamageReduction : Stat
{
    public override void UpdateTotal()//TODO: Check if this stacks multiplicatively
    {
        total = percentBonus * 0.01f;
    }
}
