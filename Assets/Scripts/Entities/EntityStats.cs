using UnityEngine;

public abstract class EntityStats : MonoBehaviour
{
    public Health Health { get; protected set; }
    public Resource Resource { get; protected set; }//mana, energy, fury, ...

    public AttackDamage AttackDamage { get; protected set; }
    public AbilityPower AbilityPower { get; protected set; }
    public Armor Armor { get; protected set; }
    public MagicResistance MagicResistance { get; protected set; }
    public AttackSpeed AttackSpeed { get; protected set; }
    public CooldownReduction CooldownReduction { get; protected set; }
    public CriticalStrikeChance CriticalStrikeChance { get; protected set; }
    public MovementSpeed MovementSpeed { get; protected set; }

    public HealthRegeneration HealthRegeneration { get; protected set; }
    public ResourceRegeneration ResourceRegeneration { get; protected set; }
    public Lethality Lethality { get; protected set; }
    public ArmorPenetrationPercent ArmorPenetrationPercent { get; protected set; }
    public MagicPenetrationFlat MagicPenetrationFlat { get; protected set; }
    public MagicPenetrationPercent MagicPenetrationPercent { get; protected set; }
    public LifeSteal LifeSteal { get; protected set; }
    public SpellVamp SpellVamp { get; protected set; }
    public AttackRange AttackRange { get; protected set; }
    public Tenacity Tenacity { get; protected set; }

    public ResourceType ResourceType { get; protected set; }

    protected virtual void Awake()
    {
        InitializeEntityStats(GetEntityBaseStats());
    }

    protected virtual void InitializeEntityStats(EntityBaseStats entityBaseStats)
    {
        Health = new Health(entityBaseStats.BaseHealth);

        AttackDamage = new AttackDamage(entityBaseStats.BaseAttackDamage);
        AbilityPower = new AbilityPower();
        Armor = new Armor(entityBaseStats.BaseArmor);
        MagicResistance = new MagicResistance(entityBaseStats.BaseMagicResistance);
        AttackSpeed = new AttackSpeed(entityBaseStats.BaseAttackSpeed);
        CooldownReduction = new CooldownReduction();
        CriticalStrikeChance = new CriticalStrikeChance();
        MovementSpeed = new MovementSpeed(entityBaseStats.BaseMovementSpeed);

        HealthRegeneration = new HealthRegeneration(entityBaseStats.BaseHealthRegeneration);
        Lethality = new Lethality();
        ArmorPenetrationPercent = new ArmorPenetrationPercent();
        MagicPenetrationFlat = new MagicPenetrationFlat();
        MagicPenetrationPercent = new MagicPenetrationPercent();
        LifeSteal = new LifeSteal();
        SpellVamp = new SpellVamp();
        AttackRange = new AttackRange(entityBaseStats.BaseAttackRange);
        Tenacity = new Tenacity();

        ExtraAdjustments();
    }

    protected abstract EntityBaseStats GetEntityBaseStats();

    protected void ExtraAdjustments()
    {
        AttackSpeed.SetEntityBasicAttack(GetComponent<EntityBasicAttack>());
    }
}
