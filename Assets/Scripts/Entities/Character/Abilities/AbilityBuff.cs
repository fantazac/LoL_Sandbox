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

    public virtual void ApplyBuffToAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        EntitiesAffectedByBuff.Add(affectedEntity);
    }

    public virtual void RemoveBuffFromAffectedEntity(Entity affectedEntity, float buffValue, int currentStacks)
    {
        EntitiesAffectedByBuff.Remove(affectedEntity);
        if (OnAbilityBuffRemoved != null)
        {
            OnAbilityBuffRemoved(affectedEntity);
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

    public virtual void AddNewBuffToAffectedEntity(Entity affectedEntity)
    {
        SetupBuff(isADebuff ? affectedEntity.EntityBuffManager.GetDebuff(this) : affectedEntity.EntityBuffManager.GetBuff(this), affectedEntity);
    }

    protected virtual void SetupBuff(Buff buff, Entity affectedEntity)
    {
        if (buff == null)
        {
            affectedEntity.EntityBuffManager.ApplyBuff(CreateNewBuff(affectedEntity), buffSprite, isADebuff);
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

    public void ConsumeBuff(Entity affectedEntity)
    {
        Consume(isADebuff ? affectedEntity.EntityBuffManager.GetDebuff(this) : affectedEntity.EntityBuffManager.GetBuff(this));
    }

    protected void Consume(Buff buff)
    {
        if (buff != null)
        {
            character.EntityBuffManager.ConsumeBuff(buff, isADebuff);
        }
    }

    protected abstract Buff CreateNewBuff(Entity affectedEntity);
}
