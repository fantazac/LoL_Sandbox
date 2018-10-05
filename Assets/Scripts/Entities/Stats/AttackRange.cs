public class AttackRange : Stat
{
    public AttackRange(float initialBaseValue) : base(initialBaseValue) { }

    public override void UpdateTotal()
    {
        total = (currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f));
    }

    public override float GetTotal()
    {
        return base.GetTotal() * StaticObjects.MultiplyingFactor;
    }
}
