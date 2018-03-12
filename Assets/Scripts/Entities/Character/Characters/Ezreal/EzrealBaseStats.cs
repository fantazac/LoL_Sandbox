using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzrealBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected EzrealBaseStats()
    {
        BaseHealth = 484.4f;
        BaseResource = 360.6f;

        BaseAttackDamage = 64;
        BaseAbilityPower = 0;
        BaseArmor = 31;
        BaseMagicResistance = 30;
        BaseAttackSpeed = 0.625f;
        BaseCooldownReduction = 0;
        BaseCriticalStrikeChance = 0;
        BaseMovementSpeed = 325;

        BaseHealthRegeneration = 6.5f;
        BaseResourceRegeneration = 8.1f;
        BaseArmorPenetration = 0;
        BaseArmorPenetrationPercent = 0;
        BaseMagicPenetration = 0;
        BaseMagicPenetrationPercent = 0;
        BaseLifeSteal = 0;
        BaseSpellVamp = 0;
        BaseAttackRange = 550;
        BaseTenacity = 0;

        HealthPerLevel = 80;
        ResourcePerLevel = 42;

        AttackDamagePerLevel = 2.41f;
        AttackSpeedPerLevel = 1.5f;

        HealthRegenerationPerLevel = 0.55f;
        ResourceRegenerationPerLevel = 0.65f;
        ArmorPerLevel = 3.5f;
        MagicResistancePerLevel = 0.5f;
    }
}
