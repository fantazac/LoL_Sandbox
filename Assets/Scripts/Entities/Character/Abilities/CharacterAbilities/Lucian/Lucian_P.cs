using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_P : PassiveTargeted, CharacterAbility, PassiveCharacterAbility
{
    protected override void SetAbilitySpritePath()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Lucian/LucianP";
    }
}
