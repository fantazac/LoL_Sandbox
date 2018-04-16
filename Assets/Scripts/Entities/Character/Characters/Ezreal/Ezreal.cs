using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal : Character
{
    protected Ezreal()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Ezreal";
    }

    protected override void SetCharacterSpecificScripts()
    {
        EntityBasicAttack = gameObject.AddComponent<EzrealBasicAttack>();
        EntityStats = gameObject.AddComponent<EzrealStats>();

        CharacterAbilityManager = gameObject.AddComponent<EzrealAbilityManager>();
    }
}
