using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBuff : MonoBehaviour
{
    protected string buffName;

    protected Character character;

    protected bool isADebuff;

    protected float buffDuration;
    protected int buffMaximumStacks;
    protected float buffFlatBonus;
    protected float buffFlatBonusPerLevel;
    protected float buffPercentBonus;
    protected float buffPercentBonusPerLevel;

    protected Sprite buffSprite;
    protected string buffSpritePath;

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

    protected void OnDestroy()
    {

    }

    public virtual void ApplyBuffToEntityHit(Entity entityHit, int currentStacksOrBuffValue)
    {
        EntitiesAffectedByBuff.Add(entityHit);
    }

    public virtual void RemoveBuffFromEntityHit(Entity entityHit, int currentStacksOrBuffValue)
    {
        EntitiesAffectedByBuff.Remove(entityHit);
        if (OnAbilityBuffRemoved != null)
        {
            OnAbilityBuffRemoved(entityHit);
        }
    }

    public virtual void UpdateBuffOnAffectedEntities(float oldFlatValue, float newFlatValue, float oldPercentValue, float newPercentValue) { }

    public void LevelUp()
    {
        float oldFlatValue = buffFlatBonus;
        float oldPercentValue = buffPercentBonus;

        buffFlatBonus += buffFlatBonusPerLevel;
        buffPercentBonus += buffPercentBonusPerLevel;

        UpdateBuffOnAffectedEntities(oldFlatValue, buffFlatBonus, oldPercentValue, buffPercentBonus);
    }

    public virtual void AddNewBuffToEntityHit(Entity entityHit)
    {
        Buff buff = entityHit.EntityBuffManager.GetBuff(this);
        if (buff == null)
        {
            buff = new Buff(this, entityHit, buffDuration);
            entityHit.EntityBuffManager.ApplyBuff(buff, buffSprite);
        }
        else if (buffDuration > 0)
        {
            buff.ResetDurationRemaining();
        }
    }

    public virtual void AddNewDebuffToEntityHit(Entity entityHit)
    {
        Buff debuff = entityHit.EntityBuffManager.GetDebuff(this);
        if (debuff == null)
        {
            debuff = new Buff(this, entityHit, buffDuration);
            entityHit.EntityBuffManager.ApplyDebuff(debuff, buffSprite);
        }
        else if (buffDuration > 0)
        {
            debuff.ResetDurationRemaining();
        }
    }

    public void ConsumeBuff(Entity affectedTarget)
    {
        Buff buff = affectedTarget.EntityBuffManager.GetBuff(this);
        if (buff != null)
        {
            buff.ConsumeBuff();
        }
    }

    public void ConsumeDebuff(Entity affectedTarget)
    {
        Buff debuff = affectedTarget.EntityBuffManager.GetDebuff(this);
        if (debuff != null)
        {
            debuff.ConsumeBuff();
        }
    }
}
