public abstract class CharacterBaseStats : BaseStats
{
    public float BaseResource { get; protected set; } //mana, energy, fury, ...
    public float BaseHealthRegeneration { get; protected set; }
    public float BaseResourceRegeneration { get; protected set; }

    public float HealthPerLevel { get; protected set; }
    public float ResourcePerLevel { get; protected set; }
    public float AttackDamagePerLevel { get; protected set; }
    public float ArmorPerLevel { get; protected set; }
    public float MagicResistancePerLevel { get; protected set; }
    public float AttackSpeedPerLevel { get; protected set; }
    public float HealthRegenerationPerLevel { get; protected set; }
    public float ResourceRegenerationPerLevel { get; protected set; }
}
