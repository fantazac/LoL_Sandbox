using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : AbilityEffect
{
    protected float duration;

    protected bool collidersInChildren;

    public void ActivateAreaOfEffect(List<Entity> unitsAlreadyHit, EntityTeam teamOfShooter, AbilityAffectedUnitType affectedUnitType, float duration)
    {
        UnitsAlreadyHit = unitsAlreadyHit;
        this.teamOfCallingEntity = teamOfShooter;
        this.affectedUnitType = affectedUnitType;
        this.duration = duration;
        StartCoroutine(ActivateAbilityEffect());
    }

    public void ActivateAreaOfEffect(List<Entity> unitsAlreadyHit, EntityTeam teamOfShooter, AbilityAffectedUnitType affectedUnitType, float duration, bool collidersInChildren)
    {
        if (collidersInChildren)
        {
            this.collidersInChildren = collidersInChildren;
            foreach(AreaOfEffectCollider aoeCollider in GetComponentsInChildren<AreaOfEffectCollider>())
            {
                aoeCollider.OnTriggerEnterInChild += OnTriggerEnterInChild;
            }
        }
        ActivateAreaOfEffect(unitsAlreadyHit, teamOfShooter, affectedUnitType, duration);
    }

    protected override IEnumerator ActivateAbilityEffect()
    {
        float timeBeforeFrame = Time.deltaTime;

        yield return null;//2 frames
        yield return null;

        DisableColliders();

        yield return new WaitForSeconds(duration - (Time.deltaTime - timeBeforeFrame));

        Destroy(gameObject);
    }

    protected void DisableColliders()
    {
        if (collidersInChildren)
        {
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }
        else
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        Entity entityHit = collider.gameObject.GetComponent<Entity>();

        if (entityHit != null && CanAffectTarget(entityHit))
        {
            UnitsAlreadyHit.Add(entityHit);
            OnAbilityEffectHitTarget(entityHit);
        }
    }

    protected void OnTriggerEnterInChild(Collider collider)
    {
        OnTriggerEnter(collider);
    }
}
