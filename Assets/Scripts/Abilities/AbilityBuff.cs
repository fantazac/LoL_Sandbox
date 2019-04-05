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
    protected StatusEffect buffStatusEffect;

    protected Sprite buffSprite;
    protected string buffSpritePath;

    protected Vector3 normalizedVector;

    protected readonly List<Unit> unitsAffectedByBuff;

    public delegate void OnAbilityBuffRemovedHandler(Unit affectedUnit);
    public event OnAbilityBuffRemovedHandler OnAbilityBuffRemoved;

    protected AbilityBuff()
    {
        unitsAffectedByBuff = new List<Unit>();
    }

    protected void Awake()
    {
        SetSpritePaths();
        
        character = GetComponent<Character>();
        buffSprite = Resources.Load<Sprite>(buffSpritePath);
    }

    protected virtual void Start() { }

    protected abstract void SetSpritePaths();

    public void ApplyBuffToAffectedUnit(Unit affectedUnit, Buff buff)
    {
        ApplyBuffEffect(affectedUnit, buff);

        unitsAffectedByBuff.Add(affectedUnit);
    }

    public void RemoveBuffFromAffectedUnit(Unit affectedUnit, Buff buff)
    {
        RemoveBuffEffect(affectedUnit, buff);

        unitsAffectedByBuff.Remove(affectedUnit);
        
        OnAbilityBuffRemoved?.Invoke(affectedUnit);
    }

    protected virtual void ApplyBuffEffect(Unit affectedUnit, Buff buff) { }
    protected virtual void RemoveBuffEffect(Unit affectedUnit, Buff buff) { }

    protected virtual void UpdateBuffOnAffectedUnits(float oldFlatValue, float newFlatValue, float oldPercentValue, float newPercentValue) { }

    public void LevelUp()
    {
        float oldFlatValue = buffFlatValue;
        float oldPercentValue = buffPercentValue;

        buffFlatValue += buffFlatValuePerLevel;
        buffPercentValue += buffPercentValuePerLevel;
        buffDuration += buffDurationPerLevel;

        UpdateBuffOnAffectedUnits(oldFlatValue, buffFlatValue, oldPercentValue, buffPercentValue);
    }

    public virtual void AddNewBuffToAffectedUnit(Unit affectedUnit)
    {
        SetupBuff(isADebuff ? affectedUnit.BuffManager.GetDebuff(this) : affectedUnit.BuffManager.GetBuff(this), affectedUnit);
    }

    public void AddNewBuffToAffectedUnits(List<Unit> previouslyAffectedUnits, List<Unit> affectedUnits)
    {
        foreach (Unit previouslyAffectedUnit in previouslyAffectedUnits)
        {
            if (!affectedUnits.Contains(previouslyAffectedUnit))
            {
                ConsumeBuff(previouslyAffectedUnit);
            }
        }
        foreach (Unit affectedUnit in affectedUnits)
        {
            SetupBuff(isADebuff ? affectedUnit.BuffManager.GetDebuff(this) : affectedUnit.BuffManager.GetBuff(this), affectedUnit);
        }
    }

    protected virtual void SetupBuff(Buff buff, Unit affectedUnit)
    {
        if (buff == null)
        {
            buff = CreateNewBuff(affectedUnit);
            affectedUnit.BuffManager.ApplyBuff(buff, buffSprite, isADebuff);
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

    public void ConsumeBuff(Unit affectedUnit)
    {
        Consume(affectedUnit, isADebuff ? affectedUnit.BuffManager.GetDebuff(this) : affectedUnit.BuffManager.GetBuff(this));
    }

    protected void Consume(Unit affectedUnit, Buff buff)
    {
        if (buff != null)
        {
            affectedUnit.BuffManager.ConsumeBuff(buff, isADebuff);
        }
    }

    public void SetNormalizedVector(Vector3 casterPositionOnCast, Vector3 targetPositionOnCast)
    {
        normalizedVector = Vector3.Normalize(targetPositionOnCast - casterPositionOnCast);
    }

    protected float GetBuffDuration(Unit affectedUnit)
    {
        if (affectedUnit.StatusManager.IsAnAirborneEffect(buffStatusEffect))
        {
            return buffDuration * (1 + affectedUnit.StatsManager.Tenacity.GetPercentMalus());
        }
        else if (affectedUnit.StatusManager.CanReduceCrowdControlDuration(buffStatusEffect))
        {
            return buffDuration * (1 - affectedUnit.StatsManager.Tenacity.GetTotal());
        }
        return buffDuration;
    }

    protected abstract Buff CreateNewBuff(Unit affectedUnit);

    public Sprite GetBuffSprite()
    {
        return buffSprite;
    }
}
