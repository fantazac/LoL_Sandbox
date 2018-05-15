using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAbilityManager : CharacterAbilityManager
{
    protected override void InitAbilities()
    {
        CharacterAbilities = new Ability[] { gameObject.AddComponent<CC_Q>(), gameObject.AddComponent<CC_W>(), gameObject.AddComponent<MissFortune_E>(), gameObject.AddComponent<Ezreal_R>() };
        PassiveCharacterAbilities = new Ability[] { };
        SummonerAbilities = new Ability[] { gameObject.AddComponent<Heal>(), gameObject.AddComponent<Flash>() };

        base.InitAbilities();
    }
}
