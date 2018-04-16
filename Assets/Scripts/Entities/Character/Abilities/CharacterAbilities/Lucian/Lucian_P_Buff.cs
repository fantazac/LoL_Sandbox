using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_P_Buff : AbilityBuff
{
    protected Lucian_P_Buff()
    {
        buffName = "Lightslinger";

        buffDuration = 3;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianP_Buff";
    }
}
