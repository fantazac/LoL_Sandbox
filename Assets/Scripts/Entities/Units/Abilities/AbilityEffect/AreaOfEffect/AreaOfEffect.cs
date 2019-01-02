using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : AbilityEffect
{
    protected float duration;

    protected bool collidersInChildren;

    public void CreateAreaOfEffect(List<Unit> unitsAlreadyHit, Team castingUnitTeam, AbilityAffectedUnitType affectedUnitType, float duration)
    {
        UnitsAlreadyHit = unitsAlreadyHit;
        this.castingUnitTeam = castingUnitTeam;
        this.affectedUnitType = affectedUnitType;
        this.duration = duration;
    }

    public void CreateAreaOfEffect(List<Unit> unitsAlreadyHit, Team castingUnitTeam, AbilityAffectedUnitType affectedUnitType, float duration, bool collidersInChildren)
    {
        if (collidersInChildren)
        {
            this.collidersInChildren = collidersInChildren;
            foreach (AreaOfEffectCollider aoeCollider in GetComponentsInChildren<AreaOfEffectCollider>())
            {
                aoeCollider.OnTriggerEnterInChild += OnTriggerEnterInChild;
            }
        }
        CreateAreaOfEffect(unitsAlreadyHit, castingUnitTeam, affectedUnitType, duration);
    }

    public void ActivateAreaOfEffect()
    {
        StartCoroutine(ActivateAbilityEffect());
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
        Unit unitHit = collider.GetComponentInParent<Unit>();
        
        if (unitHit != null && CanAffectTarget(unitHit))
        {
            UnitsAlreadyHit.Add(unitHit);
            OnAbilityEffectHitTarget(unitHit);
        }
    }

    protected void OnTriggerEnterInChild(Collider collider)
    {
        OnTriggerEnter(collider);
    }
}
