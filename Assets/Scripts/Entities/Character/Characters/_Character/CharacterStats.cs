using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : EntityStats
{
    //extra stats characters have that other entities don't

    protected override EntityBaseStats GetEntityBaseStats()
    {
        return gameObject.AddComponent<CharacterBaseStats>();
    }

    protected override void SetBaseStats(EntityBaseStats entityBaseStats)
    {
        base.SetBaseStats(entityBaseStats);

        CharacterBaseStats characterBaseStats = (CharacterBaseStats)entityBaseStats;

        //set extra character stats

        if (Resource)
        {
            Resource.SetPerLevelValue(characterBaseStats.ResourcePerLevel);
        }
        if (ResourceRegeneration)
        {
            ResourceRegeneration.SetPerLevelValue(characterBaseStats.ResourceRegenerationPerLevel);
        }

        Health.SetPerLevelValue(characterBaseStats.HealthPerLevel);

        AttackDamage.SetPerLevelValue(characterBaseStats.AttackDamagePerLevel);
        Armor.SetPerLevelValue(characterBaseStats.ArmorPerLevel);
        MagicResistance.SetPerLevelValue(characterBaseStats.MagicResistancePerLevel);
        AttackSpeed.SetPerLevelValue(characterBaseStats.AttackSpeedPerLevel);

        HealthRegeneration.SetPerLevelValue(characterBaseStats.HealthRegenerationPerLevel);
    }
}