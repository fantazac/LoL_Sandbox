using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatusManager : MonoBehaviour
{
    protected Entity entity;

    protected List<CrowdControlEffects> crowdControlEffectsOnCharacter;

    protected int cannotUseBasicAttacksCount;
    protected int cannotUseBasicAbilitiesCount;
    protected int cannotUseLongRangedAbilitiesCount;
    protected int cannotUseMovementCount;
    protected int cannotUseMovementAbilitiesCount;
    protected int cannotUseSummonerAbilitiesCount;
    protected int isBlindedCount;

    protected EntityStatusManager()
    {
        crowdControlEffectsOnCharacter = new List<CrowdControlEffects>();
    }

    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
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

    protected virtual void AddOrRemoveCrowdControlEffectFromEntity(CrowdControlEffects crowdControlEffect, bool remove)
    {
        int count = remove ? -1 : 1;
        switch (crowdControlEffect)
        {
            case CrowdControlEffects.BLIND:
                SetIsBlinded(count);
                break;
            case CrowdControlEffects.CHARM:
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
                break;
            case CrowdControlEffects.FEAR:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.FLEE:
                break;
            case CrowdControlEffects.GROUND:
                break;
            case CrowdControlEffects.KNOCKASIDE:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.KNOCKBACK:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.KNOCKDOWN:
                break;
            case CrowdControlEffects.KNOCKUP:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.NEARSIGHT:
                break;
            case CrowdControlEffects.PACIFY:
                SetCannotUseBasicAttacks(count);
                break;
            case CrowdControlEffects.POLYMORPH:
                SetCannotUseBasicAttacks(count);
                break;
            case CrowdControlEffects.PULL:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.ROOT:
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.SILENCE:
                break;
            case CrowdControlEffects.SLEEP:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.SLOW:
                break;
            case CrowdControlEffects.STASIS:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.STUN:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.SUPPRESION:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.SUSPENSION:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffects.TAUNT:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
        }
    }

    protected void SetCannotUseBasicAttacks(int count)//Should not be used since we check CanUseBasicAttacks() in EntityBasicAttack
    {
        cannotUseBasicAttacksCount += count;
        if (count == 1 && cannotUseBasicAttacksCount == 1)
        {
            //Disable basic attacking
        }
        else if (count == -1 && cannotUseBasicAttacksCount == 0)
        {
            //Enable basic attacking
        }
    }

    protected void SetCannotUseMovement(int count)//Should not be used since we check CanUseMovement() in CharacterMovement
    {
        cannotUseMovementCount += count;
        if (count == 1 && cannotUseMovementCount == 1)
        {
            //Disable normal movement
        }
        else if (count == -1 && cannotUseMovementCount == 0)
        {
            //Enable normal movement
        }
    }

    protected void SetIsBlinded(int count)
    {
        isBlindedCount += count;
        if (count == 1 && isBlindedCount == 1)
        {
            //Enable auto misses
        }
        else if (count == -1 && isBlindedCount == 0)
        {
            //Disable auto misses
        }
    }

    public bool CanUseBasicAttacks()
    {
        return cannotUseBasicAbilitiesCount == 0;
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
