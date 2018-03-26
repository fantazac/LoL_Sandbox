using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBaseStats : MonoBehaviour
{
    public float BaseHealth { get; protected set; }
    public float BaseResource { get; protected set; }//mana, energy, fury, ...

    public float BaseAttackDamage { get; protected set; }
    public float BaseAbilityPower { get; protected set; }
    public float BaseArmor { get; protected set; }
    public float BaseMagicResistance { get; protected set; }
    public float BaseAttackSpeed { get; protected set; }
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
}
