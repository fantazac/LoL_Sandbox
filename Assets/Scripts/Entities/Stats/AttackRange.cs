public class AttackRange : Stat
{
    public override float GetTotal()
    {
        return base.GetTotal() * StaticObjects.MultiplyingFactor;
    }

    public override string GetUIText()
    {
        return "ATTACK RANGE: " + base.GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
