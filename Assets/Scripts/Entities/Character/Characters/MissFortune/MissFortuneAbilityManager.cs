using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortuneAbilityManager : CharacterAbilityManager
{
    protected override void InitAbilities()
    {
        CharacterAbilities = new Ability[] { gameObject.AddComponent<Ezreal_Q>(), gameObject.AddComponent<Ezreal_W>(), gameObject.AddComponent<Ezreal_E>(), gameObject.AddComponent<Ezreal_R>() };
        PassiveCharacterAbilities = new Ability[] { gameObject.AddComponent<MissFortune_P>() };
        SummonerAbilities = new Ability[] { gameObject.AddComponent<Heal>(), gameObject.AddComponent<Flash>() };

        base.InitAbilities();
    }
}
