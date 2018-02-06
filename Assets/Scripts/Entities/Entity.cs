using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityType EntityType { get; protected set; }
    public EntityTeam Team { get; protected set; }

    private List<Buff> buffs;

    public int EntityId { get; protected set; }

    protected virtual void Start()
    {
        buffs = new List<Buff>();
    }

    public virtual void Update()
    {
        RemoveExpiredBuffs();
        UpdateActiveBuffs();
    }

    private void RemoveExpiredBuffs()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            if (buffs[i].HasExpired())
            {
                buffs[i].RemoveEffectFrom(this);
                buffs.RemoveAt(i);
            }
        }
    }

    private void UpdateActiveBuffs()
    {
        foreach (Buff buff in buffs)
        {
            buff.Update(this);
        }
    }

    public virtual void ApplyBuff(Buff buff)
    {
        buffs.Add(buff);
        buff.ApplyEffectTo(this);
    }
}
