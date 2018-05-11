using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAbilityManager : CharacterAbilityManager
{
    protected override void InitAbilities()
    {
        CharacterAbilities = new Ability[] { gameObject.AddComponent<CC_Q>(), gameObject.AddComponent<CC_W>() };
        PassiveCharacterAbilities = new Ability[] { };
        SummonerAbilities = new Ability[] { };

        base.InitAbilities();
    }
}
