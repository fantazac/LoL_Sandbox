using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityManager : CharacterBase
{
    [SerializeField]
    private Ability[] characterAbilities;
    [SerializeField]
    private Ability[] otherAbilities;

    protected override void Start()
    {
        base.Start();

        CharacterInput.OnPressedCharacterAbility += OnPressedInputForCharacterAbility;
        CharacterInput.OnPressedOtherAbility += OnPressedInputForOtherAbility;
    }

    private void OnPressedInputForCharacterAbility(int abilityId)
    {
        characterAbilities[abilityId].OnPressedInput();
    }

    private void OnPressedInputForOtherAbility(int abilityId)
    {
        if((StaticObjects.OnlineMode && !otherAbilities[abilityId].OfflineOnly) || !StaticObjects.OnlineMode)
        {
            otherAbilities[abilityId].OnPressedInput();
        }
    }
}
