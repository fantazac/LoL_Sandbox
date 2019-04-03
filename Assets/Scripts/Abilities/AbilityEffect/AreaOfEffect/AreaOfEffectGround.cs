using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaOfEffectGround : AbilityEffect
{
    private MeshRenderer meshRenderer;

    private int numberOfTicks;
    private WaitForSeconds delayPerTick;
    private float radius;
    private WaitForSeconds delayActivation;

    public delegate void OnAbilityEffectGroundHitOnSpawnHandler(AbilityEffect abilityEffect, List<Unit> affectedUnits);
    public event OnAbilityEffectGroundHitOnSpawnHandler OnAbilityEffectGroundHitOnSpawn;

    public delegate void OnAbilityEffectGroundHitHandler(AbilityEffect abilityEffect, List<Unit> previouslyAffectedUnits, List<Unit> affectedUnits);
    public event OnAbilityEffectGroundHitHandler OnAbilityEffectGroundHit;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void CreateAreaOfEffect(List<Team> affectedTeams, List<Type> affectedUnitTypes, WaitForSeconds delayPerTick, int numberOfTicks, float radius,
        WaitForSeconds delayActivation = null)
    {
        this.affectedTeams = affectedTeams;
        this.affectedUnitTypes = affectedUnitTypes;
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
            meshRenderer.enabled = false;

            yield return delayActivation;

            meshRenderer.enabled = true;
        }

        OnAbilityEffectGroundHitOnSpawn?.Invoke(this, GetAffectedUnits());

        List<Unit> previouslyAffectedUnits = new List<Unit>();

        for (int i = 0; i < numberOfTicks; i++)
        {
            yield return delayPerTick;

            List<Unit> affectedUnits = GetAffectedUnits();
            OnAbilityEffectGroundHit?.Invoke(this, previouslyAffectedUnits, affectedUnits);

            previouslyAffectedUnits = affectedUnits;
        }

        yield return delayPerTick;

        OnAbilityEffectGroundHit?.Invoke(this, previouslyAffectedUnits, new List<Unit>());

        Destroy(gameObject);
    }

    private List<Unit> GetAffectedUnits()
    {
        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        Vector3 highestPosition = groundPosition + Vector3.up * 5;

        return Physics.OverlapCapsule(groundPosition, highestPosition, radius)
            .Select(GetUnitHit)
            .Where(unitInArea => !unitInArea && CanAffectTarget(unitInArea))
            .ToList();
    }

    protected override bool CanAffectTarget(Unit unitHit)
    {
        return unitHit.IsTargetable(affectedUnitTypes, affectedTeams);
    }
}
