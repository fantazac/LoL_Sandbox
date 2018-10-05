public abstract class CharacterBaseStats : EntityBaseStats
{
    //extra stats that characters have and other entities don't
    public float HealthPerLevel { get; protected set; }
    public float ResourcePerLevel { get; protected set; }

    public float AttackDamagePerLevel { get; protected set; }
    public float ArmorPerLevel { get; protected set; }
    public float MagicResistancePerLevel { get; protected set; }
    public float AttackSpeedPerLevel { get; protected set; }

    public float HealthRegenerationPerLevel { get; protected set; }
    public float ResourceRegenerationPerLevel { get; protected set; }
}
