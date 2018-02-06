using UnityEngine;

public class Buff
{
    private float durationInSeconds;
    private float stopTime;

    public delegate void OnApplyEffect(Entity entity);
    public OnApplyEffect OnApply;

    public delegate void OnUpdateEffect(Entity entity);
    public OnUpdateEffect OnUpdate;

    public delegate void OnRemoveEffect(Entity entity);
    public OnRemoveEffect OnRemove;


    public Buff(float durationInSeconds)
    {
        this.durationInSeconds = durationInSeconds;
        stopTime = 0;
    }

    public virtual void ApplyEffectTo(Entity entity)
    {
        OnApply(entity);
        stopTime = Time.time + durationInSeconds;
    }

    public virtual void Update(Entity entity)
    {
        OnUpdate(entity);
    }

    public virtual void RemoveEffectFrom(Entity entity)
    {
        OnRemove(entity);
    }

    public virtual bool HasExpired()
    {
        return Time.time > stopTime;
    }
}
