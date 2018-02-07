using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                buffUIManager.UpdateBuffDuration(buff, buff.Duration, buff.DurationRemaining, buff.Stacks);
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

    public void ApplyBuff(Buff buff, Sprite buffSprite)
    {
        buffs.Add(buff);
        if (buffUIManager != null)
        {
            buffUIManager.SetNewBuff(buff, buffSprite);
        }
    }

    public void ApplyDebuff(Buff debuff, Sprite debuffSprite)
    {
        debuffs.Add(debuff);
        if (debuffUIManager != null)
        {
            debuffUIManager.SetNewBuff(debuff, debuffSprite);
        }
    }
}
