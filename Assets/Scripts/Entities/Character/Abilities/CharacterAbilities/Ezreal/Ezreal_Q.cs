using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_Q : DirectionTargetedProjectile, CharacterAbility
{
    protected Ezreal_Q()
    {
        range = 1150;
        speed = 2000;
        damage = 100;
        castTime = 0.25f;
        delayCastTime = new WaitForSeconds(castTime);

        CanStopMovement = true;
        HasCastTime = true;
    }

    protected override void OnProjectileHit(Projectile projectile)
    {
        OnProjectileReachedEnd(projectile);
    }
}
