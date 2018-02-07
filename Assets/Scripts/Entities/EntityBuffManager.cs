using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBuffManager : MonoBehaviour
{
    private List<Buff> buffs;
    private List<Buff> debuffs;

    protected EntityBuffManager()
    {
        buffs = new List<Buff>();
        debuffs = new List<Buff>();
    }

    private void Update()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            if (buffs[i].HasDuration())
            {
                buffs[i].ReduceDurationRemaining(Time.deltaTime);
                if (buffs[i].HasExpired())
                {
                    buffs[i].RemoveBuff();
                    buffs.RemoveAt(i);
                }
            }
        }
        for (int j = debuffs.Count - 1; j >= 0; j--)
        {
            if (debuffs[j].HasDuration())
            {
                debuffs[j].ReduceDurationRemaining(Time.deltaTime);
                if (debuffs[j].HasExpired())
                {
                    debuffs[j].RemoveBuff();
                    debuffs.RemoveAt(j);
                }
            }
        }
    }

    public void ApplyBuff(Buff buff)
    {
        buffs.Add(buff);
    }

    public void ApplyDebuff(Buff debuff)
    {
        debuffs.Add(debuff);
    }
}
