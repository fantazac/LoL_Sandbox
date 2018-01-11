using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityManager : CharacterBase
{
    [SerializeField]
    private Ability[] characterAbilities;
    [SerializeField]
    private Ability[] otherAbilities;
    private Dictionary<AbilityInput, Ability> abilities;

    private List<Ability> currentlyUsedAbilities;

    private CharacterAbilityManager()
    {
        currentlyUsedAbilities = new List<Ability>();
    }

    protected override void Start()
    {
        base.Start();

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

                abilities[i].OnAbilityUsed += OnAbilityUsed;
                abilities[i].OnAbilityFinished += OnAbilityFinished;
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
