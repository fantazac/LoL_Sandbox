using UnityEngine;

public class Buff
{
    public AbilityBuff SourceAbilityBuff { get; private set; }
    private Entity affectedEntity;

    public float Duration { get; private set; }
    public float DurationForUI { get; private set; }
    public float DurationRemaining { get; private set; }

    public float BuffValue { get; private set; }
    public int CurrentStacks { get; private set; }
    public int MaximumStacks { get; private set; }
    public float StackDecayingDelay { get; private set; }

    public bool HasBeenConsumed { get; private set; }
    public bool HasDuration { get; private set; }
    public bool HasStacks { get; private set; }
    public bool HasValueToSet { get; private set; }

    //With stacks that decay 1 by 1 at a certain delay
    public Buff(AbilityBuff sourceAbilityBuff, Entity affectedEntity, float buffValue, float buffDuration, int maximumStacks, float stackDecayingDelay)
    {
        SourceAbilityBuff = sourceAbilityBuff;
        this.affectedEntity = affectedEntity;
        BuffValue = buffValue;
        Duration = buffDuration;
        DurationForUI = buffDuration;
        DurationRemaining = buffDuration;
        MaximumStacks = maximumStacks;
        StackDecayingDelay = stackDecayingDelay;

        CurrentStacks = maximumStacks > 0 ? 1 : 0;
        HasDuration = buffDuration > 0;
        HasStacks = maximumStacks > 0;
    }

    //No buff value, no duration, no stacks
    public Buff(AbilityBuff sourceAbilityBuff, Entity affectedEntity) : this(sourceAbilityBuff, affectedEntity, 0, 0, 0, 0) { }

    //No duration, no stacks
    public Buff(AbilityBuff sourceAbilityBuff, Entity affectedEntity, float buffValue) : this(sourceAbilityBuff, affectedEntity, buffValue, 0, 0, 0) { }

    //No stacks (EzrealW, LucianP)
    public Buff(AbilityBuff sourceAbilityBuff, Entity affectedEntity, float buffValue, float duration) : this(sourceAbilityBuff, affectedEntity, buffValue, duration, 0, 0) { }

    //With stacks that disappear instantly if the buff expires (EzrealP)
    public Buff(AbilityBuff sourceAbilityBuff, Entity affectedEntity, float buffValue, float duration, int maximumStacks) : this(sourceAbilityBuff, affectedEntity, buffValue, duration, maximumStacks, 0) { }

    public void SetBuffValue(int buffValue)
    {
        BuffValue = buffValue;
    }

    public void SetBuffValueOnUI()
    {
        HasValueToSet = true;
    }

    public void SetBuffValueOnUI(int buffValue)
    {
        SetBuffValue(buffValue);
        SetBuffValueOnUI();
    }

    public void ValueWasSet()
    {
        HasValueToSet = false;
    }

    public void ApplyBuff()
    {
        SourceAbilityBuff.ApplyBuffToAffectedEntity(affectedEntity, BuffValue, CurrentStacks);
    }

    public void RemoveBuff()
    {
        SourceAbilityBuff.RemoveBuffFromAffectedEntity(affectedEntity, BuffValue, CurrentStacks);
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
                if (StackDecayingDelay > 0)
                {
                    CurrentStacks--;
                    ApplyBuff();
                    DurationRemaining = StackDecayingDelay;
                    DurationForUI = StackDecayingDelay;
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
            SourceAbilityBuff.RemoveBuffFromAffectedEntity(affectedEntity, BuffValue, CurrentStacks);
            SourceAbilityBuff.ApplyBuffToAffectedEntity(affectedEntity, BuffValue, ++CurrentStacks);
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
