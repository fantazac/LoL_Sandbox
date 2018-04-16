using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortuneStats : CharacterStats
{
    //extra stats the character has that other characters don't

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void SetResource()
    {
        Resource = gameObject.AddComponent<Mana>();
    }

    protected override void SetResourceRegeneration()
    {
        ResourceRegeneration = gameObject.AddComponent<ManaRegeneration>();
    }

    protected override EntityBaseStats GetEntityBaseStats()
    {
        return gameObject.AddComponent<MissFortuneBaseStats>();
    }

    protected override void SetBaseStats(EntityBaseStats entityStats)
    {
        base.SetBaseStats(entityStats);

        //set extra character stats
    }
}
