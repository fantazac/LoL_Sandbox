public class HealthRegeneration : Stat
{
    public HealthRegeneration() : base() { }
    public HealthRegeneration(float initialBaseValue) : base(initialBaseValue) { }
    public HealthRegeneration(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

    public override void UpdateTotal()
    {
        total = (currentBaseValue + flatBonus) * (1 + (percentBonus * 0.01f)) * (1 - (percentMalus * 0.01f));
    }
}
