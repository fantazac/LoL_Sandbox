public class MovementSpeed : Stat
{
    private float lowCap = 220;
    private float firstCap = 415;
    private float secondCap = 490;

    public override float GetTotal()
    {
        return base.GetTotal() * StaticObjects.MultiplyingFactor;
    }

    public override void UpdateTotal()
    {
        base.UpdateTotal();

        if (total > secondCap)
        {
            total = (total * 0.5f) + 230;
        }
        else if (total > firstCap)
        {
            total = (total * 0.8f) + 83;
        }
        else if (total < lowCap)
        {
            total = (total * 0.5f) + 110;
        }
    }

    protected override string GetSimpleUIText()
    {
        return "MOVEMENT SPEED: " + base.GetTotal();
    }

    protected override string GetUIText()
    {
        return GetSimpleUIText() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}
