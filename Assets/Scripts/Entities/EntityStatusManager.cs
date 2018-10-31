using System.Collections.Generic;
using UnityEngine;

public class EntityStatusManager : MonoBehaviour
{
    private Entity entity;
    private Character character;

    public List<CrowdControlEffects> CrowdControlEffectsOnCharacter { get; private set; }

    private int blockBasicAttacksCount;
    private int blockBasicAbilitiesCount;
    private int blockMovementCount;
    private int blockMovementAbilitiesCount;
    private int blockSummonerAbilitiesCount;
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

    private void AddOrRemoveCrowdControlEffectFromEntity(CrowdControlEffects crowdControlEffect, bool remove)//TODO: Split it in half
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
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.StopAllMovement();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.CRIPPLE:
                break;
            case CrowdControlEffects.DISARM:
                SetCannotUseBasicAttacks(count);
                if (!remove && character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.DISRUPT:
                if (!remove && character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                }
                break;
            case CrowdControlEffects.DROWSY:
                break;
            case CrowdControlEffects.ENTANGLE:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                if (!remove && character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.FEAR:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.StopAllMovement();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.FLEE:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.StopAllMovement();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.GROUND:
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffects.KNOCKASIDE:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.KNOCKBACK:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.SetMoveTowardsHalfDistanceOfAbilityCastRange();
                        character.CharacterMovementManager.SetCharacterIsInTargetRangeEventForBasicAttack();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.KNOCKDOWN:
                if (!remove && character.EntityDisplacementManager)
                {
                    character.EntityDisplacementManager.StopCurrentDisplacement();
                }
                break;
            case CrowdControlEffects.KNOCKUP:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.SetMoveTowardsHalfDistanceOfAbilityCastRange();
                        character.CharacterMovementManager.SetCharacterIsInTargetRangeEventForBasicAttack();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.NEARSIGHT://TODO
                break;
            case CrowdControlEffects.PACIFY:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                if (!remove && character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.POLYMORPH:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                if (!remove && character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffects.PULL:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.ROOT:
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    }
                }
                break;
            case CrowdControlEffects.SILENCE://TODO: You continue to walk towards the target position if you casted a spell, ground or unit, and you do nothing while you're silenced if you stop moving
                SetCannotUseBasicAbilities(count);
                if (!remove && character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                }
                break;
            case CrowdControlEffects.SLEEP:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.SLOW:
                break;
            case CrowdControlEffects.STASIS:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.STUN:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.SUPPRESION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.SUSPENSION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        if (!character.CharacterMovementManager.IsMovingTowardsPositionForAnEvent() && !character.CharacterMovementManager.IsMovingTowardsTarget())
                        {
                            character.CharacterAutoAttackManager.EnableAutoAttackWithBiggerRange();
                        }
                        character.CharacterMovementManager.StopAllMovement();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
            case CrowdControlEffects.TAUNT:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (!remove)
                {
                    if (character.CharacterMovementManager)
                    {
                        character.CharacterMovementManager.StopAllMovement();
                    }
                    if (character.CharacterAbilityManager)
                    {
                        character.CharacterAbilityManager.CancelAllChannelingAbilities();
                        character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                    }
                }
                break;
        }
    }

    private void SetCannotUseBasicAttacks(int count)
    {
        blockBasicAttacksCount += count;
        if (CanBlockAbilitiesOrBasicAttacks(count, blockBasicAttacksCount) && entity.EntityBasicAttack)
        {
            entity.EntityBasicAttack.StopBasicAttack(true);
        }
    }

    private void SetCannotUseMovement(int count)
    {
        blockMovementCount += count;
    }

    private void SetIsBlinded(int count)
    {
        isBlindedCount += count;
    }

    private void SetCannotUseBasicAbilities(int count)
    {
        blockBasicAbilitiesCount += count;
        if (character.CharacterAbilityManager)
        {
            if (CanBlockAbilitiesOrBasicAttacks(count, blockBasicAbilitiesCount))
            {
                character.CharacterAbilityManager.BlockAllBasicAbilities();
            }
            else if (CanUnblockAbilitiesOrBasicAttacks(count, blockBasicAbilitiesCount))
            {
                character.CharacterAbilityManager.UnblockAllBasicAbilities(blockMovementAbilitiesCount == 0, blockSummonerAbilitiesCount == 0);
            }
        }
    }

    private void SetCannotUseMovementAbilities(int count)
    {
        blockMovementAbilitiesCount += count;
        if (blockBasicAbilitiesCount == 0 && character.CharacterAbilityManager)
        {
            if (CanBlockAbilitiesOrBasicAttacks(count, blockMovementAbilitiesCount))
            {
                character.CharacterAbilityManager.BlockAllMovementAbilities();
            }
            else if (CanUnblockAbilitiesOrBasicAttacks(count, blockMovementAbilitiesCount))
            {
                character.CharacterAbilityManager.UnblockAllMovementAbilities();
            }
        }
    }

    private void SetCannotUseSummonerAbilities(int count)
    {
        blockSummonerAbilitiesCount += count;
        if (character.CharacterAbilityManager)
        {
            if (CanBlockAbilitiesOrBasicAttacks(count, blockSummonerAbilitiesCount))
            {
                character.CharacterAbilityManager.BlockAllSummonerAbilities();
            }
            else if (CanUnblockAbilitiesOrBasicAttacks(count, blockSummonerAbilitiesCount))
            {
                character.CharacterAbilityManager.UnblockAllSummonerAbilities(blockMovementAbilitiesCount == 0);
            }
        }
    }

    protected bool CanBlockAbilitiesOrBasicAttacks(int count, int blockAbilitiesCount)
    {
        return count == 1 && blockAbilitiesCount == 1;
    }

    protected bool CanUnblockAbilitiesOrBasicAttacks(int count, int blockAbilitiesCount)
    {
        return count == -1 && blockAbilitiesCount == 0;
    }

    public bool CanUseBasicAbilities()
    {
        return blockBasicAbilitiesCount == 0;
    }

    public bool CanUseBasicAttacks()
    {
        return blockBasicAttacksCount == 0;
    }

    public bool CanUseMovement()
    {
        return blockMovementCount == 0;
    }

    public bool IsBlinded()
    {
        return isBlindedCount > 0;
    }

    public bool CanReduceCrowdControlDuration(CrowdControlEffects buffCrowdControlEffect)
    {
        return buffCrowdControlEffect != CrowdControlEffects.NONE &&
            buffCrowdControlEffect != CrowdControlEffects.SUPPRESION &&
            buffCrowdControlEffect != CrowdControlEffects.STASIS &&
            !IsAnAirborneEffect(buffCrowdControlEffect);
    }

    public bool IsAnAirborneEffect(CrowdControlEffects buffCrowdControlEffect)
    {
        return buffCrowdControlEffect == CrowdControlEffects.KNOCKASIDE ||
            buffCrowdControlEffect == CrowdControlEffects.KNOCKBACK ||
            buffCrowdControlEffect == CrowdControlEffects.KNOCKUP ||
            buffCrowdControlEffect == CrowdControlEffects.PULL ||
            buffCrowdControlEffect == CrowdControlEffects.SUSPENSION;
    }
}
