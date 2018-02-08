using UnityEngine;
using System.Collections;

public abstract class DirectionTargetedProjectile : DirectionTargeted
{
    [SerializeField]
    protected GameObject projectilePrefab;

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        character.CharacterOrientation.RotateCharacterInstantly(destinationOnCast);
        
        Projectile projectile = ((GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation)).GetComponent<Projectile>();
        projectile.ShootProjectile(character.Team, affectedUnitType, speed, range);
        projectile.OnAbilityEffectHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;
        
        FinishAbilityCast();
    }

    protected virtual void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(damage);
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
