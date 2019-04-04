﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : AbilityEffect
{
    protected float speed;
    protected float range;
    protected bool isACriticalStrike;
    protected bool willMiss;

    protected Vector3 initialPosition;

    public delegate void OnProjectileHitHandler(Projectile projectile, Unit unitHit, bool isACriticalStrike, bool willMiss);
    public event OnProjectileHitHandler OnProjectileHit;

    public delegate void OnProjectileReachedEndHandler(Projectile projectile);
    public event OnProjectileReachedEndHandler OnProjectileReachedEnd;

    public void ShootProjectile(List<Team> affectedTeams, List<Type> affectedUnitTypes, float speed, float range, bool isACriticalStrike = false, bool willMiss = false)
    {
        this.affectedTeams = affectedTeams;
        this.affectedUnitTypes = affectedUnitTypes;
        this.speed = speed;
        this.range = range;
        this.isACriticalStrike = isACriticalStrike;
        this.willMiss = willMiss;
        initialPosition = transform.position;
        StartCoroutine(Shoot());
    }

    protected void OnProjectileHitTarget(Unit unitHit, bool isACriticalStrike = false, bool willMiss = false)
    {
        OnProjectileHit?.Invoke(this, unitHit, isACriticalStrike, willMiss);
    }

    protected void OnProjectileReachedEndOfRange()
    {
        OnProjectileReachedEnd?.Invoke(this);
    }

    protected virtual IEnumerator Shoot()
    {
        while (Vector3.Distance(transform.position, initialPosition) < range)
        {
            transform.position += transform.forward * Time.deltaTime * speed;

            yield return null;
        }

        OnProjectileReachedEndOfRange();
    }
}
