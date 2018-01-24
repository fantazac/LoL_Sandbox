public class AttackSpeed : Stat
{
    public override string GetUIText()
    {
        return "ATTACK SPEED: " + GetTotal().ToString("0.00") + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
