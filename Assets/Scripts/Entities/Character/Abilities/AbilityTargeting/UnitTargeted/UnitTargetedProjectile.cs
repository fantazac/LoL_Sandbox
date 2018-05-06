using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitTargetedProjectile : UnitTargeted
{
    protected string projectilePrefabPath;
    protected GameObject projectilePrefab;

    protected override void LoadPrefabs()
    {
        projectilePrefab = Resources.Load<GameObject>(projectilePrefabPath);
    }

    protected override void OnAbilityEffectHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
        AbilityHit();
    }
}
