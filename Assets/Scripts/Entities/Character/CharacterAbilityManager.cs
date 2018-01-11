using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityManager : CharacterBase
{
    [SerializeField]
    private Ability[] characterAbilities;
    [SerializeField]
    private Ability[] otherAbilities;

    private List<Ability> currentlyUsedAbilities;

    private CharacterAbilityManager()
    {
        currentlyUsedAbilities = new List<Ability>();
    }

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

                abilities[i].OnAbilityUsed += OnAbilityUsed;
                abilities[i].OnAbilityFinished += OnAbilityFinished;
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
        if (!IsUsingAbilityPreventingAbilityCasts())
        {
            characterAbilities[abilityId].UseAbility(destination);
        }
        //else put in queue
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

    private void OnAbilityUsed(Ability ability)
    {
        currentlyUsedAbilities.Add(ability);
    }

    private void OnAbilityFinished(Ability ability)
    {
        currentlyUsedAbilities.Remove(ability);
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
        if(otherAbilities[abilityId] != null && (!StaticObjects.OnlineMode || !otherAbilities[abilityId].OfflineOnly))
        {
            otherAbilities[abilityId].OnPressedInput(mousePosition);
        }
    }

    public bool IsUsingAbilityPreventingMovement()
    {
        if(currentlyUsedAbilities.Count == 0)
        {
            return false;
        }

        foreach(Ability ability in currentlyUsedAbilities)
        {
            if (!ability.CanMoveWhileCasting)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsUsingAbilityPreventingAbilityCasts()
    {
        if (currentlyUsedAbilities.Count == 0)
        {
            return false;
        }

        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (!ability.CanCastOtherAbilitiesWithCasting)
            {
                return true;
            }
        }

        return false;
    }
}
