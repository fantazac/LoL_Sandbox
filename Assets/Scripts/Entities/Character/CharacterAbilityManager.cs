using UnityEngine;

public class CharacterAbilityManager : CharacterBase
{
    [SerializeField]
    private Ability[] characterAbilities;
    [SerializeField]
    private Ability[] otherAbilities;

    public bool isUsingAbilityWithOtherAbilityCastsUnallowed;
    public bool isUsingAbilityWithMovementUnallowed;

    protected override void Start()
    {
        base.Start();

        CharacterInput.OnPressedCharacterAbility += OnPressedInputForCharacterAbility;
        CharacterInput.OnPressedOtherAbility += OnPressedInputForOtherAbility;

        InitAbilities(characterAbilities);
        InitAbilities(otherAbilities);
    }

    private void InitAbilities(Ability[] abilities)
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] != null)
            {
                abilities[i].AbilityId = i;
                if (abilities[i] is CharacterAbility)
                {
                    abilities[i].SendToServer_Ability += SendToServer_CharacterAbility;
                }
                else if (abilities[i] is OtherAbility)
                {
                    abilities[i].SendToServer_Ability += SendToServer_OtherAbility;
                }

                if (!abilities[i].CanCastOtherAbilitiesWithCasting)
                {
                    abilities[i].OnAbilityUsedWithOtherAbilityCastsUnallowed += OnAbilityUsedWithOtherAbilityCastsUnallowed;
                    abilities[i].OnAbilityUsedWithOtherAbilityCastsUnallowedFinished += OnAbilityUsedWithOtherAbilityCastsUnallowedFinished;
                }
                if (!abilities[i].CanMoveWhileCasting)
                {
                    abilities[i].OnAbilityUsedWithMovementUnallowedDuringCast += OnAbilityUsedWithMovementUnallowedDuringCast;
                    abilities[i].OnAbilityUsedWithMovementUnallowedDuringCastFinished += OnAbilityUsedWithMovementUnallowedDuringCastFinished;
                }
            }
        }
    }

    private void SendToServer_CharacterAbility(int abilityId, Vector3 destination)
    {
        PhotonView.RPC("ReceiveFromServer_CharacterAbility", PhotonTargets.AllViaServer, abilityId, destination);
    }

    [PunRPC]
    protected void ReceiveFromServer_CharacterAbility(int abilityId, Vector3 destination)
    {
        characterAbilities[abilityId].UseAbility(destination);
    }

    private void SendToServer_OtherAbility(int abilityId, Vector3 destination)
    {
        PhotonView.RPC("ReceiveFromServer_OtherAbility", PhotonTargets.AllViaServer, abilityId, destination);
    }

    [PunRPC]
    protected void ReceiveFromServer_OtherAbility(int abilityId, Vector3 destination)
    {
        otherAbilities[abilityId].UseAbility(destination);
    }

    private void OnAbilityUsedWithOtherAbilityCastsUnallowed()
    {
        isUsingAbilityWithOtherAbilityCastsUnallowed = true;
    }

    private void OnAbilityUsedWithOtherAbilityCastsUnallowedFinished()
    {
        isUsingAbilityWithOtherAbilityCastsUnallowed = false;
    }

    private void OnAbilityUsedWithMovementUnallowedDuringCast()
    {
        isUsingAbilityWithMovementUnallowed = true;
    }

    private void OnAbilityUsedWithMovementUnallowedDuringCastFinished()
    {
        isUsingAbilityWithMovementUnallowed = false;
    }

    private void OnPressedInputForCharacterAbility(int abilityId, Vector3 mousePosition)
    {
        if(characterAbilities[abilityId] != null)
        {
            characterAbilities[abilityId].OnPressedInput(mousePosition);
        }
    }

    private void OnPressedInputForOtherAbility(int abilityId, Vector3 mousePosition)
    {
        if(otherAbilities[abilityId] != null && ((StaticObjects.OnlineMode && !otherAbilities[abilityId].OfflineOnly) || !StaticObjects.OnlineMode))
        {
            otherAbilities[abilityId].OnPressedInput(mousePosition);
        }
    }
}
