public abstract class MultiplicativeBonusStat : Stat
{
    protected float multiplicativePercentBonus;

    public MultiplicativeBonusStat(float initialBaseValue) : base(initialBaseValue) { }
    public MultiplicativeBonusStat(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

    public float GetMultiplicativePercentBonus()
    {
        return multiplicativePercentBonus;
    }

    protected void AddMultiplicativePercentBonus(float multiplicativePercentBonus)
    {
        this.multiplicativePercentBonus += multiplicativePercentBonus;
        UpdateTotal();
    }

    protected void RemoveMultiplicativePercentBonus(float multiplicativePercentBonus)
    {
        this.multiplicativePercentBonus -= multiplicativePercentBonus;
        UpdateTotal();
    }
}
