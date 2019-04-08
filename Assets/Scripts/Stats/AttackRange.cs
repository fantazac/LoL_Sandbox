public class AttackRange : Stat
{
    public AttackRange(float initialBaseValue) : base(initialBaseValue) { }

    protected override void UpdateTotal()
    {
        total = (currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f)) * 0.01f;
    }
}
