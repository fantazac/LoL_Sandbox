using UnityEngine;
using System.Collections;

public abstract class DirectionTargetedProjectile : DirectionTargeted
{
    protected string projectilePrefabPath;
    protected GameObject projectilePrefab;

    protected override void LoadPrefabs()
    {
        projectilePrefab = Resources.Load<GameObject>(projectilePrefabPath);
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        champion.OrientationManager.RotateCharacterInstantly(destinationOnCast);

        SpawnProjectile(transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.5f), transform.rotation);

        FinishAbilityCast();
    }

    protected void SpawnProjectile(Vector3 position, Quaternion rotation)
    {
        Projectile projectile = (Instantiate(projectilePrefab, position, rotation)).GetComponent<Projectile>();
        projectile.ShootProjectile(champion.Team, affectedUnitType, speed, range);
        projectile.OnAbilityEffectHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;
    }

    protected virtual void OnProjectileHit(AbilityEffect projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        float damage = GetAbilityDamage(unitHit, isACriticalStrike);
        DamageUnit(unitHit, damage);
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
        AbilityHit(unitHit, damage);
    }

    protected virtual void OnProjectileReachedEnd(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
