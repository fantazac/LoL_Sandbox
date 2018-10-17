public abstract class EntityBaseStats
{
    public float BaseHealth { get; protected set; }
    public float BaseAttackDamage { get; protected set; }
    public float BaseArmor { get; protected set; }
    public float BaseMagicResistance { get; protected set; }
    public float BaseAttackSpeed { get; protected set; }
    public float AttackDelay { get; protected set; }
    public float BaseMovementSpeed { get; protected set; }
    public float BaseAttackRange { get; protected set; }

    public EntityBaseStats()
    {
        SetBaseStats();
    }

    protected abstract void SetBaseStats();
}
