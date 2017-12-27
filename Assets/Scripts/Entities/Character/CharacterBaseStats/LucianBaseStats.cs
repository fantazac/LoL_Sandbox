using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucianBaseStats : CharacterBaseStats
{
    //extra stats for each character

    protected LucianBaseStats()
    {
        BaseHealth = 500;
        BaseResource = 28;

        BaseAttackDamage = 420;
        BaseAbilityPower = 234;
        BaseArmor = 60;
        BaseMagicResistance = 40;
        BaseAttackSpeed = 1.325f;
        BaseCooldownReduction = 10;
        BaseCriticalStrikeChance = 42;
        BaseMovementSpeed = 325;

        BaseHealthRegeneration = 8;
        BaseResourceRegeneration = 11;
        BaseArmorPenetration = 9;
        BaseArmorPenetrationPercent = 32;
        BaseMagicPenetration = 14;
        BaseMagicPenetrationPercent = 30;
        BaseLifeSteal = 31;
        BaseSpellVamp = 17;
        BaseAttackRange = 550;
        BaseTenacity = 22;
    }
}
