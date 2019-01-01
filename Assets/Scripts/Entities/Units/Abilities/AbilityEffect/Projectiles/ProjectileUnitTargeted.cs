using System.Collections;
using UnityEngine;

public class ProjectileUnitTargeted : Projectile
{
    protected Unit target;
    protected Collider targetCollider;

    protected bool alreadyHitATarget;//This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    public void ShootProjectile(Team castingUnitTeam, Unit target, float speed, bool isACriticalStrike = false, bool willMiss = false)
    {
        if (target != null)
        {
            this.castingUnitTeam = castingUnitTeam;
            this.target = target;
            this.speed = speed;
            this.isACriticalStrike = isACriticalStrike;
            this.willMiss = willMiss;
            targetCollider = target.GetComponentInChildren<Collider>();
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
            OnAbilityEffectHitTarget(target, isACriticalStrike, willMiss);
            alreadyHitATarget = true;
            GetComponent<Collider>().enabled = false;
        }
    }
}
