using System.Collections.Generic;
using UnityEngine;

public abstract class StatusManager : MonoBehaviour
{
    public List<StatusEffect> StatusEffectsOnCharacter { get; private set; }

    protected int blockBasicAttacksCount;
    protected int blockBasicAbilitiesCount;
    protected int blockMovementCount;
    protected int blockMovementAbilitiesCount;
    protected int blockSummonerAbilitiesCount;
    protected int isBlindedCount;

    public delegate void OnStatusEffectAddedHandler(StatusEffect statusEffect);
    public event OnStatusEffectAddedHandler OnStatusEffectAdded;

    public delegate void OnStatusEffectRemovedHandler(StatusEffect statusEffect);
    public event OnStatusEffectRemovedHandler OnStatusEffectRemoved;

    protected StatusManager()
    {
        StatusEffectsOnCharacter = new List<StatusEffect>();
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        StatusEffectsOnCharacter.Add(statusEffect);
        ApplyStatusEffectToUnit(statusEffect);
        OnStatusEffectAdded?.Invoke(statusEffect);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        StatusEffectsOnCharacter.Remove(statusEffect);
        RemoveStatusEffectFromUnit(statusEffect);
        OnStatusEffectRemoved?.Invoke(statusEffect);
    }

    private void ApplyStatusEffectToUnit(StatusEffect statusEffect)
    {
        int count = 1;
        switch (statusEffect)
        {
            case StatusEffect.BLIND:
                SetIsBlinded(count);
                break;
            case StatusEffect.CHARM:
            case StatusEffect.FEAR:
            case StatusEffect.FLEE:
            case StatusEffect.TAUNT:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                OnForcedAction();
                break;
            case StatusEffect.CRIPPLE:
            case StatusEffect.DROWSY:
            case StatusEffect.SLOW:
                break;
            case StatusEffect.DISARM:
                SetCannotUseBasicAttacks(count);
                OnDisarm();
                break;
            case StatusEffect.DISRUPT:
                OnDisrupt();
                break;
            case StatusEffect.ENTANGLE:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                OnEntangle();
                break;
            case StatusEffect.GROUND:
                SetCannotUseMovementAbilities(count);
                break;
            case StatusEffect.KNOCKASIDE:
            case StatusEffect.PULL:
            case StatusEffect.SLEEP:
            case StatusEffect.STUN:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                OnStun();
                break;
            case StatusEffect.KNOCKBACK:
            case StatusEffect.KNOCKUP:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                OnKnockUp();
                break;
            case StatusEffect.KNOCKDOWN:
                OnKnockDown();
                break;
            case StatusEffect.NEARSIGHT: //TODO
                break;
            case StatusEffect.PACIFY:
            case StatusEffect.POLYMORPH:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                OnDisarm();
                break;
            case StatusEffect.ROOT:
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                OnRoot();
                break;
            case StatusEffect.SILENCE
                : //TODO: You continue to walk towards the target position if you casted a spell, ground or unit, and you do nothing while you're silenced if you stop moving
                SetCannotUseBasicAbilities(count);
                OnSilence();
                break;
            case StatusEffect.STASIS:
            case StatusEffect.SUPPRESSION:
                SetCannotUseSummonerAbilities(count);
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                OnStun();
                break;
            case StatusEffect.SUSPENSION:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                OnSuspension();
                break;
        }
    }

    private void RemoveStatusEffectFromUnit(StatusEffect statusEffect)
    {
        int count = -1;
        switch (statusEffect)
        {
            case StatusEffect.CHARM:
            case StatusEffect.FEAR:
            case StatusEffect.FLEE:
            case StatusEffect.KNOCKASIDE:
            case StatusEffect.KNOCKBACK:
            case StatusEffect.KNOCKUP:
            case StatusEffect.PULL:
            case StatusEffect.SLEEP:
            case StatusEffect.STUN:
            case StatusEffect.SUSPENSION:
            case StatusEffect.TAUNT:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case StatusEffect.STASIS:
            case StatusEffect.SUPPRESSION:
                SetCannotUseSummonerAbilities(count);
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                break;
            case StatusEffect.PACIFY:
            case StatusEffect.POLYMORPH:
                SetCannotUseBasicAbilities(count);
                SetCannotUseBasicAttacks(count);
                break;
            case StatusEffect.CRIPPLE:
            case StatusEffect.DISRUPT:
            case StatusEffect.DROWSY:
            case StatusEffect.KNOCKDOWN:
            case StatusEffect.SLOW:
                break;
            case StatusEffect.BLIND:
                SetIsBlinded(count);
                break;
            case StatusEffect.DISARM:
                SetCannotUseBasicAttacks(count);
                break;
            case StatusEffect.ENTANGLE:
                SetCannotUseBasicAttacks(count);
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                break;
            case StatusEffect.GROUND:
                SetCannotUseMovementAbilities(count);
                break;
            case StatusEffect.NEARSIGHT: //TODO
                break;
            case StatusEffect.ROOT:
                SetCannotUseMovement(count);
                SetCannotUseMovementAbilities(count);
                break;
            case StatusEffect.SILENCE:
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

    public bool CanReduceCrowdControlDuration(StatusEffect buffStatusEffect)
    {
        return buffStatusEffect != StatusEffect.NONE &&
               buffStatusEffect != StatusEffect.SUPPRESSION &&
               buffStatusEffect != StatusEffect.STASIS &&
               !IsAnAirborneEffect(buffStatusEffect);
    }

    public bool IsAnAirborneEffect(StatusEffect buffStatusEffect)
    {
        return buffStatusEffect == StatusEffect.KNOCKASIDE ||
               buffStatusEffect == StatusEffect.KNOCKBACK ||
               buffStatusEffect == StatusEffect.KNOCKUP ||
               buffStatusEffect == StatusEffect.PULL ||
               buffStatusEffect == StatusEffect.SUSPENSION;
    }
}
