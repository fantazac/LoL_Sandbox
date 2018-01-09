using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityManager : CharacterBase
{
    [SerializeField]
    private Ability[] characterAbilities;
    [SerializeField]
    private Ability[] otherAbilities;

    public bool isCastingAbility;

    protected override void Start()
    {
        base.Start();

        CharacterInput.OnPressedCharacterAbility += OnPressedInputForCharacterAbility;
        CharacterInput.OnPressedOtherAbility += OnPressedInputForOtherAbility;

        foreach(Ability ability in characterAbilities)
        {
            if (ability != null && ability.HasCastTime)
            {
                ability.OnAbilityCast += OnAbilityCast;
                ability.OnAbilityCastFinished += OnAbilityCastFinished;
            }
        }

        foreach (Ability ability in otherAbilities)
        {
            if (ability != null && ability.HasCastTime)
            {
                ability.OnAbilityCast += OnAbilityCast;
                ability.OnAbilityCastFinished += OnAbilityCastFinished;
            }
        }
    }

    private void OnAbilityCast()
    {
        isCastingAbility = true;
    }

    private void OnAbilityCastFinished()
    {
        isCastingAbility = false;
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
