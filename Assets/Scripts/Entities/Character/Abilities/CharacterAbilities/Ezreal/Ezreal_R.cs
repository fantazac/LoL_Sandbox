using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_R : SkillShot
{
    protected Ezreal_R()
    {
        range = (float)Range.GLOBAL;
        speed = 2000;
        damage = 300;
        castTime = 1;
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
