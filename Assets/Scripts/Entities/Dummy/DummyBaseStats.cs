using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected DummyBaseStats()
    {
        BaseHealth = 10000;
        BaseResource = 0;

        BaseAttackDamage = 0;
        BaseAbilityPower = 0;
        BaseArmor = 100;
        BaseMagicResistance = 100;
        BaseAttackSpeed = 0.658f;
        BaseCooldownReduction = 0;
        BaseCriticalStrikeChance = 0;
        BaseMovementSpeed = 325;

        BaseHealthRegeneration = 0;
        BaseResourceRegeneration = 0;
        BaseLethality = 0;
        BaseArmorPenetrationPercent = 0;
        BaseMagicPenetrationFlat = 0;
        BaseMagicPenetrationPercent = 0;
        BaseLifeSteal = 0;
        BaseSpellVamp = 0;
        BaseAttackRange = 125;
        BaseTenacity = 0;
    }
}
