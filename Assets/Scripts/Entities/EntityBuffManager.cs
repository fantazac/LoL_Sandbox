using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityBuffManager : MonoBehaviour
{
    private List<Buff> buffs;
    private List<Buff> debuffs;

    private BuffUIManager buffUIManager;
    private BuffUIManager debuffUIManager;

    protected EntityBuffManager()
    {
        buffs = new List<Buff>();
        debuffs = new List<Buff>();
    }

    public void SetUIManagers(BuffUIManager buffUIManager, BuffUIManager debuffUIManager)
    {
        this.buffUIManager = buffUIManager;
        this.debuffUIManager = debuffUIManager;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            UpdateBuff(buffs[i], buffs, i, deltaTime, buffUIManager);
        }
        for (int j = debuffs.Count - 1; j >= 0; j--)
        {
            UpdateBuff(debuffs[j], debuffs, j, deltaTime, debuffUIManager);
        }
    }

    private void UpdateBuff(Buff buff, List<Buff> buffsList, int position, float deltaTime, BuffUIManager selectedUIManager)
    {
        bool hasBuffUIManager = selectedUIManager != null;
        if (buff.HasDuration)
        {
            buff.ReduceDurationRemaining(deltaTime);
            if (hasBuffUIManager)
            {
                selectedUIManager.UpdateBuffDuration(buff, buff.DurationForUI, buff.DurationRemaining, buff.CurrentStacks);
            }
        }
        if (buff.HasValueToSet)
        {
            if (hasBuffUIManager)
            {
                selectedUIManager.UpdateBuffValue(buff, (int)buff.BuffValue);
            }
            buff.ValueWasSet();
        }
        if (buff.HasBeenConsumed)
        {
            ConsumeBuff(buff, buffsList, position, selectedUIManager);
        }
    }

    public void ConsumeBuff(Buff buff, bool isADebuff)
    {
        int position;
        List<Buff> buffsList = isADebuff ? debuffs : buffs;
        for (position = buffsList.Count - 1; position >= 0; position--)
        {
            if (buffsList[position] == buff)
            {
                ConsumeBuff(buff, buffsList, position, isADebuff ? debuffUIManager : buffUIManager);
            }
        }
    }

    public void ConsumeBuff(Buff buff, List<Buff> buffsList, int position, BuffUIManager selectedUIManager)
    {
        if (selectedUIManager)
        {
            selectedUIManager.RemoveExpiredBuff(buff);
        }
        buff.RemoveBuff();
        buffsList.RemoveAt(position);
    }

    public Buff GetBuff(AbilityBuff sourceAbilityBuff)
    {
        Buff buff = null;

        foreach (Buff activeBuff in buffs)
        {
            if (activeBuff.SourceAbilityBuff == sourceAbilityBuff)
            {
                buff = activeBuff;
                break;
            }
        }

        return buff;
    }

    public Buff GetDebuff(AbilityBuff sourceAbilityBuff)
    {
        Buff debuff = null;

        foreach (Buff activeDebuff in debuffs)
        {
            if (activeDebuff.SourceAbilityBuff == sourceAbilityBuff)
            {
                debuff = activeDebuff;
                break;
            }
        }

        return debuff;
    }

    public Buff GetBuffOfSameType(AbilityBuff sourceAbilityBuff)
    {
        Buff buff = null;

        foreach (Buff activeBuff in buffs)
        {
            if (activeBuff.SourceAbilityBuff.GetType() == sourceAbilityBuff.GetType())
            {
                buff = activeBuff;
                break;
            }
        }

        return buff;
    }

    public Buff GetDebuffOfSameType(AbilityBuff sourceAbilityBuff)
    {
        Buff debuff = null;

        foreach (Buff activeDebuff in debuffs)
        {
            if (activeDebuff.SourceAbilityBuff.GetType() == sourceAbilityBuff.GetType())
            {
                debuff = activeDebuff;
                break;
            }
        }

        return debuff;
    }

    public void ApplyBuff(Buff buff, Sprite buffSprite, bool isADebuff)
    {
        if (isADebuff)
        {
            debuffs.Add(buff);
            buff.ApplyBuff();
            if (debuffUIManager != null)
            {
                debuffUIManager.SetNewBuff(buff, buffSprite);
            }
        }
        else
        {
            buffs.Add(buff);
            buff.ApplyBuff();
            if (buffUIManager != null)
            {
                buffUIManager.SetNewBuff(buff, buffSprite);
            }
        }

    }
}
