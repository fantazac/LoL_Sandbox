using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUpdatingWithDelay : Buff // TODO: currently only works if buffDurationPostDelay = 0
{
    private float buffDurationPostDelay;
    private float buffValuePostDelay;

    public BuffUpdatingWithDelay(AbilityBuff sourceAbilityBuff, Entity affectedEntity, float buffValue, float buffDuration, float buffValuePostDelay, float buffDurationPostDelay,
        int maximumStacks, float stackDecayingDelay)
        : base(sourceAbilityBuff, affectedEntity, buffValue, buffDuration, maximumStacks, stackDecayingDelay)
    {
        this.buffDurationPostDelay = buffDurationPostDelay;
        this.buffValuePostDelay = buffValuePostDelay;
    }

    public BuffUpdatingWithDelay(AbilityBuff sourceAbilityBuff, Entity affectedEntity, float buffValue, float buffDuration, float buffValuePostDelay) :
        this(sourceAbilityBuff, affectedEntity, buffValue, buffDuration, buffValuePostDelay, 0, 0, 0)
    { }

    public BuffUpdatingWithDelay(AbilityBuff sourceAbilityBuff, Entity affectedEntity, float buffValue, float buffDuration, float buffValuePostDelay, float buffDurationPostDelay) :
        this(sourceAbilityBuff, affectedEntity, buffValue, buffDuration, buffValuePostDelay, buffDurationPostDelay, 0, 0)
    { }

    public BuffUpdatingWithDelay(AbilityBuff sourceAbilityBuff, Entity affectedEntity, float buffValue, float buffDuration, float buffValuePostDelay, float buffDurationPostDelay, int maximumStacks) :
        this(sourceAbilityBuff, affectedEntity, buffValue, buffDuration, buffValuePostDelay, buffDurationPostDelay, maximumStacks, 0)
    { }

    protected void BuffPostDelay()
    {
        float oldBuffValue = BuffValue;
        BuffValue = buffValuePostDelay;
        Duration = buffDurationPostDelay;
        DurationForUI = buffDurationPostDelay;
        DurationRemaining = buffDurationPostDelay;

        InitProperties();
        UpdateBuffOnAffectedEntity(oldBuffValue, BuffValue);

        SetBuffValueOnUI();
    }

    public void SetBuffValuePostDelay(float newBuffValuePostDelay)
    {
        buffValuePostDelay = newBuffValuePostDelay;
    }

    public void SetBuffValueAndBuffDurationPostDelay(float newBuffValuePostDelay, float newBuffDurationPostDelay)
    {
        buffValuePostDelay = newBuffValuePostDelay;
        buffDurationPostDelay = newBuffDurationPostDelay;
    }

    public override void ConsumeBuff()
    {
        BuffPostDelay();
    }

    private void UpdateBuffOnAffectedEntity(float oldValue, float newValue)
    {
        SourceAbilityBuff.RemoveBuffFromAffectedEntity(affectedEntity, oldValue, CurrentStacks);
        SourceAbilityBuff.ApplyBuffToAffectedEntity(affectedEntity, newValue, CurrentStacks);
    }
}
