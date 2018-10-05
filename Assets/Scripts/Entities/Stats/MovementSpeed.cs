public class MovementSpeed : Stat
{
    private const float LOW_CAP = 220;
    private const float FIRST_CAP = 415;
    private const float SECOND_CAP = 490;

    private float multiplicativePercentBonus;

    //TODO: Requires a list of slows and applies only the biggest slow value

    public MovementSpeed(float initialBaseValue) : base(initialBaseValue) { }

    public override float GetTotal()
    {
        return base.GetTotal() * StaticObjects.MultiplyingFactor;
    }

    public override void UpdateTotal()
    {
        total = (currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f)) * (1 - (GetBiggestSlow() * 0.01f)) * (1 + (multiplicativePercentBonus * 0.01f));

        if (total > SECOND_CAP)
        {
            total = (total * 0.5f) + 230;
        }
        else if (total > FIRST_CAP)
        {
            total = (total * 0.8f) + 83;
        }
        else if (total < LOW_CAP)
        {
            total = (total * 0.5f) + 110;
        }
    }

    public float GetBiggestSlow()
    {
        return 0;
    }

    public float GetMultiplicativePercentBonus()
    {
        return multiplicativePercentBonus;
    }

    private void AddMultiplicativePercentBonus(float multiplicativePercentBonus)
    {
        this.multiplicativePercentBonus += multiplicativePercentBonus;
        UpdateTotal();
    }

    private void RemoveMultiplicativePercentBonus(float multiplicativePercentBonus)
    {
        this.multiplicativePercentBonus -= multiplicativePercentBonus;
        UpdateTotal();
    }
}
