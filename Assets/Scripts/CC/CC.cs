using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC : Character
{
    protected CC()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/CC";
    }

    protected override void SetCharacterSpecificScripts()
    {
        EntityBasicAttack = gameObject.AddComponent<CCBasicAttack>();
        EntityStats = gameObject.AddComponent<CCStats>();

        CharacterAbilityManager = gameObject.AddComponent<CCAbilityManager>();
    }
}
