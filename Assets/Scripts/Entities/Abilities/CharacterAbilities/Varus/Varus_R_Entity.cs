using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varus_R_Entity : MonoBehaviour
{
    private Varus_R ability;

    private List<Entity> alreadyAffectedEntities;

    private float tetherRange;
    private float timeBetweenTetherRangeChecks;
    private WaitForSeconds delayTetherRangeChecks;

    private float timeToLive;
    private WaitForSeconds delayLife;

    private List<Entity> entitiesAffectedByTethers;
    private List<Varus_R_Entity> varusREntities;

    private Varus_R_Entity()
    {
        tetherRange = 600;
        timeBetweenTetherRangeChecks = 0.2f;
        delayTetherRangeChecks = new WaitForSeconds(timeBetweenTetherRangeChecks);

        timeToLive = 2;
        delayLife = new WaitForSeconds(timeToLive);

        entitiesAffectedByTethers = new List<Entity>();
    }

    private void Start()
    {
        ModifyValues();
    }

    private void ModifyValues()
    {
        tetherRange *= StaticObjects.MultiplyingFactor;
    }

    public void SetupEntity(Varus_R ability, Entity entityHit, List<Entity> alreadyAffectedEntities, List<Varus_R_Entity> varusREntities)
    {
        this.ability = ability;
        this.alreadyAffectedEntities = alreadyAffectedEntities;
        this.varusREntities = varusREntities;
        ApplyAbilityEffectToEntityHit(entityHit);
        varusREntities.Add(this);
    }

    private void ApplyAbilityEffectToEntityHit(Entity entityHit)
    {
        StartCoroutine(EntityLife());

        ability.DamageEntity(entityHit);
        ability.AbilityDebuffs[0].AddNewBuffToAffectedEntity(entityHit);
        foreach (Entity entityInRange in ability.GetEntitiesInRange(entityHit, alreadyAffectedEntities))
        {
            SetupTether(entityInRange);
        }
    }

    private void SetupTether(Entity entityInRange)
    {
        if (entityInRange.EntityBuffManager.GetDebuff(ability.AbilityDebuffs[1]) == null)
        {
            AddTetherToEntityInRange(entityInRange);
        }
        else
        {
            foreach (Varus_R_Entity varusREntity in varusREntities)
            {
                float tetherLength = varusREntity.GetTetherLength(entityInRange);
                if (tetherLength == float.MaxValue)
                {
                    continue;
                }
                if (tetherLength > Vector3.Distance(transform.position, entityInRange.transform.position))
                {
                    varusREntity.RemoveTether(entityInRange);
                    AddTetherToEntityInRange(entityInRange);
                    break;
                }
            }
        }
    }

    private void AddTetherToEntityInRange(Entity entityInRange)
    {
        ability.AbilityDebuffs[1].AddNewBuffToAffectedEntity(entityInRange);

        entitiesAffectedByTethers.Add(entityInRange);
        StartCoroutine(Tether(entityInRange));
    }

    public float GetTetherLength(Entity entityAffectedByTether)
    {
        if (entitiesAffectedByTethers.Contains(entityAffectedByTether))
        {
            return Vector3.Distance(transform.position, entityAffectedByTether.transform.position);
        }
        return float.MaxValue;
    }

    public void RemoveTether(Entity entityAffectedByTether)
    {
        entitiesAffectedByTethers.Remove(entityAffectedByTether);
        ability.AbilityDebuffs[1].ConsumeBuff(entityAffectedByTether);
    }

    private IEnumerator EntityLife()
    {
        yield return delayLife;

        varusREntities.Remove(this);

        List<Varus_R_Entity> varusREntitiesToSpawn = new List<Varus_R_Entity>();
        foreach (Entity entityAffectedByTether in entitiesAffectedByTethers)
        {
            ability.AbilityDebuffs[1].ConsumeBuff(entityAffectedByTether);
            varusREntitiesToSpawn.Add(SetupNextEntity(entityAffectedByTether));
            alreadyAffectedEntities.Add(entityAffectedByTether);
        }
        foreach (Varus_R_Entity varusREntity in varusREntitiesToSpawn)
        {
            varusREntity.SetupEntity(ability, entitiesAffectedByTethers[0], alreadyAffectedEntities, varusREntities);
            entitiesAffectedByTethers.RemoveAt(0);
        }

        Destroy(gameObject);
    }

    private Varus_R_Entity SetupNextEntity(Entity entityHit)
    {
        return Instantiate(gameObject, Vector3.right * entityHit.transform.position.x + Vector3.forward * entityHit.transform.position.z, Quaternion.identity).GetComponent<Varus_R_Entity>();
    }

    private IEnumerator Tether(Entity affectedEntity)
    {
        while (entitiesAffectedByTethers.Contains(affectedEntity) && Vector3.Distance(transform.position, affectedEntity.transform.position) <= tetherRange)
        {
            yield return delayTetherRangeChecks;
        }

        if (entitiesAffectedByTethers.Contains(affectedEntity))
        {
            RemoveTether(affectedEntity);
        }
    }
}
