using UnityEngine;
using System.Collections;

public class Lucian_R : SkillShot
{
    private int amountOfProjectilesToShoot;
    private float durationOfActive;
    private float offset;

    private WaitForSeconds delayBetweenBullets;

    private Projectile projectile;

    protected Lucian_R()
    {
        range = 1200;
        speed = 2000;
        damage = 40;

        amountOfProjectilesToShoot = 20;
        durationOfActive = 3;
        offset = 0.2f;
        delayBetweenBullets = new WaitForSeconds(durationOfActive / (float)amountOfProjectilesToShoot);

        CanMoveWhileCasting = true;
        CanCastOtherAbilitiesWithCasting = true;
    }

    protected override IEnumerator AbilityWithoutCastTime()
    {
        ShootProjectile(0);

        for (int i = 1; i < amountOfProjectilesToShoot; i++)
        {
            yield return delayBetweenBullets;

            ShootProjectile(i);
        }

        FinishAbilityCast();
    }

    private void ShootProjectile(int projectileId)
    {
        projectile = ((GameObject)Instantiate(projectilePrefab,
                transform.position + (transform.forward * 0.6f) + (transform.right * (projectileId % 2 == 0 ? offset : -offset)),
                transform.rotation)).GetComponent<Projectile>();
        projectile.ShootProjectile(GetComponent<Character>().team, speed, range, damage);
        projectile.OnProjectileHit += OnProjectileHit;
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;
    }

    protected override void OnProjectileHit(Projectile projectile)
    {
        OnProjectileReachedEnd(projectile);
    }
}
