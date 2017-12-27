using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzrealBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected EzrealBaseStats()
    {
        BaseHealth = 450;
        BaseResource = 40;

        BaseAttackDamage = 350;
        BaseAbilityPower = 378;
        BaseArmor = 13;
        BaseMagicResistance = 73;
        BaseAttackSpeed = 0.771f;
        BaseCooldownReduction = 5;
        BaseCriticalStrikeChance = 23;
        BaseMovementSpeed = 335;

        BaseHealthRegeneration = 5;
        BaseResourceRegeneration = 9;
        BaseArmorPenetration = 15;
        BaseArmorPenetrationPercent = 15;
        BaseMagicPenetration = 23;
        BaseMagicPenetrationPercent = 44;
        BaseLifeSteal = 15;
        BaseSpellVamp = 25;
        BaseAttackRange = 525;
        BaseTenacity = 29;
    }
}
