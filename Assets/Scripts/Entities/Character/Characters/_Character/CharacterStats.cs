using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : EntityStats
{
    //extra stats characters have that other entities don't

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void SetBaseStats(EntityBaseStats entityBaseStats)
    {
        base.SetBaseStats(entityBaseStats);

        CharacterBaseStats characterBaseStats = (CharacterBaseStats)entityBaseStats;

        //set extra character stats
        Health.SetPerLevelValue(characterBaseStats.HealthPerLevel);
        Resource.SetPerLevelValue(characterBaseStats.ResourcePerLevel);

        AttackDamage.SetPerLevelValue(characterBaseStats.AttackDamagePerLevel);
        Armor.SetPerLevelValue(characterBaseStats.ArmorPerLevel);
        MagicResistance.SetPerLevelValue(characterBaseStats.MagicResistancePerLevel);
        AttackSpeed.SetPerLevelValue(characterBaseStats.AttackSpeedPerLevel);

        HealthRegeneration.SetPerLevelValue(characterBaseStats.HealthRegenerationPerLevel);
        ResourceRegeneration.SetPerLevelValue(characterBaseStats.ResourceRegenerationPerLevel);
    }
}