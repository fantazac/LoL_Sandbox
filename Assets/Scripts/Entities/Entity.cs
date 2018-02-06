using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityType EntityType { get; protected set; }
    public EntityTeam Team { get; protected set; }

    private List<Buff> buffs;

    public int EntityId { get; protected set; }

    protected Entity()
    {
        buffs = new List<Buff>();
    }

    protected virtual void Start() { }

    public virtual void Update() // TODO FIX ME: Should be a coroutine handled by CharacterBuffManager
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
            buff.UpdateBuff(this);
        }
    }

    public virtual void ApplyBuff(Buff buff)
    {
        buffs.Add(buff);
        buff.ApplyEffectTo(this);
    }
}
