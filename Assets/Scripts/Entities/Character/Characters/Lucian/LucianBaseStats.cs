using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucianBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected LucianBaseStats()
    {
        BaseHealth = 554.4f;
        BaseResource = 348.9f;

        BaseAttackDamage = 65;
        BaseAbilityPower = 0;
        BaseArmor = 33;
        BaseMagicResistance = 30;
        BaseAttackSpeed = 2.5f;
        BaseCooldownReduction = 0;
        BaseCriticalStrikeChance = 0;
        BaseMovementSpeed = 335;

        BaseHealthRegeneration = 6;
        BaseResourceRegeneration = 8.2f;
        BaseArmorPenetration = 0;
        BaseArmorPenetrationPercent = 0;
        BaseMagicPenetration = 0;
        BaseMagicPenetrationPercent = 0;
        BaseLifeSteal = 0;
        BaseSpellVamp = 0;
        BaseAttackRange = 500;
        BaseTenacity = 0;
    }
}
