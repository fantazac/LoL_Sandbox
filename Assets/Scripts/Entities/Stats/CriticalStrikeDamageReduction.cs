public class CriticalStrikeDamageReduction : Stat
{
    public CriticalStrikeDamageReduction() : base() { }

    public override void UpdateTotal()//TODO: Check if this stacks multiplicatively
    {
        total = percentBonus * 0.01f;
    }
}
