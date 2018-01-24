public class AttackRange : Stat
{
    public override string GetUIText()
    {
        return "ATTACK RANGE: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
