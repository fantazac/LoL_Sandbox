﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortuneBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected override void SetBaseStats()
    {
        BaseHealth = 541;
        BaseResource = 325.84f;

        BaseAttackDamage = 50;
        BaseAbilityPower = 0;
        BaseArmor = 28;
        BaseMagicResistance = 30;
        BaseAttackSpeed = 0.656f;
        BaseCooldownReduction = 0;
        BaseCriticalStrikeChance = 0;
        BaseMovementSpeed = 325;

        BaseHealthRegeneration = 3.75f;
        BaseResourceRegeneration = 8;
        BaseLethality = 0;
        BaseArmorPenetrationPercent = 0;
        BaseMagicPenetrationFlat = 0;
        BaseMagicPenetrationPercent = 0;
        BaseLifeSteal = 0;
        BaseSpellVamp = 0;
        BaseAttackRange = 550;
        BaseTenacity = 0;

        HealthPerLevel = 91;
        ResourcePerLevel = 35;

        AttackDamagePerLevel = 2.7f;
        AttackSpeedPerLevel = 3;

        HealthRegenerationPerLevel = 0.65f;
        ResourceRegenerationPerLevel = 0.65f;
        ArmorPerLevel = 3;
        MagicResistancePerLevel = 0.5f;
    }
}
