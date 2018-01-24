public class AttackDamage : Stat
{
    public override string GetUIText()
    {
        return "ATTACK DAMAGE: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
