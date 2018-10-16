using UnityEngine;

public abstract class UnitTargetedProjectile : UnitTargeted
{
    protected string projectilePrefabPath;
    protected GameObject projectilePrefab;

    protected override void LoadPrefabs()
    {
        projectilePrefab = Resources.Load<GameObject>(projectilePrefabPath);
    }

    protected override void OnAbilityEffectHit(AbilityEffect projectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        float damage = GetAbilityDamage(entityHit, isACriticalStrike);
        entityHit.EntityStats.Health.Reduce(damage);
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
        AbilityHit(entityHit, damage);
    }
}
