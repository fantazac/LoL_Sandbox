using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUnitTargeted : Projectile
{
    private Collider projectileCollider;
    
    private Unit target;
    private Collider targetCollider;

    private bool alreadyHitATarget; //This is to prevent OnTriggerEnter to cast multiple times if multiple targets enter the collider at the same time

    protected void Awake()
    {
        projectileCollider = GetComponent<Collider>();
    }
    
    public void ShootProjectile(List<Team> affectedTeams, Unit target, float speed, bool isACriticalStrike = false, bool willMiss = false)
    {
        if (target)
        {
            this.affectedTeams = affectedTeams;
            this.target = target;
            this.speed = speed;
            this.isACriticalStrike = isACriticalStrike;
            this.willMiss = willMiss;
            targetCollider = target.GetComponentInChildren<Collider>();
            StartCoroutine(Shoot());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override IEnumerator Shoot()
    {
        Vector3 lastTargetPosition = transform.position;
        while (target)
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

    protected void OnTriggerEnter(Collider collider)
    {
        if (alreadyHitATarget || collider != targetCollider) return;
        
        unitsAlreadyHit.Add(target);
        OnProjectileHitTarget(target, isACriticalStrike, willMiss);
        alreadyHitATarget = true;
        projectileCollider.enabled = false;
    }
}
