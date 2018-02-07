using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_P : PassiveTargeted, CharacterAbility, PassiveCharacterAbility
{
    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Ezreal/EzrealP";
        buffSpritePath = "Sprites/CharacterAbilities/Ezreal/EzrealP_Buff";
    }
}
