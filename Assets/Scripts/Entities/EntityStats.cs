using UnityEngine;

public abstract class EntityStats : MonoBehaviour
{
    public Health Health { get; protected set; }
    public Resource Resource { get; protected set; }//mana, energy, fury, ...

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
    public LifeSteal LifeSteal { get; protected set; }
    public SpellVamp SpellVamp { get; protected set; }
    public AttackRange AttackRange { get; protected set; }
    public Tenacity Tenacity { get; protected set; }

    public ResourceType ResourceType { get; protected set; }

    public CriticalStrikeDamage CriticalStrikeDamage { get; protected set; }
    public CriticalStrikeDamageReduction CriticalStrikeDamageReduction { get; protected set; }
    public DamageModifier PhysicalDamageModifier { get; protected set; }
    public DamageModifier MagicDamageModifier { get; protected set; }
    public DamageModifier PhysicalDamageReceivedModifier { get; protected set; }
    public DamageModifier MagicDamageReceivedModifier { get; protected set; }
    public HealAndShieldPower HealAndShieldPower { get; protected set; }
    public SlowResistance SlowResistance { get; protected set; }

    protected virtual void Awake()
    {
        InitializeEntityStats(GetEntityBaseStats());
    }

    protected virtual void InitializeEntityStats(EntityBaseStats entityBaseStats)
    {
        Health = new Health(entityBaseStats.BaseHealth);

        AttackDamage = new AttackDamage(entityBaseStats.BaseAttackDamage);
        AbilityPower = new AbilityPower();
        Armor = new Resistance(entityBaseStats.BaseArmor);
        MagicResistance = new Resistance(entityBaseStats.BaseMagicResistance);
        AttackSpeed = new AttackSpeed(entityBaseStats.BaseAttackSpeed);
        CooldownReduction = new CooldownReduction();
        CriticalStrikeChance = new CriticalStrikeChance();
        MovementSpeed = new MovementSpeed(entityBaseStats.BaseMovementSpeed);

        HealthRegeneration = new ResourceRegeneration();
        Lethality = new Lethality();
        ArmorPenetrationPercent = new ResistancePenetrationPercent();
        MagicPenetrationFlat = new ResistancePenetrationFlat();
        MagicPenetrationPercent = new ResistancePenetrationPercent();
        LifeSteal = new LifeSteal();
        SpellVamp = new SpellVamp();
        AttackRange = new AttackRange(entityBaseStats.BaseAttackRange);
        Tenacity = new Tenacity();

        CriticalStrikeDamage = new CriticalStrikeDamage();
        CriticalStrikeDamageReduction = new CriticalStrikeDamageReduction();
        PhysicalDamageModifier = new DamageModifier();
        MagicDamageModifier = new DamageModifier();
        PhysicalDamageReceivedModifier = new DamageModifier();
        MagicDamageReceivedModifier = new DamageModifier();
        HealAndShieldPower = new HealAndShieldPower();
        SlowResistance = new SlowResistance();

        ExtraAdjustments();
    }

    protected abstract EntityBaseStats GetEntityBaseStats();

    protected void ExtraAdjustments()
    {
        AttackSpeed.SetEntityBasicAttack(GetComponent<EntityBasicAttack>());
        MovementSpeed.SubscribeToSlowResistanceChangedEvent(SlowResistance);
    }
}
