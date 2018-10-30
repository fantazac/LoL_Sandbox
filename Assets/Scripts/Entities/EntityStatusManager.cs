using System.Collections.Generic;
using UnityEngine;

public class EntityStatusManager : MonoBehaviour
{
    private Entity entity;
    private Character character;

    public List<CrowdControlEffects> CrowdControlEffectsOnCharacter { get; private set; }

    private int cannotUseBasicAttacksCount;
    private int cannotUseBasicAbilitiesCount;
    private int cannotUseLongRangedAbilitiesCount;
    private int cannotUseMovementCount;
    private int cannotUseMovementAbilitiesCount;
    private int cannotUseSummonerAbilitiesCount;
    private int isBlindedCount;

    private EntityStatusManager()
    {
        CrowdControlEffectsOnCharacter = new List<CrowdControlEffects>();
    }

    private void Start()
    {
        entity = GetComponent<Entity>();
        if (entity is Character)
        {
            character = (Character)entity;
        }
    }

    public void AddCrowdControlEffectOnEntity(CrowdControlEffects crowdControlEffect)
    {
        CrowdControlEffectsOnCharacter.Add(crowdControlEffect);
        AddOrRemoveCrowdControlEffectFromEntity(crowdControlEffect, false);
    }

    public void RemoveCrowdControlEffectFromEntity(CrowdControlEffects crowdControlEffect)
    {
        CrowdControlEffectsOnCharacter.Remove(crowdControlEffect);
        AddOrRemoveCrowdControlEffectFromEntity(crowdControlEffect, true);
    }

    private void AddOrRemoveCrowdControlEffectFromEntity(CrowdControlEffects crowdControlEffect, bool remove)
    {
        int count = remove ? -1 : 1;
        switch (crowdControlEffect)
        {
            case CrowdControlEffects.BLIND:
                SetIsBlinded(count);
                break;
            case CrowdControlEffects.CHARM:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterMovement)
                {
                    character.CharacterMovement.StopAllMovement();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.CRIPPLE:
                break;
            case CrowdControlEffects.DISARM:
                SetCannotUseBasicAttacks(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.DISRUPT:
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                }
                break;
            case CrowdControlEffects.DROWSY://TODO
                break;
            case CrowdControlEffects.ENTANGLE:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.FEAR://TODO
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.FLEE:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterMovement)
                {
                    character.CharacterMovement.StopAllMovement();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.GROUND:
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffects.KNOCKASIDE:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                character.CharacterMovement.StopMovementTowardsPointIfHasEvent();
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.KNOCKBACK:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                character.CharacterMovement.SetMoveTowardsHalfDistanceOfAbilityCastRange();
                character.CharacterMovement.SetCharacterIsInTargetRangeEventForBasicAttack();
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.KNOCKDOWN:
                if (character.EntityDisplacementManager)
                {
                    character.EntityDisplacementManager.StopCurrentDisplacement();
                }
                break;
            case CrowdControlEffects.KNOCKUP:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                character.CharacterMovement.SetMoveTowardsHalfDistanceOfAbilityCastRange();
                character.CharacterMovement.SetCharacterIsInTargetRangeEventForBasicAttack();
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.NEARSIGHT://TODO
                SetCannotUseLongRangedAbilities(count);
                break;
            case CrowdControlEffects.PACIFY:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.POLYMORPH:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.PULL:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                character.CharacterMovement.StopMovementTowardsPointIfHasEvent();
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.ROOT:
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                character.CharacterMovement.StopMovementTowardsPointIfHasEvent();
                break;
            case CrowdControlEffects.SILENCE:
                SetCannotUseBasicAbilities(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                }
                break;
            case CrowdControlEffects.SLEEP://TODO
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.SLOW:
                break;
            case CrowdControlEffects.STASIS://TODO
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.STUN:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                character.CharacterMovement.StopMovementTowardsPointIfHasEvent();
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.SUPPRESION://TODO
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.SUSPENSION://TODO
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (!character.CharacterMovement.IsMovingTowardsPosition())
                {
                    character.CharacterAutoAttack.EnableAutoAttackWithBiggerRange();
                }
                character.CharacterMovement.StopMovementTowardsPoint();
                character.CharacterMovement.StopMovementTowardsTargetIfHasEvent();
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.TAUNT://TODO
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterMovement)
                {
                    character.CharacterMovement.StopAllMovement();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
        }
    }

    private void SetCannotUseBasicAttacks(int count)
    {
        cannotUseBasicAttacksCount += count;
        if (count == 1 && cannotUseBasicAttacksCount == 1 && entity.EntityBasicAttack)
        {
            entity.EntityBasicAttack.StopBasicAttack(true);
        }
    }

    private void SetCannotUseMovement(int count)
    {
        cannotUseMovementCount += count;
    }

    private void SetIsBlinded(int count)
    {
        isBlindedCount += count;
    }

    private void SetCannotUseBasicAbilities(int count)
    {
        cannotUseBasicAbilitiesCount += count;
        if (character.CharacterAbilityManager)
        {
            if (count == 1 && cannotUseBasicAbilitiesCount == 1)
            {
                character.CharacterAbilityManager.BlockAllBasicAbilities();
            }
            else if (count == -1 && cannotUseBasicAbilitiesCount == 0)
            {
                character.CharacterAbilityManager.UnblockAllBasicAbilities(cannotUseLongRangedAbilitiesCount > 0, cannotUseMovementAbilitiesCount > 0, cannotUseSummonerAbilitiesCount > 0);
            }
        }
    }

    private void SetCannotUseLongRangedAbilities(int count)
    {
        cannotUseLongRangedAbilitiesCount += count;
        if (cannotUseBasicAbilitiesCount == 0 && character.CharacterAbilityManager)
        {
            if (count == 1 && cannotUseLongRangedAbilitiesCount == 1)
            {
                character.CharacterAbilityManager.BlockAllLongRangedAbilities();
            }
            else if (count == -1 && cannotUseLongRangedAbilitiesCount == 0)
            {
                character.CharacterAbilityManager.UnblockAllLongRangedAbilities();
            }
        }
    }

    private void SetCannotUseMovementAbilities(int count)
    {
        cannotUseMovementAbilitiesCount += count;
        if (cannotUseBasicAbilitiesCount == 0 && character.CharacterAbilityManager)
        {
            if (count == 1 && cannotUseMovementAbilitiesCount == 1)
            {
                character.CharacterAbilityManager.BlockAllMovementAbilities();
            }
            else if (count == -1 && cannotUseMovementAbilitiesCount == 0)
            {
                character.CharacterAbilityManager.UnblockAllMovementAbilities();
            }
        }
    }

    private void SetCannotUseSummonerAbilities(int count)
    {
        cannotUseSummonerAbilitiesCount += count;
        if (character.CharacterAbilityManager)
        {
            if (count == 1 && cannotUseSummonerAbilitiesCount == 1)
            {
                character.CharacterAbilityManager.BlockAllSummonerAbilities();
            }
            else if (count == -1 && cannotUseSummonerAbilitiesCount == 0)
            {
                character.CharacterAbilityManager.UnblockAllSummonerAbilities(cannotUseBasicAbilitiesCount > 0);
            }
        }
    }

    public bool CanUseBasicAbilities()
    {
        return cannotUseBasicAbilitiesCount == 0;
    }

    public bool CanUseBasicAttacks()
    {
        return cannotUseBasicAttacksCount == 0;
    }

    public bool CanUseMovement()
    {
        return cannotUseMovementCount == 0;
    }

    public bool IsBlinded()
    {
        return isBlindedCount > 0;
    }
}
