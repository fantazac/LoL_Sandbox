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
    
    public HealthRegeneration HealthRegenaration { get; protected set; }
    public ResourceRegeneration ResourceRegeneration { get; protected set; }
    public ArmorPenetration ArmorPenetration { get; protected set; }
    public MagicPenetration MagicPenetration { get; protected set; }
    public LifeSteal LifeSteal { get; protected set; }
    public SpellVamp SpellVamp { get; protected set; }
    public AttackRange AttackRange { get; protected set; }
    public Tenacity Tenacity { get; protected set; }

    protected virtual void OnEnable()
    {
        Health = GetComponent<Health>();
        Resource = GetComponent<Resource>();//need to handle when there is no resource (ex. garen)

        AttackDamage = GetComponent<AttackDamage>();
        AbilityPower = GetComponent<AbilityPower>();
        Armor = GetComponent<Armor>();
        MagicResistance = GetComponent<MagicResistance>();
        AttackSpeed = GetComponent<AttackSpeed>();
        CooldownReduction = GetComponent<CooldownReduction>();
        CriticalStrikeChance = GetComponent<CriticalStrikeChance>();
        MovementSpeed = GetComponent<MovementSpeed>();

        HealthRegenaration = GetComponent<HealthRegeneration>();
        ResourceRegeneration = GetComponent<ResourceRegeneration>();
        ArmorPenetration = GetComponent<ArmorPenetration>();
        MagicPenetration = GetComponent<MagicPenetration>();
        LifeSteal = GetComponent<LifeSteal>();
        SpellVamp = GetComponent<SpellVamp>();
        AttackRange = GetComponent<AttackRange>();
        Tenacity = GetComponent<Tenacity>();

        SetBaseStats(GetComponent<EntityBaseStats>());
    }

    protected virtual void SetBaseStats(EntityBaseStats entityBaseStats)
    {
        Health.SetBaseHealth(entityBaseStats.BaseHealth);
        Resource.SetBaseResource(entityBaseStats.BaseResource);

        AttackDamage.SetBaseAttackDamage(entityBaseStats.BaseAttackDamage);
        AbilityPower.SetBaseAbilityPower(entityBaseStats.BaseAbilityPower);
        Armor.SetBaseArmor(entityBaseStats.BaseArmor);
        MagicResistance.SetBaseMagicResistance(entityBaseStats.BaseMagicResistance);
        AttackSpeed.SetBaseAttackSpeed(entityBaseStats.BaseAttackSpeed);
        CooldownReduction.SetBaseCooldownReduction(entityBaseStats.BaseCooldownReduction);
        CriticalStrikeChance.SetBaseCriticalStrikeChance(entityBaseStats.BaseCriticalStrikeChance);
        MovementSpeed.SetBaseMovementSpeed(entityBaseStats.BaseMovementSpeed);

        HealthRegenaration.SetBaseHealthRegeneration(entityBaseStats.BaseHealthRegeneration);
        ResourceRegeneration.SetBaseResourceRegeneration(entityBaseStats.BaseResourceRegeneration);
        ArmorPenetration.SetBaseArmorPenetration(entityBaseStats.BaseArmorPenetration, entityBaseStats.BaseArmorPenetrationPercent);
        MagicPenetration.SetBaseMagicPenetration(entityBaseStats.BaseMagicPenetration, entityBaseStats.BaseMagicPenetrationPercent);
        LifeSteal.SetBaseLifeSteal(entityBaseStats.BaseLifeSteal);
        SpellVamp.SetBaseSpellVamp(entityBaseStats.BaseSpellVamp);
        AttackRange.SetBaseAttackRange(entityBaseStats.BaseAttackRange);
        Tenacity.SetBaseTenacity(entityBaseStats.BaseTenacity);
    }
}
