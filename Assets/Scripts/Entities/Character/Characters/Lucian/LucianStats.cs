using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucianStats : CharacterStats
{
    //extra stats the character has that other characters don't

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void SetResource()
    {
        Resource = gameObject.AddComponent<Mana>();
        ResourceRegeneration = gameObject.AddComponent<ManaRegeneration>();
    }

    protected override EntityBaseStats GetEntityBaseStats()
    {
        return gameObject.AddComponent<LucianBaseStats>();
    }

    protected override void SetBaseStats(EntityBaseStats entityStats)
    {
        base.SetBaseStats(entityStats);

        //set extra character stats
    }
}
