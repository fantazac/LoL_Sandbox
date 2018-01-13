﻿using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityManager : CharacterBase
{
    [SerializeField]
    private Dictionary<AbilityInput, Ability> abilities;
    private List<Ability> currentlyUsedAbilities;

    private CharacterAbilityManager()
    {
        abilities = new Dictionary<AbilityInput, Ability>();
        currentlyUsedAbilities = new List<Ability>();
    }

    protected override void Start()
    {
        base.Start();

        InitAbilitiesDictionnary();
        SubscribeToAbilitiesEvents(abilities);
    }

    private void InitAbilitiesDictionnary()
    {
        Ability[] abilitiesOnCharacter = GetComponents<Ability>();
        for (int i = 0; i < abilitiesOnCharacter.Length; i++)
        {
            abilities.Add((AbilityInput)i, abilitiesOnCharacter[i]);
        }
    }

    private void SubscribeToAbilitiesEvents(Dictionary<AbilityInput, Ability> abilities)
    {
        foreach (Ability ability in abilities.Values)
        {
            if (ability != null)
            {
                ability.OnAbilityUsed += OnAbilityUsed;
                ability.OnAbilityFinished += OnAbilityFinished;
            }
        }
    }

    private void SendToServer_Ability(AbilityInput abilityInput, Vector3 destination)
    {
        PhotonView.RPC("ReceiveFromServer_Ability", PhotonTargets.AllViaServer, abilityInput, destination);
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability(AbilityInput abilityInput, Vector3 destination)
    {
        if (!IsUsingAbilityPreventingAbilityCasts())
        {
            abilities[abilityInput].UseAbility(destination);
        }
        //else put in queue
    }

    private void OnAbilityUsed(Ability ability)
    {
        currentlyUsedAbilities.Add(ability);
    }

    private void OnAbilityFinished(Ability ability)
    {
        currentlyUsedAbilities.Remove(ability);
    }

    public void OnPressedInputForAbility(AbilityInput abilityInput, Vector3 mousePosition)
    {
        if (abilities.ContainsKey(abilityInput))
        {
            Ability ability = abilities[abilityInput];
            if (ability.CanBeCast(mousePosition))
            {
                Vector3 destination = ability.GetDestination();
                if (StaticObjects.OnlineMode)
                {
                    SendToServer_Ability(abilityInput, destination);
                }
                else
                {
                    ability.UseAbility(destination);
                }
            }
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
