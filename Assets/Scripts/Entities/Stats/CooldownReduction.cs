public class CooldownReduction : Stat
{
    public override void UpdateTotal()
    {
        total = baseValue + flatBonus - flatMalus;
    }

    public override string GetUIText()
    {
        return "COOLDOWN REDUCTION: " + GetTotal() + "% (" + GetBaseValue() + "% + " + GetFlatBonus() + "% - " + GetFlatMalus() + "%)";
    }
}
