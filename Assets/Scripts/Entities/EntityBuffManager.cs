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

    private void UpdateBuff(Buff buff, List<Buff> buffs, int position, float deltaTime, BuffUIManager buffUIManager)
    {
        bool hasBuffUIManager = buffUIManager != null;
        if (buff.HasDuration)
        {
            buff.ReduceDurationRemaining(deltaTime);
            if (hasBuffUIManager)
            {
                buffUIManager.UpdateBuffDuration(buff, buff.DurationForUI, buff.DurationRemaining, buff.CurrentStacks);
            }
            if (buff.HasExpired())
            {
                if (hasBuffUIManager)
                {
                    buffUIManager.RemoveExpiredBuff(buff);
                }
                buff.RemoveBuff();
                buffs.RemoveAt(position);
            }
        }
    }

    public Buff GetBuff(Ability sourceAbility)
    {
        Buff buff = null;

        foreach (Buff activeBuff in buffs)
        {
            if (activeBuff.SourceAbility == sourceAbility)
            {
                buff = activeBuff;
                break;
            }
        }

        return buff;
    }

    public Buff GetDebuff(Ability sourceAbility)
    {
        Buff debuff = null;

        foreach (Buff activeDebuff in debuffs)
        {
            if (activeDebuff.SourceAbility == sourceAbility)
            {
                debuff = activeDebuff;
                break;
            }
        }

        return debuff;
    }

    public Buff GetBuffOfSameType(Ability sourceAbility)
    {
        Buff buff = null;

        foreach (Buff activeBuff in buffs)
        {
            if (activeBuff.SourceAbility.GetType() == sourceAbility.GetType())
            {
                buff = activeBuff;
                break;
            }
        }

        return buff;
    }

    public Buff GetDebuffOfSameType(Ability sourceAbility)
    {
        Buff debuff = null;

        foreach (Buff activeDebuff in debuffs)
        {
            if (activeDebuff.SourceAbility.GetType() == sourceAbility.GetType())
            {
                debuff = activeDebuff;
                break;
            }
        }

        return debuff;
    }

    public void ApplyBuff(Buff buff, Sprite buffSprite)
    {
        buffs.Add(buff);
        buff.ApplyBuff();
        if (buffUIManager != null)
        {
            buffUIManager.SetNewBuff(buff, buffSprite);
        }
    }

    public void ApplyDebuff(Buff debuff, Sprite debuffSprite)
    {
        debuffs.Add(debuff);
        debuff.ApplyBuff();
        if (debuffUIManager != null)
        {
            debuffUIManager.SetNewBuff(debuff, debuffSprite);
        }
    }
}
