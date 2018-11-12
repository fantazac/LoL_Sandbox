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

    public delegate void OnAbilityEffectGroundHitOnSpawnHandler(AbilityEffect abilityEffect, List<Entity> entitiesHit);
    public event OnAbilityEffectGroundHitOnSpawnHandler OnAbilityEffectGroundHitOnSpawn;

    public delegate void OnAbilityEffectGroundHitHandler(AbilityEffect abilityEffect, List<Entity> previousEntitiesHit, List<Entity> entitiesHit);
    public event OnAbilityEffectGroundHitHandler OnAbilityEffectGroundHit;

    public void CreateAreaOfEffect(EntityTeam teamOfCallingEntity, AbilityAffectedUnitType affectedUnitType, WaitForSeconds delayPerTick, int numberOfTicks, float radius, WaitForSeconds delayActivation = null)
    {
        this.teamOfCallingEntity = teamOfCallingEntity;
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

        List<Entity> previousAffectedEntities = new List<Entity>();

        if (OnAbilityEffectGroundHitOnSpawn != null)
        {
            OnAbilityEffectGroundHitOnSpawn(this, GetAffectedEntities());
        }

        for (int i = 0; i < numberOfTicks; i++)
        {
            yield return delayPerTick;

            List<Entity> affectedEntities = GetAffectedEntities();
            if (OnAbilityEffectGroundHit != null)
            {
                OnAbilityEffectGroundHit(this, previousAffectedEntities, affectedEntities);
            }

            previousAffectedEntities = affectedEntities;
        }

        yield return delayPerTick;

        if (OnAbilityEffectGroundHit != null)
        {
            OnAbilityEffectGroundHit(this, previousAffectedEntities, new List<Entity>());
        }

        Destroy(gameObject);
    }

    protected List<Entity> GetAffectedEntities()
    {
        List<Entity> affectedEntities = new List<Entity>();

        Entity entityInArea;
        Vector3 groundPosition = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, radius))
        {
            entityInArea = GetEntityHit(collider);
            if (entityInArea != null && CanAffectTarget(entityInArea))
            {
                affectedEntities.Add(entityInArea);
            }
        }

        return affectedEntities;
    }

    protected override bool CanAffectTarget(Entity entityHit)
    {
        return TargetIsValid.CheckIfTargetIsValid(entityHit, affectedUnitType, teamOfCallingEntity);
    }
}
