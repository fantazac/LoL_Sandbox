public class EntityBaseStats
{
    public float BaseHealth { get; protected set; }
    public float BaseResource { get; protected set; }//mana, energy, fury, ...

    public float BaseAttackDamage { get; protected set; }
    public float BaseAbilityPower { get; protected set; }
    public float BaseArmor { get; protected set; }
    public float BaseMagicResistance { get; protected set; }
    public float BaseAttackSpeed { get; protected set; }
    public float AttackDelay { get; protected set; }
    public float BaseCooldownReduction { get; protected set; }
    public float BaseCriticalStrikeChance { get; protected set; }
    public float BaseMovementSpeed { get; protected set; }

    public float BaseHealthRegeneration { get; protected set; }
    public float BaseResourceRegeneration { get; protected set; }
    public float BaseLethality { get; protected set; }
    public float BaseArmorPenetrationPercent { get; protected set; }
    public float BaseMagicPenetrationFlat { get; protected set; }
    public float BaseMagicPenetrationPercent { get; protected set; }
    public float BaseLifeSteal { get; protected set; }
    public float BaseSpellVamp { get; protected set; }
    public float BaseAttackRange { get; protected set; }
    public float BaseTenacity { get; protected set; }

    public float BaseCriticalStrikeDamage { get; protected set; }

    public EntityBaseStats()
    {
        SetBaseStats();
    }

    protected virtual void SetBaseStats()
    {
        BaseAttackSpeed = 0.625f;
        AttackDelay = 0;
        BaseMovementSpeed = 325;

        BaseAttackRange = 125;
    }
}
