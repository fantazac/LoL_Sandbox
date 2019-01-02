using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectGround : AbilityEffect
{
    protected int numberOfTicks;
    protected WaitForSeconds delayPerTick;
    protected float radius;
    protected bool callEventOnSpawn;
    protected WaitForSeconds delayActivation;

    public delegate void OnAbilityEffectGroundHitOnSpawnHandler(AbilityEffect abilityEffect, List<Unit> affectedUnits);
    public event OnAbilityEffectGroundHitOnSpawnHandler OnAbilityEffectGroundHitOnSpawn;

    public delegate void OnAbilityEffectGroundHitHandler(AbilityEffect abilityEffect, List<Unit> previouslyAffectedUnits, List<Unit> affectedUnits);
    public event OnAbilityEffectGroundHitHandler OnAbilityEffectGroundHit;

    public void CreateAreaOfEffect(Team castingUnitTeam, AbilityAffectedUnitType affectedUnitType, WaitForSeconds delayPerTick, int numberOfTicks, float radius, WaitForSeconds delayActivation = null)
    {
        this.castingUnitTeam = castingUnitTeam;
        this.affectedUnitType = affectedUnitType;
        this.delayPerTick = delayPerTick;
        this.numberOfTicks = numberOfTicks;
        this.radius = radius;
        this.delayActivation = delayActivation;
    }

    public void ActivateAreaOfEffect()
    {
        StartCoroutine(ActivateAbilityEffect());
    }

    protected override IEnumerator ActivateAbilityEffect()
    {
        if (delayActivation != null)
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
            yield return delayActivation;
            meshRenderer.enabled = true;
        }

        List<Unit> previouslyAffectedUnits = new List<Unit>();

        if (OnAbilityEffectGroundHitOnSpawn != null)
        {
            OnAbilityEffectGroundHitOnSpawn(this, GetAffectedUnits());
        }

        for (int i = 0; i < numberOfTicks; i++)
        {
            yield return delayPerTick;

            List<Unit> affectedUnits = GetAffectedUnits();
            if (OnAbilityEffectGroundHit != null)
            {
                OnAbilityEffectGroundHit(this, previouslyAffectedUnits, affectedUnits);
            }

            previouslyAffectedUnits = affectedUnits;
        }

        yield return delayPerTick;

        if (OnAbilityEffectGroundHit != null)
        {
            OnAbilityEffectGroundHit(this, previouslyAffectedUnits, new List<Unit>());
        }

        Destroy(gameObject);
    }

    protected List<Unit> GetAffectedUnits()
    {
        List<Unit> affectedUnits = new List<Unit>();

        Unit unitInArea;
        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, radius))
        {
            unitInArea = GetUnitHit(collider);
            if (unitInArea != null && CanAffectTarget(unitInArea))
            {
                affectedUnits.Add(unitInArea);
            }
        }

        return affectedUnits;
    }

    protected override bool CanAffectTarget(Unit unitHit)
    {
        return unitHit.IsTargetable(affectedUnitType, castingUnitTeam);
    }
}
