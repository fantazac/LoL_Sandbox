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
        projectile.ShootProjectile(GetComponent<Character>().team, speed, range, damage);
        projectile.OnProjectileHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;

        FinishAbilityCast();
    }

    protected virtual void OnProjectileHit(Projectile projectile) { }

    protected virtual void OnProjectileReachedEnd(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
