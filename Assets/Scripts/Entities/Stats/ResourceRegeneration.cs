public class ResourceRegeneration : Stat
{
    public ResourceRegeneration(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

    public override void UpdateTotal()
    {
        total = (currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f));
    }
}
