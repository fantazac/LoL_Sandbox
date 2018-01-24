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
        Resource = GetComponent<Resource>();//TODO: need to handle when there is no resource (ex. garen)

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
        Health.SetBaseValue(entityBaseStats.BaseHealth);
        Resource.SetBaseValue(entityBaseStats.BaseResource);

        AttackDamage.SetBaseValue(entityBaseStats.BaseAttackDamage);
        AbilityPower.SetBaseValue(entityBaseStats.BaseAbilityPower);
        Armor.SetBaseValue(entityBaseStats.BaseArmor);
        MagicResistance.SetBaseValue(entityBaseStats.BaseMagicResistance);
        AttackSpeed.SetBaseValue(entityBaseStats.BaseAttackSpeed);
        CooldownReduction.SetBaseValue(entityBaseStats.BaseCooldownReduction);
        CriticalStrikeChance.SetBaseValue(entityBaseStats.BaseCriticalStrikeChance);
        MovementSpeed.SetBaseValue(entityBaseStats.BaseMovementSpeed);

        HealthRegenaration.SetBaseValue(entityBaseStats.BaseHealthRegeneration);
        ResourceRegeneration.SetBaseValue(entityBaseStats.BaseResourceRegeneration);
        ArmorPenetration.SetBaseValue(entityBaseStats.BaseArmorPenetration);
        MagicPenetration.SetBaseValue(entityBaseStats.BaseMagicPenetration);
        LifeSteal.SetBaseValue(entityBaseStats.BaseLifeSteal);
        SpellVamp.SetBaseValue(entityBaseStats.BaseSpellVamp);
        AttackRange.SetBaseValue(entityBaseStats.BaseAttackRange);
        Tenacity.SetBaseValue(entityBaseStats.BaseTenacity);
    }
}
