public class CooldownReduction : Stat
{
    public override string GetUIText()
    {
        return "COOLDOWN REDUCTION: " + GetTotal() + "% ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
