using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUnitTargeted : Projectile
{
    private Entity target;
    private Collider targetCollider;

    protected bool alreadyHitATarget;//This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    public void ShootProjectile(EntityTeam teamOfCallingEntity, Entity target, float speed)
    {
        this.teamOfCallingEntity = teamOfCallingEntity;
        this.target = target;
        this.speed = speed;
        targetCollider = target.GetComponent<Collider>();
        StartCoroutine(ActivateAbilityEffect());
    }

    protected override IEnumerator ActivateAbilityEffect()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, Time.deltaTime * speed, 0));

            yield return null;
        }
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        if (!alreadyHitATarget && collider == targetCollider)
        {
            UnitsAlreadyHit.Add(target);
            OnAbilityEffectHitTarget(target);
            alreadyHitATarget = true;
            GetComponent<Collider>().enabled = false;
        }
    }
}
