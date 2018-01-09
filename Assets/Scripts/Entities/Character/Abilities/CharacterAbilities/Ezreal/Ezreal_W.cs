using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_W : SkillShot
{
    protected Ezreal_W()
    {
        range = 1000;
        speed = 1550;
        damage = 80;
        castTime = 0.2f;
        delayCastTime = new WaitForSeconds(castTime);

        CanStopMovement = true;
        HasCastTime = true;
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.GetComponent<ProjectileMovement>().ShootProjectile(GetComponent<Health>(), speed, range, damage, true);//HEALTH TEMPORAIRE

        FinishAbilityCast();
    }
}
