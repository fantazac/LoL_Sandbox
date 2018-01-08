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

    private void OnPressedInputForCharacterAbility(int abilityId, Vector3 mousePosition)
    {
        characterAbilities[abilityId].OnPressedInput(mousePosition);
    }

    private void OnPressedInputForOtherAbility(int abilityId, Vector3 mousePosition)
    {
        if((StaticObjects.OnlineMode && !otherAbilities[abilityId].OfflineOnly) || !StaticObjects.OnlineMode)
        {
            otherAbilities[abilityId].OnPressedInput(mousePosition);
        }
    }
}
