public class AttackRange : Stat
{
    private const float ATTACK_RANGE_MODIFIER = 0.01f;    // This is because our map is 100 times smaller than the real one.

    public override float GetTotal()
    {
        return base.GetTotal() * ATTACK_RANGE_MODIFIER;
    }

    public override string GetUIText()
    {
        return "ATTACK RANGE: " + base.GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
