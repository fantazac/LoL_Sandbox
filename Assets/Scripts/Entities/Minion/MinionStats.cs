using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionStats : EntityStats
{
    //extra stats minions have that other entities don't

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void SetBaseStats(EntityBaseStats entityStats)
    {
        base.SetBaseStats(entityStats);

        //set extra minion stats
    }
}