using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian : Character
{
    protected Lucian()
    {
        characterPortraitPath = "Sprites/Characters/CharacterPortraits/Lucian";
    }

    protected override void SetCharacterSpecificScripts()
    {
        EntityBasicAttack = gameObject.AddComponent<LucianBasicAttack>();
        EntityStats = gameObject.AddComponent<LucianStats>();

        CharacterAbilityManager = gameObject.AddComponent<LucianAbilityManager>();
    }
}
