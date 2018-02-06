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
    public event OnRemoveEffect OnRemove;


    public Buff(float durationInSeconds)
    {
        this.durationInSeconds = durationInSeconds;
        stopTime = 0;
    }

    public virtual void ApplyEffectTo(Entity entity)
    {
        if (OnApply != null)
        {
            OnApply(entity);
        }
        stopTime = Time.time + durationInSeconds;
    }

    public virtual void Update(Entity entity)
    {
        if (OnUpdate != null)
        {
            OnUpdate(entity);
        }
    }

    public virtual void RemoveEffectFrom(Entity entity)
    {
        if (OnRemove != null)
        {
            OnRemove(entity);
        }
    }

    public virtual bool HasExpired()
    {
        return Time.time > stopTime;
    }
}
