using UnityEngine;
using System.Collections;

public abstract class DirectionTargetedProjectile : DirectionTargeted
{
    [SerializeField]
    protected GameObject projectilePrefab;

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        character.CharacterOrientation.RotateCharacterInstantly(destinationOnCast);

        SpawnProjectile(transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.5f), transform.rotation);

        FinishAbilityCast();
    }

    protected void SpawnProjectile(Vector3 position, Quaternion rotation)
    {
        Projectile projectile = (Instantiate(projectilePrefab, position, rotation)).GetComponent<Projectile>();
        projectile.ShootProjectile(character.Team, affectedUnitType, speed, range);
        projectile.OnAbilityEffectHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;
    }

    protected virtual void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(GetAbilityDamage());
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
        AbilityHit();
    }

    protected virtual void OnProjectileReachedEnd(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
