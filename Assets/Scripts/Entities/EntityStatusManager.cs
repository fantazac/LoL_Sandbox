using System.Collections.Generic;
using UnityEngine;

public class EntityStatusManager : MonoBehaviour
{
    private Entity entity;
    private Character character;//TODO: to remove

    public List<CrowdControlEffect> CrowdControlEffectsOnCharacter { get; private set; }

    private int blockBasicAttacksCount;
    private int blockBasicAbilitiesCount;
    private int blockMovementCount;
    private int blockMovementAbilitiesCount;
    private int blockSummonerAbilitiesCount;
    private int isBlindedCount;

    private EntityStatusManager()
    {
        CrowdControlEffectsOnCharacter = new List<CrowdControlEffect>();
    }

    private void Start()
    {
        entity = GetComponent<Entity>();
        if (entity is Character)
        {
            character = (Character)entity;
        }
    }

    public void AddCrowdControlEffect(CrowdControlEffect crowdControlEffect)
    {
        CrowdControlEffectsOnCharacter.Add(crowdControlEffect);
        ApplyCrowdControlAffectToEntity(crowdControlEffect);
    }

    public void RemoveCrowdControlEffect(CrowdControlEffect crowdControlEffect)
    {
        CrowdControlEffectsOnCharacter.Remove(crowdControlEffect);
        RemoveCrowdControlEffectFromEntity(crowdControlEffect);
    }

    private void ApplyCrowdControlAffectToEntity(CrowdControlEffect crowdControlEffect)
    {
        int count = 1;
        switch (crowdControlEffect)
        {
            case CrowdControlEffect.BLIND:
                SetIsBlinded(count);
                break;
            case CrowdControlEffect.CHARM:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterMovementManager)
                {
                    character.CharacterMovementManager.StopAllMovement();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.CRIPPLE:
                break;
            case CrowdControlEffect.DISARM:
                SetCannotUseBasicAttacks(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();//TODO: verify, wiki says yes though
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.DISRUPT:
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                }
                break;
            case CrowdControlEffect.DROWSY:
                break;
            case CrowdControlEffect.ENTANGLE:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.FEAR:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterMovementManager)
                {
                    character.CharacterMovementManager.StopAllMovement();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.FLEE:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterMovementManager)
                {
                    character.CharacterMovementManager.StopAllMovement();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.GROUND:
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffect.KNOCKASIDE:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterMovementManager)
                {
                    character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.KNOCKBACK:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
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
                break;
            case CrowdControlEffect.KNOCKDOWN:
                if (character.EntityDisplacementManager)
                {
                    character.EntityDisplacementManager.StopCurrentDisplacement();
                }
                break;
            case CrowdControlEffect.KNOCKUP:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
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
                break;
            case CrowdControlEffect.NEARSIGHT://TODO
                break;
            case CrowdControlEffect.PACIFY:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.POLYMORPH:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.PULL:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterMovementManager)
                {
                    character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.ROOT:
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                if (character.CharacterMovementManager)
                {
                    character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                }
                break;
            case CrowdControlEffect.SILENCE://TODO: You continue to walk towards the target position if you casted a spell, ground or unit, and you do nothing while you're silenced if you stop moving
                SetCannotUseBasicAbilities(count);
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                }
                break;
            case CrowdControlEffect.SLEEP:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterMovementManager)
                {
                    character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.SLOW:
                break;
            case CrowdControlEffect.STASIS:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                if (character.CharacterMovementManager)
                {
                    character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.STUN:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterMovementManager)
                {
                    character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.SUPPRESION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                if (character.CharacterMovementManager)
                {
                    character.CharacterMovementManager.StopMovementTowardsPointIfHasEvent();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
            case CrowdControlEffect.SUSPENSION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
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
                break;
            case CrowdControlEffect.TAUNT:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                if (character.CharacterMovementManager)
                {
                    character.CharacterMovementManager.StopAllMovement();
                }
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.CancelAllChannelingAbilities();
                    character.CharacterAbilityManager.CancelAllActiveAbilitiesThatAreNotBeingCastedOrChanneled();
                }
                break;
        }
    }

    private void RemoveCrowdControlEffectFromEntity(CrowdControlEffect crowdControlEffect)
    {
        int count = -1;
        switch (crowdControlEffect)
        {
            case CrowdControlEffect.BLIND:
                SetIsBlinded(count);
                break;
            case CrowdControlEffect.CHARM:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffect.CRIPPLE:
                break;
            case CrowdControlEffect.DISARM:
                SetCannotUseBasicAttacks(count);
                break;
            case CrowdControlEffect.DISRUPT:
                break;
            case CrowdControlEffect.DROWSY:
                break;
            case CrowdControlEffect.ENTANGLE:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffect.FEAR:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffect.FLEE:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffect.GROUND:
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffect.KNOCKASIDE:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffect.KNOCKBACK:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffect.KNOCKDOWN:
                break;
            case CrowdControlEffect.KNOCKUP:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffect.NEARSIGHT:
                break;
            case CrowdControlEffect.PACIFY:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                break;
            case CrowdControlEffect.POLYMORPH:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                break;
            case CrowdControlEffect.PULL:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffect.ROOT:
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffect.SILENCE:
                SetCannotUseBasicAbilities(count);
                break;
            case CrowdControlEffect.SLEEP:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffect.SLOW:
                break;
            case CrowdControlEffect.STASIS:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                break;
            case CrowdControlEffect.STUN:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffect.SUPPRESION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                break;
            case CrowdControlEffect.SUSPENSION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffect.TAUNT:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
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

    public bool CanReduceCrowdControlDuration(CrowdControlEffect buffCrowdControlEffect)
    {
        return buffCrowdControlEffect != CrowdControlEffect.NONE &&
            buffCrowdControlEffect != CrowdControlEffect.SUPPRESION &&
            buffCrowdControlEffect != CrowdControlEffect.STASIS &&
            !IsAnAirborneEffect(buffCrowdControlEffect);
    }

    public bool IsAnAirborneEffect(CrowdControlEffect buffCrowdControlEffect)
    {
        return buffCrowdControlEffect == CrowdControlEffect.KNOCKASIDE ||
            buffCrowdControlEffect == CrowdControlEffect.KNOCKBACK ||
            buffCrowdControlEffect == CrowdControlEffect.KNOCKUP ||
            buffCrowdControlEffect == CrowdControlEffect.PULL ||
            buffCrowdControlEffect == CrowdControlEffect.SUSPENSION;
    }
}
