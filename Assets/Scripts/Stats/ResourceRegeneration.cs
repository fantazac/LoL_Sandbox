public class ResourceRegeneration : Stat
{
    public ResourceRegeneration() : base() { }
    public ResourceRegeneration(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

    protected override void UpdateTotal()
    {
        total = (currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f)) * (1 - (percentMalus * 0.01f));
    }
}
