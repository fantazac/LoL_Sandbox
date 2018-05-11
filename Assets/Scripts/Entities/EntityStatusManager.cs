using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatusManager : MonoBehaviour
{
    private Entity entity;
    private Character character;

    private List<CrowdControlEffects> crowdControlEffectsOnCharacter;

    private int cannotUseBasicAttacksCount;
    private int cannotUseBasicAbilitiesCount;
    private int cannotUseLongRangedAbilitiesCount;
    private int cannotUseMovementCount;
    private int cannotUseMovementAbilitiesCount;
    private int cannotUseSummonerAbilitiesCount;
    private int isBlindedCount;

    private EntityStatusManager()
    {
        crowdControlEffectsOnCharacter = new List<CrowdControlEffects>();
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
        crowdControlEffectsOnCharacter.Add(crowdControlEffect);
        AddOrRemoveCrowdControlEffectFromEntity(crowdControlEffect, false);
    }

    public void RemoveCrowdControlEffectFromEntity(CrowdControlEffects crowdControlEffect)
    {
        crowdControlEffectsOnCharacter.Remove(crowdControlEffect);
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
                break;
            case CrowdControlEffects.CRIPPLE:
                break;
            case CrowdControlEffects.DISARM:
                SetCannotUseBasicAttacks(count);
                break;
            case CrowdControlEffects.DISRUPT:
                break;
            case CrowdControlEffects.DROWSY:
                break;
            case CrowdControlEffects.ENTANGLE:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffects.FEAR:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.FLEE:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.GROUND:
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffects.KNOCKASIDE:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.KNOCKBACK:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.KNOCKDOWN:
                if (character.CharacterAbilityManager)
                {
                    character.CharacterAbilityManager.StopAllDashAbilities();//TODO: cancels all airborne effects in certain cases (VeigarE) 
                }
                break;
            case CrowdControlEffects.KNOCKUP:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.NEARSIGHT:
                SetCannotUseLongRangedAbilities(count);
                break;
            case CrowdControlEffects.PACIFY:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                break;
            case CrowdControlEffects.POLYMORPH:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                break;
            case CrowdControlEffects.PULL:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.ROOT:
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffects.SILENCE:
                SetCannotUseBasicAbilities(count);
                break;
            case CrowdControlEffects.SLEEP:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.SLOW:
                break;
            case CrowdControlEffects.STASIS:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                break;
            case CrowdControlEffects.STUN:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.SUPPRESION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                break;
            case CrowdControlEffects.SUSPENSION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.TAUNT:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
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
