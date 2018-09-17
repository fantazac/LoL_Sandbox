using UnityEngine;

public class EntityStats : MonoBehaviour
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
        EntityBaseStats entityBaseStats = SetEntityBaseStats();

        Health = new Health();
        Resource = new Resource(ResourceType);

        AttackDamage = new AttackDamage();
        AbilityPower = new AbilityPower();
        Armor = new Armor();
        MagicResistance = new MagicResistance();
        AttackSpeed = new AttackSpeed();
        CooldownReduction = new CooldownReduction();
        CriticalStrikeChance = new CriticalStrikeChance();
        MovementSpeed = new MovementSpeed();

        HealthRegeneration = new HealthRegeneration();
        ResourceRegeneration = new ResourceRegeneration(ResourceType);
        Lethality = new Lethality();
        ArmorPenetrationPercent = new ArmorPenetrationPercent();
        MagicPenetrationFlat = new MagicPenetrationFlat();
        MagicPenetrationPercent = new MagicPenetrationPercent();
        LifeSteal = new LifeSteal();
        SpellVamp = new SpellVamp();
        AttackRange = new AttackRange();
        Tenacity = new Tenacity();//TODO

        ExtraAdjustments();

        SetBaseStats(entityBaseStats);
    }

    protected virtual EntityBaseStats SetEntityBaseStats()
    {
        return gameObject.AddComponent<EntityBaseStats>();
    }

    protected void ExtraAdjustments()
    {
        AttackSpeed.SetEntityBasicAttack(GetComponent<EntityBasicAttack>());
    }

    protected virtual void SetBaseStats(EntityBaseStats entityBaseStats)
    {
        if (Resource != null)
        {
            Resource.SetBaseValue(entityBaseStats.BaseResource);
        }
        if (ResourceRegeneration != null)
        {
            ResourceRegeneration.SetBaseValue(entityBaseStats.BaseResourceRegeneration);
        }

        Health.SetBaseValue(entityBaseStats.BaseHealth);

        AttackDamage.SetBaseValue(entityBaseStats.BaseAttackDamage);
        AbilityPower.SetBaseValue(entityBaseStats.BaseAbilityPower);
        Armor.SetBaseValue(entityBaseStats.BaseArmor);
        MagicResistance.SetBaseValue(entityBaseStats.BaseMagicResistance);
        AttackSpeed.SetBaseValue(entityBaseStats.BaseAttackSpeed);
        CooldownReduction.SetBaseValue(entityBaseStats.BaseCooldownReduction);
        CriticalStrikeChance.SetBaseValue(entityBaseStats.BaseCriticalStrikeChance);
        MovementSpeed.SetBaseValue(entityBaseStats.BaseMovementSpeed);

        HealthRegeneration.SetBaseValue(entityBaseStats.BaseHealthRegeneration);
        Lethality.SetBaseValue(entityBaseStats.BaseLethality);
        ArmorPenetrationPercent.SetBaseValue(entityBaseStats.BaseArmorPenetrationPercent);
        MagicPenetrationFlat.SetBaseValue(entityBaseStats.BaseMagicPenetrationFlat);
        MagicPenetrationPercent.SetBaseValue(entityBaseStats.BaseMagicPenetrationPercent);
        LifeSteal.SetBaseValue(entityBaseStats.BaseLifeSteal);
        SpellVamp.SetBaseValue(entityBaseStats.BaseSpellVamp);
        AttackRange.SetBaseValue(entityBaseStats.BaseAttackRange);
        Tenacity.SetBaseValue(entityBaseStats.BaseTenacity);
    }
}
