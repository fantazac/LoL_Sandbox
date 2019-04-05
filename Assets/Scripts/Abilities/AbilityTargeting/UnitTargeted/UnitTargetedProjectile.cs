using UnityEngine;

public abstract class UnitTargetedProjectile : UnitTargeted
{
    protected string projectilePrefabPath;
    protected GameObject projectilePrefab;

    protected override void LoadPrefabs()
    {
        projectilePrefab = Resources.Load<GameObject>(projectilePrefabPath);
    }

    protected virtual void OnProjectileHit(Projectile projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
        
        float abilityDamage = GetAbilityDamage(unitHit, isACriticalStrike);
        DamageUnit(unitHit, abilityDamage);
        AbilityHit(unitHit, abilityDamage);
    }
}
