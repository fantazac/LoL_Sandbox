using UnityEngine;

public class Buff
{
    public AbilityBuff SourceAbilityBuff { get; private set; }
    private Entity entityHit;

    public float Duration { get; private set; }
    public float DurationForUI { get; private set; }
    public float DurationRemaining { get; private set; }

    public int BuffValue { get; private set; }
    public int CurrentStacks { get; private set; }
    public int MaximumStacks { get; private set; }
    public float StackDecayingDuration { get; private set; }

    public bool HasBeenConsumed { get; private set; }
    public bool HasDuration { get; private set; }
    public bool HasStacks { get; private set; }
    public bool HasValueToSet { get; private set; }

    public Buff(AbilityBuff sourceAbilityBuff, Entity entityHit, float buffDuration, int maximumStacks, float stackDecayingDuration)
    {
        SourceAbilityBuff = sourceAbilityBuff;
        this.entityHit = entityHit;
        Duration = buffDuration;
        DurationForUI = buffDuration;
        DurationRemaining = buffDuration;
        MaximumStacks = maximumStacks;
        StackDecayingDuration = stackDecayingDuration;

        if (maximumStacks > 0)
        {
            CurrentStacks = 1;
        }

        HasDuration = buffDuration > 0;
        HasStacks = maximumStacks > 0;
    }

    public Buff(AbilityBuff sourceAbilityBuff, Entity entityHit) : this(sourceAbilityBuff, entityHit, 0, 0, 0) { }
    public Buff(AbilityBuff sourceAbilityBuff, Entity entityHit, float duration) : this(sourceAbilityBuff, entityHit, duration, 0, 0) { }
    public Buff(AbilityBuff sourceAbilityBuff, Entity entityHit, float duration, int maximumStacks) : this(sourceAbilityBuff, entityHit, duration, maximumStacks, 0) { }

    public void SetBuffValue(int buffValue)
    {
        BuffValue = buffValue;
        HasValueToSet = BuffValue > 0;
    }

    public void ValueWasSet()
    {
        HasValueToSet = false;
    }

    public void ApplyBuff()
    {
        if (!HasStacks && BuffValue > 0)
        {
            SourceAbilityBuff.ApplyBuffToEntityHit(entityHit, BuffValue);
        }
        else
        {
            SourceAbilityBuff.ApplyBuffToEntityHit(entityHit, CurrentStacks);
        }
    }

    public void RemoveBuff()
    {
        if (!HasStacks && BuffValue > 0)
        {
            SourceAbilityBuff.RemoveBuffFromEntityHit(entityHit, BuffValue);
        }
        else
        {
            SourceAbilityBuff.RemoveBuffFromEntityHit(entityHit, CurrentStacks);
        }
    }

    public void ConsumeBuff()
    {
        CurrentStacks = 0;
        DurationRemaining = 0;
        HasBeenConsumed = true;
    }

    public void ReduceDurationRemaining(float frameDuration)
    {
        DurationRemaining -= frameDuration;
        if (DurationRemaining <= 0)
        {
            if (!HasExpired())
            {
                RemoveBuff();
                if (StackDecayingDuration > 0)
                {
                    CurrentStacks--;
                    ApplyBuff();
                    DurationRemaining = StackDecayingDuration;
                    DurationForUI = StackDecayingDuration;
                }
                else
                {
                    ConsumeBuff();
                }
            }
            else
            {
                ConsumeBuff();
            }
        }
    }

    public void ResetDurationRemaining()
    {
        DurationForUI = Duration;
        DurationRemaining = Duration;
    }

    public void IncreaseCurrentStacks()
    {
        if (CurrentStacks < MaximumStacks)
        {
            SourceAbilityBuff.RemoveBuffFromEntityHit(entityHit, CurrentStacks);
            SourceAbilityBuff.ApplyBuffToEntityHit(entityHit, ++CurrentStacks);
        }
    }

    public bool HasExpired()
    {
        if (MaximumStacks == 0)
        {
            return DurationRemaining <= 0;
        }
        else
        {
            return CurrentStacks == 0;
        }
    }
}
