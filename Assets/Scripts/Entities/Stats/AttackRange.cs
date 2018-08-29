public class AttackRange : Stat
{
    public override float GetTotal()
    {
        return base.GetTotal() * StaticObjects.MultiplyingFactor;
    }

    protected override string GetSimpleUIText()
    {
        return "ATTACK RANGE: " + base.GetTotal();
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
