using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_P : PassiveTargeted, CharacterAbility, PassiveCharacterAbility
{
    protected override void SetAbilitySpritePath()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Ezreal/EzrealP";
    }
}
