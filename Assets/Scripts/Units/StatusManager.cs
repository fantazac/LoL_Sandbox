using System.Collections.Generic;
using UnityEngine;

public abstract class StatusManager : MonoBehaviour
{
    public List<CrowdControlEffect> CrowdControlEffectsOnCharacter { get; private set; }

    protected int blockBasicAttacksCount;
    protected int blockBasicAbilitiesCount;
    protected int blockMovementCount;
    protected int blockMovementAbilitiesCount;
    protected int blockSummonerAbilitiesCount;
    protected int isBlindedCount;

    public delegate void OnCrowdControlEffectAddedHandler(CrowdControlEffect crowdControlEffect);
    public event OnCrowdControlEffectAddedHandler OnCrowdControlEffectAdded;

    public delegate void OnCrowdControlEffectRemovedHandler(CrowdControlEffect crowdControlEffect);
    public event OnCrowdControlEffectRemovedHandler OnCrowdControlEffectRemoved;

    protected StatusManager()
    {
        CrowdControlEffectsOnCharacter = new List<CrowdControlEffect>();
    }

    public void AddCrowdControlEffect(CrowdControlEffect crowdControlEffect)
    {
        CrowdControlEffectsOnCharacter.Add(crowdControlEffect);
        ApplyCrowdControlEffectToUnit(crowdControlEffect);
        if (OnCrowdControlEffectAdded != null)
        {
            OnCrowdControlEffectAdded(crowdControlEffect);
        }
    }

    public void RemoveCrowdControlEffect(CrowdControlEffect crowdControlEffect)
    {
        CrowdControlEffectsOnCharacter.Remove(crowdControlEffect);
        RemoveCrowdControlEffectFromUnit(crowdControlEffect);
        if (OnCrowdControlEffectRemoved != null)
        {
            OnCrowdControlEffectRemoved(crowdControlEffect);
        }
    }

    private void ApplyCrowdControlEffectToUnit(CrowdControlEffect crowdControlEffect)
    {
        int count = 1;
        switch (crowdControlEffect)
        {
            case CrowdControlEffect.BLIND:
                SetIsBlinded(count);
                break;
            case CrowdControlEffect.CHARM:
            case CrowdControlEffect.FEAR:
            case CrowdControlEffect.FLEE:
            case CrowdControlEffect.TAUNT:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                OnForcedAction();
                break;
            case CrowdControlEffect.CRIPPLE:
            case CrowdControlEffect.DROWSY:
            case CrowdControlEffect.SLOW:
                break;
            case CrowdControlEffect.DISARM:
                SetCannotUseBasicAttacks(count);
                OnDisarm();
                break;
            case CrowdControlEffect.DISRUPT:
                OnDisrupt();
                break;
            case CrowdControlEffect.ENTANGLE:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                OnEntangle();
                break;
            case CrowdControlEffect.GROUND:
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffect.KNOCKASIDE:
            case CrowdControlEffect.PULL:
            case CrowdControlEffect.SLEEP:
            case CrowdControlEffect.STUN:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                OnStun();
                break;
            case CrowdControlEffect.KNOCKBACK:
            case CrowdControlEffect.KNOCKUP:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                OnKnockUp();
                break;
            case CrowdControlEffect.KNOCKDOWN:
                OnKnockDown();
                break;
            case CrowdControlEffect.NEARSIGHT://TODO
                break;
            case CrowdControlEffect.PACIFY:
            case CrowdControlEffect.POLYMORPH:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                OnDisarm();
                break;
            case CrowdControlEffect.ROOT:
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                OnRoot();
                break;
            case CrowdControlEffect.SILENCE://TODO: You continue to walk towards the target position if you casted a spell, ground or unit, and you do nothing while you're silenced if you stop moving
                SetCannotUseBasicAbilities(count);
                OnSilence();
                break;
            case CrowdControlEffect.STASIS:
            case CrowdControlEffect.SUPPRESSION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                OnStun();
                break;
            case CrowdControlEffect.SUSPENSION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                OnSuspension();
                break;
        }
    }

    private void RemoveCrowdControlEffectFromUnit(CrowdControlEffect crowdControlEffect)
    {
        int count = -1;
        switch (crowdControlEffect)
        {
            case CrowdControlEffect.CHARM:
            case CrowdControlEffect.FEAR:
            case CrowdControlEffect.FLEE:
            case CrowdControlEffect.KNOCKASIDE:
            case CrowdControlEffect.KNOCKBACK:
            case CrowdControlEffect.KNOCKUP:
            case CrowdControlEffect.PULL:
            case CrowdControlEffect.SLEEP:
            case CrowdControlEffect.STUN:
            case CrowdControlEffect.SUSPENSION:
            case CrowdControlEffect.TAUNT:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case CrowdControlEffect.STASIS:
            case CrowdControlEffect.SUPPRESSION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseSummonerAbilities(count);
                break;
            case CrowdControlEffect.PACIFY:
            case CrowdControlEffect.POLYMORPH:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                break;
            case CrowdControlEffect.CRIPPLE:
            case CrowdControlEffect.DISRUPT:
            case CrowdControlEffect.DROWSY:
            case CrowdControlEffect.KNOCKDOWN:
            case CrowdControlEffect.SLOW:
                break;
            case CrowdControlEffect.BLIND:
                SetIsBlinded(count);
                break;
            case CrowdControlEffect.DISARM:
                SetCannotUseBasicAttacks(count);
                break;
            case CrowdControlEffect.ENTANGLE:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffect.GROUND:
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffect.NEARSIGHT://TODO
                break;
            case CrowdControlEffect.ROOT:
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                break;
            case CrowdControlEffect.SILENCE:
                SetCannotUseBasicAbilities(count);
                break;
        }
    }

    protected abstract void OnDisarm();
    protected abstract void OnDisrupt();
    protected abstract void OnEntangle();
    protected abstract void OnForcedAction();
    protected abstract void OnKnockDown();
    protected abstract void OnKnockUp();
    protected abstract void OnRoot();
    protected abstract void OnSilence();
    protected abstract void OnStun();
    protected abstract void OnSuspension();

    protected virtual void SetCannotUseBasicAbilities(int count)
    {
        blockBasicAbilitiesCount += count;
    }

    protected virtual void SetCannotUseBasicAttacks(int count)
    {
        blockBasicAttacksCount += count;
    }

    protected virtual void SetCannotUseMovementAbilities(int count)
    {
        blockMovementAbilitiesCount += count;
    }

    protected virtual void SetCannotUseSummonerAbilities(int count)
    {
        blockSummonerAbilitiesCount += count;
    }

    protected void SetCannotUseMovement(int count)
    {
        blockMovementCount += count;
    }

    protected void SetIsBlinded(int count)
    {
        isBlindedCount += count;
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
            buffCrowdControlEffect != CrowdControlEffect.SUPPRESSION &&
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
