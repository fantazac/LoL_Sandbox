using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUnitTargeted : Projectile
{
    protected Entity target;
    protected Collider targetCollider;
    protected bool isACriticalAttack;

    protected bool alreadyHitATarget;//This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    public delegate void OnProjectileUnitTargetedHitHandler(AbilityEffect abilityEffect, Entity entityHit, bool isACriticalAttack);
    public event OnProjectileUnitTargetedHitHandler OnProjectileUnitTargetedHit;

    public void ShootProjectile(EntityTeam teamOfCallingEntity, Entity target, float speed, bool isACriticalAttack = false)
    {
        if(target != null)
        {
            this.teamOfCallingEntity = teamOfCallingEntity;
            this.target = target;
            this.speed = speed;
            this.isACriticalAttack = isACriticalAttack;
            targetCollider = target.GetComponent<Collider>();
            StartCoroutine(ActivateAbilityEffect());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override IEnumerator ActivateAbilityEffect()
    {
        Vector3 lastTargetPosition = transform.position;
        while (target != null)
        {
            lastTargetPosition = target.transform.position;

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, Time.deltaTime * speed, 0));

            yield return null;
        }

        while (transform.position != lastTargetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, lastTargetPosition, Time.deltaTime * speed);

            yield return null;
        }

        Destroy(gameObject);
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        if (!alreadyHitATarget && collider == targetCollider)
        {
            UnitsAlreadyHit.Add(target);
            if(OnProjectileUnitTargetedHit != null)
            {
                OnProjectileUnitTargetedHit(this, target, isACriticalAttack);
            }
            OnAbilityEffectHitTarget(target);
            alreadyHitATarget = true;
            GetComponent<Collider>().enabled = false;
        }
    }
}
