using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : EntityStats 
{
    //extra stats characters have that other entities don't

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void SetBaseStats(EntityBaseStats entityStats)
    {
        base.SetBaseStats(entityStats);

        //set extra character stats
    }
}