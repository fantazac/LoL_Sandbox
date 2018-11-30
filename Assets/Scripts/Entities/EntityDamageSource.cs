using UnityEngine;

public class EntityDamageSource : MonoBehaviour
{
    protected DamageType damageType;

    public delegate void OnKilledEntityHandler(EntityDamageSource damageSource, Entity killedEntity);
    public event OnKilledEntityHandler OnKilledEntity;

    protected void DamageEntity(Entity entityToDamage, float damage)
    {
        DamageEntity(entityToDamage, damageType, damage);
    }

    protected void DamageEntity(Entity entityToDamage, DamageType damageType, float damage)
    {
        entityToDamage.EntityStatsManager.ReduceHealth(this, damageType, damage);
    }

    public void KilledEntity(Entity killedEntity)
    {
        if (OnKilledEntity != null)
        {
            OnKilledEntity(this, killedEntity);
        }
    }
}
