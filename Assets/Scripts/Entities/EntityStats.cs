using UnityEngine;
using System.Collections;

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

    protected virtual void Awake()
    {
        EntityBaseStats entityBaseStats = GetEntityBaseStats();

        Health = gameObject.AddComponent<Health>();
        SetResource();

        AttackDamage = gameObject.AddComponent<AttackDamage>();
        AbilityPower = gameObject.AddComponent<AbilityPower>();
        Armor = gameObject.AddComponent<Armor>();
        MagicResistance = gameObject.AddComponent<MagicResistance>();
        AttackSpeed = gameObject.AddComponent<AttackSpeed>();
        CooldownReduction = gameObject.AddComponent<CooldownReduction>();
        CriticalStrikeChance = gameObject.AddComponent<CriticalStrikeChance>();//TODO
        MovementSpeed = gameObject.AddComponent<MovementSpeed>();

        HealthRegeneration = gameObject.AddComponent<HealthRegeneration>();
        SetResourceRegeneration();
        Lethality = gameObject.AddComponent<Lethality>();
        ArmorPenetrationPercent = gameObject.AddComponent<ArmorPenetrationPercent>();
        MagicPenetrationFlat = gameObject.AddComponent<MagicPenetrationFlat>();
        MagicPenetrationPercent = gameObject.AddComponent<MagicPenetrationPercent>();
        LifeSteal = gameObject.AddComponent<LifeSteal>();//TODO
        SpellVamp = gameObject.AddComponent<SpellVamp>();//TODO
        AttackRange = gameObject.AddComponent<AttackRange>();
        Tenacity = gameObject.AddComponent<Tenacity>();//TODO

        ExtraAdjustments();

        SetBaseStats(entityBaseStats);
    }

    protected virtual EntityBaseStats GetEntityBaseStats()
    {
        return gameObject.AddComponent<EntityBaseStats>();
    }

    protected void ExtraAdjustments()
    {
        AttackSpeed.SetEntityBasicAttack();
    }

    protected virtual void SetBaseStats(EntityBaseStats entityBaseStats)
    {
        if (Resource)
        {
            Resource.SetBaseValue(entityBaseStats.BaseResource);
        }
        if (ResourceRegeneration)
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

    protected virtual void SetResource() { }
    protected virtual void SetResourceRegeneration() { }
}
