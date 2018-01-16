using UnityEngine;
using System.Collections;

public abstract class SkillShot : DirectionTargeted
{
    [SerializeField]
    protected GameObject projectilePrefab;

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        Projectile projectile = ((GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation)).GetComponent<Projectile>();
        projectile.ShootProjectile(GetComponent<Character>().team, speed, range, damage);
        projectile.OnProjectileHit += OnSkillShotHit;
        projectile.OnProjectileReachedEnd += OnSkillShotReachedEnd;

        FinishAbilityCast();
    }

    protected virtual void OnSkillShotHit(Projectile projectile) { }

    protected virtual void OnSkillShotReachedEnd(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
