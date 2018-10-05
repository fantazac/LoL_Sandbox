using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterStats : EntityStats
{
    //extra stats monsters have that other entities don't

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitializeEntityStats(EntityBaseStats entityBaseStats)
    {
        base.InitializeEntityStats(entityBaseStats);

        //set extra monster stats
    }
}