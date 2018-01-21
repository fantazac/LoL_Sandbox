using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterStats : EntityStats
{
    //extra stats monsters have that other entities don't

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void SetBaseStats(EntityBaseStats entityStats)
    {
        base.SetBaseStats(entityStats);

        //set extra monster stats
    }
}