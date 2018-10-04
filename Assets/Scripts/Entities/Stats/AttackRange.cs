public class AttackRange : Stat
{
    public override float GetTotal()
    {
        return base.GetTotal() * StaticObjects.MultiplyingFactor;
    }
}
