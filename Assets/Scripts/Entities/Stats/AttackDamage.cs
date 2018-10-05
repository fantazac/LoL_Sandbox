public class AttackDamage : Stat
{
    public AttackDamage(float initialBaseValue) : base(initialBaseValue) { }
    public AttackDamage(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

    public override void UpdateTotal()
    {
        total = (currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f)) - flatMalus;
    }
}
