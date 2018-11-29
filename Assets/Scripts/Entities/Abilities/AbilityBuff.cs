using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBuff : MonoBehaviour
{
    protected string buffName;

    protected Character character;

    protected bool isADebuff;
    protected bool showBuffValueOnUI;

    protected float buffDuration;
    protected float buffDurationPerLevel;
    protected int buffMaximumStacks;
    protected float buffFlatValue;
    protected float buffFlatValuePerLevel;
    protected float buffPercentValue;
    protected float buffPercentValuePerLevel;
    protected CrowdControlEffect buffCrowdControlEffect;

    protected Sprite buffSprite;
    protected string buffSpritePath;

    protected Vector3 normalizedVector;

    public List<Entity> EntitiesAffectedByBuff { get; protected set; }

    public delegate void OnAbilityBuffRemovedHandler(Entity entityHit);
    public event OnAbilityBuffRemovedHandler OnAbilityBuffRemoved;

    protected AbilityBuff()
    {
        EntitiesAffectedByBuff = new List<Entity>();
        SetSpritePaths();
    }

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
        buffSprite = Resources.Load<Sprite>(buffSpritePath);
    }

    protected virtual void Start() { }

    protected abstract void SetSpritePaths();

    public void ApplyBuffToAffectedEntity(Entity affectedEntity, Buff buff)
    {
        ApplyBuffEffect(affectedEntity, buff);

        EntitiesAffectedByBuff.Add(affectedEntity);
    }

    public void RemoveBuffFromAffectedEntity(Entity affectedEntity, Buff buff)
    {
        RemoveBuffEffect(affectedEntity, buff);

        EntitiesAffectedByBuff.Remove(affectedEntity);
        if (OnAbilityBuffRemoved != null)
        {
            OnAbilityBuffRemoved(affectedEntity);
        }
    }

    protected virtual void ApplyBuffEffect(Entity affectedEntity, Buff buff) { }
    protected virtual void RemoveBuffEffect(Entity affectedEntity, Buff buff) { }

    public virtual void UpdateBuffOnAffectedEntities(float oldFlatValue, float newFlatValue, float oldPercentValue, float newPercentValue) { }

    public void LevelUp()
    {
        float oldFlatValue = buffFlatValue;
        float oldPercentValue = buffPercentValue;

        buffFlatValue += buffFlatValuePerLevel;
        buffPercentValue += buffPercentValuePerLevel;
        buffDuration += buffDurationPerLevel;

        UpdateBuffOnAffectedEntities(oldFlatValue, buffFlatValue, oldPercentValue, buffPercentValue);
    }

    public virtual void AddNewBuffToAffectedEntity(Entity affectedEntity)
    {
        SetupBuff(isADebuff ? affectedEntity.EntityBuffManager.GetDebuff(this) : affectedEntity.EntityBuffManager.GetBuff(this), affectedEntity);
    }

    public virtual void AddNewBuffToAffectedEntities(List<Entity> previousEntitiesHit, List<Entity> entitiesHit)
    {
        foreach (Entity previousEntityHit in previousEntitiesHit)
        {
            if (!entitiesHit.Contains(previousEntityHit))
            {
                ConsumeBuff(previousEntityHit);
            }
        }
        foreach (Entity entityHit in entitiesHit)
        {
            SetupBuff(isADebuff ? entityHit.EntityBuffManager.GetDebuff(this) : entityHit.EntityBuffManager.GetBuff(this), entityHit);
        }
    }

    protected virtual void SetupBuff(Buff buff, Entity affectedEntity)
    {
        if (buff == null)
        {
            buff = CreateNewBuff(affectedEntity);
            affectedEntity.EntityBuffManager.ApplyBuff(buff, buffSprite, isADebuff);
            if (showBuffValueOnUI)
            {
                buff.SetBuffValueOnUI();
            }
        }
        else if (buffDuration > 0)
        {
            if (buffMaximumStacks > 0)
            {
                buff.IncreaseCurrentStacks();
            }
            buff.ResetDurationRemaining();
        }
    }

    public virtual void ConsumeBuff(Entity affectedEntity)
    {
        Consume(affectedEntity, isADebuff ? affectedEntity.EntityBuffManager.GetDebuff(this) : affectedEntity.EntityBuffManager.GetBuff(this));
    }

    protected void Consume(Entity affectedEntity, Buff buff)
    {
        if (buff != null)
        {
            affectedEntity.EntityBuffManager.ConsumeBuff(buff, isADebuff);
        }
    }

    public void SetNormalizedVector(Vector3 casterPositionOnCast, Vector3 targetPositionOnCast)
    {
        normalizedVector = Vector3.Normalize(targetPositionOnCast - casterPositionOnCast);
    }

    protected float GetBuffDuration(Entity affectedEntity)
    {
        if (affectedEntity.EntityStatusManager.IsAnAirborneEffect(buffCrowdControlEffect))
        {
            return buffDuration * (1 + affectedEntity.EntityStatsManager.Tenacity.GetPercentMalus());
        }
        else if (affectedEntity.EntityStatusManager.CanReduceCrowdControlDuration(buffCrowdControlEffect))
        {
            return buffDuration * (1 - affectedEntity.EntityStatsManager.Tenacity.GetTotal());
        }
        return buffDuration;
    }

    protected abstract Buff CreateNewBuff(Entity affectedEntity);
}
