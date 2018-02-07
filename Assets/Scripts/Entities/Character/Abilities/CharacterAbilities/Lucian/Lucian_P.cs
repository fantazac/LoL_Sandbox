using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_P : PassiveTargeted, CharacterAbility, PassiveCharacterAbility
{
    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Lucian/LucianP";
        buffSpritePath = "Sprites/CharacterAbilities/Lucian/LucianP_Buff";
    }
}
