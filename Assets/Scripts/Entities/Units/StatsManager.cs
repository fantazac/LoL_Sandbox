using UnityEngine;

public abstract class StatsManager : MonoBehaviour
{
    protected Unit unit;

    public Health Health { get; protected set; }
    public Resource Resource { get; protected set; }//mana, energy, ...

    public AttackDamage AttackDamage { get; protected set; }
    public AbilityPower AbilityPower { get; protected set; }
    public Resistance Armor { get; protected set; }
    public Resistance MagicResistance { get; protected set; }
    public AttackSpeed AttackSpeed { get; protected set; }
    public CooldownReduction CooldownReduction { get; protected set; }
    public CriticalStrikeChance CriticalStrikeChance { get; protected set; }
    public MovementSpeed MovementSpeed { get; protected set; }

    public ResourceRegeneration HealthRegeneration { get; protected set; }
    public ResourceRegeneration ResourceRegeneration { get; protected set; }
    public Lethality Lethality { get; protected set; }
    public ResistancePenetrationPercent ArmorPenetrationPercent { get; protected set; }
    public ResistancePenetrationFlat MagicPenetrationFlat { get; protected set; }
    public ResistancePenetrationPercent MagicPenetrationPercent { get; protected set; }
    public PercentBonusOnlyStat LifeSteal { get; protected set; }
    public PercentBonusOnlyStat SpellVamp { get; protected set; }
    public AttackRange AttackRange { get; protected set; }
    public Tenacity Tenacity { get; protected set; }

    public ResourceType ResourceType { get; protected set; }

    public CriticalStrikeDamage CriticalStrikeDamage { get; protected set; }
    public PercentBonusOnlyStat CriticalStrikeDamageReduction { get; protected set; }
    public DamageModifier PhysicalDamageModifier { get; protected set; }
    public DamageModifier MagicDamageModifier { get; protected set; }
    public DamageModifier PhysicalDamageReceivedModifier { get; protected set; }
    public DamageModifier MagicDamageReceivedModifier { get; protected set; }
    public PercentBonusOnlyStat HealAndShieldPower { get; protected set; }
    public SlowResistance SlowResistance { get; protected set; }

    public GrievousWounds GrievousWounds { get; protected set; }

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();

        InitializUnitStats(GetBaseStats());
    }

    protected virtual void InitializUnitStats(BaseStats baseStats)
    {
        Health = new Health(baseStats.BaseHealth);

        AttackDamage = new AttackDamage(baseStats.BaseAttackDamage);
        AbilityPower = new AbilityPower();
        Armor = new Resistance(baseStats.BaseArmor);
        MagicResistance = new Resistance(baseStats.BaseMagicResistance);
        AttackSpeed = new AttackSpeed(baseStats.BaseAttackSpeed);
        CooldownReduction = new CooldownReduction();
        CriticalStrikeChance = new CriticalStrikeChance();
        MovementSpeed = new MovementSpeed(baseStats.BaseMovementSpeed);

        HealthRegeneration = new ResourceRegeneration();
        Lethality = new Lethality();
        ArmorPenetrationPercent = new ResistancePenetrationPercent();
        MagicPenetrationFlat = new ResistancePenetrationFlat();
        MagicPenetrationPercent = new ResistancePenetrationPercent();
        LifeSteal = new PercentBonusOnlyStat();
        SpellVamp = new PercentBonusOnlyStat();
        AttackRange = new AttackRange(baseStats.BaseAttackRange);
        Tenacity = new Tenacity();

        CriticalStrikeDamage = new CriticalStrikeDamage();
        CriticalStrikeDamageReduction = new PercentBonusOnlyStat();
        PhysicalDamageModifier = new DamageModifier();
        MagicDamageModifier = new DamageModifier();
        PhysicalDamageReceivedModifier = new DamageModifier();
        MagicDamageReceivedModifier = new DamageModifier();
        HealAndShieldPower = new PercentBonusOnlyStat();
        SlowResistance = new SlowResistance();

        GrievousWounds = new GrievousWounds();

        ExtraAdjustments();
    }

    protected abstract BaseStats GetBaseStats();

    protected void ExtraAdjustments()
    {
        AttackSpeed.SetBasicAttack(GetComponent<BasicAttack>());
        MovementSpeed.SubscribeToSlowResistanceChangedEvent(SlowResistance);
    }

    public void ReduceHealth(DamageSource damageSource, DamageType damageType, float damage)
    {
        if (damage < 0)
        {
            RestoreHealth(damage);
        }
        else if (damage > 0)
        {
            float remainingDamage = damage;
            if (damageType == DamageType.MAGIC)
            {
                remainingDamage = unit.ShieldManager.DamageShield(ShieldType.MAGIC, remainingDamage);
            }
            else if (damageType == DamageType.PHYSICAL)
            {
                remainingDamage = unit.ShieldManager.DamageShield(ShieldType.PHYSICAL, remainingDamage);
            }
            else
            {
                remainingDamage = unit.ShieldManager.DamageShield(ShieldType.NORMAL, remainingDamage);
            }

            if (remainingDamage > 0)
            {
                bool wasAliveBeforeTakingDamage = !Health.IsDead();
                Health.Reduce(remainingDamage);
                if (Health.IsDead() && wasAliveBeforeTakingDamage)
                {
                    damageSource.KilledUnit(unit);
                }
            }
        }
    }

    public void RestoreHealth(float heal)
    {
        Health.Restore(heal * GrievousWounds.GetTotal());
    }
}
