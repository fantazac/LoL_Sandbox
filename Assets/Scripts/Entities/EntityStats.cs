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
        Health = GetComponent<Health>();
        Resource = GetComponent<Resource>();//supposed to work when no resource is used so check to confirm (ex. garen), also check energy (ex. zed)

        AttackDamage = GetComponent<AttackDamage>();
        AbilityPower = GetComponent<AbilityPower>();
        Armor = GetComponent<Armor>();
        MagicResistance = GetComponent<MagicResistance>();
        AttackSpeed = GetComponent<AttackSpeed>();
        CooldownReduction = GetComponent<CooldownReduction>();
        CriticalStrikeChance = GetComponent<CriticalStrikeChance>();//TODO
        MovementSpeed = GetComponent<MovementSpeed>();

        HealthRegeneration = GetComponent<HealthRegeneration>();
        ResourceRegeneration = GetComponent<ResourceRegeneration>();
        Lethality = GetComponent<Lethality>();
        ArmorPenetrationPercent = GetComponent<ArmorPenetrationPercent>();
        MagicPenetrationFlat = GetComponent<MagicPenetrationFlat>();
        MagicPenetrationPercent = GetComponent<MagicPenetrationPercent>();
        LifeSteal = GetComponent<LifeSteal>();//TODO
        SpellVamp = GetComponent<SpellVamp>();//TODO
        AttackRange = GetComponent<AttackRange>();
        Tenacity = GetComponent<Tenacity>();//TODO

        ExtraAdjustments();

        SetBaseStats(GetComponent<EntityBaseStats>());
    }

    protected void ExtraAdjustments()
    {
        AttackSpeed.SetEntityBasicAttack();
    }

    protected virtual void SetBaseStats(EntityBaseStats entityBaseStats)
    {
        Health.SetBaseValue(entityBaseStats.BaseHealth);
        if (Resource)
        {
            Resource.SetBaseValue(entityBaseStats.BaseResource);
        }

        AttackDamage.SetBaseValue(entityBaseStats.BaseAttackDamage);
        AbilityPower.SetBaseValue(entityBaseStats.BaseAbilityPower);
        Armor.SetBaseValue(entityBaseStats.BaseArmor);
        MagicResistance.SetBaseValue(entityBaseStats.BaseMagicResistance);
        AttackSpeed.SetBaseValue(entityBaseStats.BaseAttackSpeed);
        CooldownReduction.SetBaseValue(entityBaseStats.BaseCooldownReduction);
        CriticalStrikeChance.SetBaseValue(entityBaseStats.BaseCriticalStrikeChance);
        MovementSpeed.SetBaseValue(entityBaseStats.BaseMovementSpeed);

        HealthRegeneration.SetBaseValue(entityBaseStats.BaseHealthRegeneration);
        if (ResourceRegeneration)
        {
            ResourceRegeneration.SetBaseValue(entityBaseStats.BaseResourceRegeneration);
        }
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
