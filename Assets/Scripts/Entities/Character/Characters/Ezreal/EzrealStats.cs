using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EzrealStats : CharacterStats
{
    //extra stats the character has that other characters don't

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void SetBaseStats(EntityBaseStats entityStats)
    {
        base.SetBaseStats(entityStats);

        //set extra character stats
    }
}
