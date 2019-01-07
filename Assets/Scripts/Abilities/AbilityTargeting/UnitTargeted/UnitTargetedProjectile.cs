using UnityEngine;

public abstract class UnitTargetedProjectile : UnitTargeted
{
    protected string projectilePrefabPath;
    protected GameObject projectilePrefab;

    protected override void LoadPrefabs()
    {
        projectilePrefab = Resources.Load<GameObject>(projectilePrefabPath);
    }

    protected override void OnAbilityEffectHit(AbilityEffect projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        float damage = GetAbilityDamage(unitHit, isACriticalStrike);
        DamageUnit(unitHit, damage);
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
        AbilityHit(unitHit, damage);
    }
}
