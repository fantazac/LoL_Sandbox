using UnityEngine;
using System.Collections;

public abstract class DirectionTargetedProjectile : DirectionTargeted
{
    [SerializeField]
    protected GameObject projectilePrefab;

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        Projectile projectile = ((GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation)).GetComponent<Projectile>();
        projectile.ShootProjectile(character.Team, affectedUnitType, speed, range);
        projectile.OnAbilityEffectHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;

        FinishAbilityCast();
    }

    protected virtual void OnAreaOfEffectHit(AbilityEffect projectile, Entity entityHit) // This will most likely go after Lucian Q becomes a UnitTargeted ability
    {
        entityHit.GetComponent<Health>().Hit(damage);
    }

    protected virtual void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.GetComponent<Health>().Hit(damage);
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }
    }

    protected virtual void OnProjectileReachedEnd(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
