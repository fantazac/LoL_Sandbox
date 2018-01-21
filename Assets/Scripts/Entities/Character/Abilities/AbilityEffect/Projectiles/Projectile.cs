using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : AbilityEffect
{
    protected float speed;
    protected float range;

    protected Vector3 initialPosition;

    public delegate void OnProjectileReachedEndHandler(Projectile projectile);
    public event OnProjectileReachedEndHandler OnProjectileReachedEnd;

    public void ShootProjectile(EntityTeam teamOfCallingEntity, AbilityAffectedUnitType affectedUnitType, float speed, float range)
    {
        this.teamOfCallingEntity = teamOfCallingEntity;
        this.affectedUnitType = affectedUnitType;
        this.speed = speed;
        this.range = range;
        initialPosition = transform.position;
        StartCoroutine(ActivateAbilityEffect());
    }

    protected void OnProjectileReachedEndOfRange()
    {
        if (OnProjectileReachedEnd != null)
        {
            OnProjectileReachedEnd(this);
        }
    }
}
