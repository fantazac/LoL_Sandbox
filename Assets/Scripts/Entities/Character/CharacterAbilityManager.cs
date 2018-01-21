using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityManager : MonoBehaviour
{
    private Character character;

    [SerializeField]
    private Dictionary<AbilityInput, Ability> abilities;
    private List<Ability> currentlyUsedAbilities;

    private CharacterAbilityManager()
    {
        abilities = new Dictionary<AbilityInput, Ability>();
        currentlyUsedAbilities = new List<Ability>();
    }

    private void Start()
    {
        character = GetComponent<Character>();
        InitAbilities(abilities);
    }

    private void InitAbilities(Dictionary<AbilityInput, Ability> abilities)
    {
        Ability[] abilitiesOnCharacter = GetComponents<Ability>();
        for (int i = 0; i < abilitiesOnCharacter.Length; i++)
        {
            abilitiesOnCharacter[i].OnAbilityUsed += OnAbilityUsed;
            abilitiesOnCharacter[i].OnAbilityFinished += OnAbilityFinished;

            abilities.Add((AbilityInput)i, abilitiesOnCharacter[i]);
        }
    }

    private void SendToServer_Ability_Destination(AbilityInput abilityInput, Vector3 destination)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Destination", PhotonTargets.AllViaServer, abilityInput, destination);
    }

    [PunRPC]
    private void ReceiveFromServer_Ability_Destination(AbilityInput abilityInput, Vector3 destination)
    {
        if (!IsUsingAbilityPreventingAbilityCasts())
        {
            abilities[abilityInput].UseAbility(destination);
        }
        //else put in queue
    }

    private void SendToServer_Ability_Entity(AbilityInput abilityInput, Entity target)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_Entity", PhotonTargets.AllViaServer, abilityInput, target.EntityId, target.EntityType);
    }

    [PunRPC]
    private void ReceiveFromServer_Ability_Entity(AbilityInput abilityInput, int entityId, EntityType entityType)
    {
        if (!IsUsingAbilityPreventingAbilityCasts())
        {
            abilities[abilityInput].UseAbility(FindTarget(entityId, entityType));
        }
        //else put in queue
    }

    private Entity FindTarget(int entityId, EntityType entityType) // TODO: when adding an EntityType
    {
        Entity entity = null;
        switch (entityType)
        {
            case EntityType.CHARACTER:
                foreach (Character character in FindObjectsOfType<Character>())
                {
                    if (character.EntityId == entityId)
                    {
                        entity = character;
                        break;
                    }
                }
                break;
            /*case EntityType.MINION:
                foreach (Minion minion in FindObjectsOfType<Minion>())
                {
                    if (minion.EntityId == entityId)
                    {
                        entity = minion;
                        break;
                    }
                }
                break;*/
        }
        return entity;
    }

    private void OnAbilityUsed(Ability ability)
    {
        currentlyUsedAbilities.Add(ability);
    }

    private void OnAbilityFinished(Ability ability)
    {
        currentlyUsedAbilities.Remove(ability);
    }

    public void OnPressedInputForAbility(AbilityInput abilityInput)
    {
        if (abilities.ContainsKey(abilityInput))
        {
            Ability ability = abilities[abilityInput];
            if (ability is UnitTargeted)
            {
                Entity hoveredEntity = character.CharacterMouseManager.HoveredEntity;
                if (hoveredEntity != null && ability.CanBeCast(character.CharacterMouseManager.HoveredEntity, this))
                {
                    if (StaticObjects.OnlineMode)
                    {
                        SendToServer_Ability_Entity(abilityInput, hoveredEntity);
                    }
                    else
                    {
                        ability.UseAbility(hoveredEntity);
                    }
                }
            }
            else if (ability.CanBeCast(Input.mousePosition, this))
            {
                if (StaticObjects.OnlineMode)
                {
                    SendToServer_Ability_Destination(abilityInput, ability.GetDestination());
                }
                else
                {
                    ability.UseAbility(ability.GetDestination());
                }
            }
        }
    }

    public bool IsUsingAbilityPreventingMovement()
    {
        if (currentlyUsedAbilities.Count == 0)
        {
            return false;
        }

        foreach (Ability ability in currentlyUsedAbilities)
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

    public bool IsUsingAbilityPreventingRotation()
    {
        if (currentlyUsedAbilities.Count == 0)
        {
            return false;
        }

        foreach (Ability ability in currentlyUsedAbilities)
        {
            if (!ability.CanRotateWhileCasting)
            {
                return true;
            }
        }

        return false;
    }
}
