using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyStats : CharacterStats
{
    //extra stats the character has that other characters don't

    protected override void Awake()
    {
        base.Awake();
    }

    protected override EntityBaseStats SetEntityBaseStats()
    {
        return gameObject.AddComponent<DummyBaseStats>();
    }

    protected override void SetBaseStats(EntityBaseStats entityStats)
    {
        base.SetBaseStats(entityStats);

        //set extra character stats
    }
}
