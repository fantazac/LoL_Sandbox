using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varus_R_Entity : MonoBehaviour
{
    private Varus_R ability;
    private Varus_W varusW;

    private List<Entity> alreadyAffectedEntities;
    private Entity affectedEntity;

    private float tetherRange;
    private float timeBetweenTetherRangeChecks;
    private WaitForSeconds delayTetherRangeChecks;

    private float spreadTime;
    private WaitForSeconds delaySpread;

    private List<Entity> entitiesAffectedByTethers;
    private List<Varus_R_Entity> varusREntitiesCurrentlySpreading;

    private int stacksToApply;
    private float timeBetweenStacks;
    private WaitForSeconds delayForStacks;

    private Varus_R_Entity()
    {
        tetherRange = 600;
        timeBetweenTetherRangeChecks = 0.2f;
        delayTetherRangeChecks = new WaitForSeconds(timeBetweenTetherRangeChecks);

        spreadTime = 2;
        delaySpread = new WaitForSeconds(spreadTime);

        entitiesAffectedByTethers = new List<Entity>();

        stacksToApply = 3;
        timeBetweenStacks = 0.5f;
        delayForStacks = new WaitForSeconds(timeBetweenStacks);
    }

    private void Start()
    {
        ModifyValues();
    }

    private void ModifyValues()
    {
        tetherRange *= StaticObjects.MultiplyingFactor;
    }

    public void SetupEntity(Varus_R ability, Entity affectedEntity, Varus_W varusW, List<Entity> alreadyAffectedEntities, List<Varus_R_Entity> varusREntitiesCurrentlySpreading)
    {
        this.ability = ability;
        this.affectedEntity = affectedEntity;
        this.varusW = varusW;
        this.alreadyAffectedEntities = alreadyAffectedEntities;
        this.varusREntitiesCurrentlySpreading = varusREntitiesCurrentlySpreading;
        ApplyAbilityEffectToEntityHit(affectedEntity);
        varusREntitiesCurrentlySpreading.Add(this);
    }

    private void ApplyAbilityEffectToEntityHit(Entity entityHit)
    {
        ability.ApplyDamageAndCrowdControlToEntityHit(entityHit);
        foreach (Entity entityInRange in ability.GetEntitiesInRange(entityHit, alreadyAffectedEntities))
        {
            SetupTether(entityInRange);
        }
    }

    private void SetupTether(Entity entityInRange)
    {
        if (!entityInRange.EntityBuffManager.IsAffectedByDebuff(ability.AbilityDebuffs[1]))
        {
            AddTetherToEntityInRange(entityInRange);
        }
        else
        {
            bool entityIsAffectedByAnotherTether = false;
            foreach (Varus_R_Entity varusREntity in varusREntitiesCurrentlySpreading)
            {
                float tetherLength = varusREntity.GetTetherLength(entityInRange);
                if (tetherLength == float.MaxValue)
                {
                    continue;
                }
                entityIsAffectedByAnotherTether = true;
                if (TetherIsShorter(tetherLength, entityInRange))
                {
                    varusREntity.RemoveTether(entityInRange);
                    AddTetherToEntityInRange(entityInRange);
                    break;
                }
            }
            if (!entityIsAffectedByAnotherTether)
            {
                AddTetherToEntityInRange(entityInRange);
            }
        }
    }

    private void AddTetherToEntityInRange(Entity entityInRange)
    {
        ability.ApplyTetherToEntityInRange(entityInRange);
        entitiesAffectedByTethers.Add(entityInRange);
    }

    private bool TetherIsShorter(float otherTetherLength, Entity entityInRange)
    {
        return Vector3.Distance(transform.position, entityInRange.transform.position) < otherTetherLength;
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
    }

    public void StartEntityLife()
    {
        StartCoroutine(EntityLife());
        if (varusW)
        {
            StartCoroutine(ApplyVarusWPassiveStacks());
        }

        foreach (Entity entityAffectedByTether in entitiesAffectedByTethers)
        {
            StartCoroutine(Tether(entityAffectedByTether));
        }
    }

    private IEnumerator EntityLife()
    {
        yield return delaySpread;

        varusREntitiesCurrentlySpreading.Remove(this);

        List<Varus_R_Entity> varusREntitiesToSpawn = new List<Varus_R_Entity>();
        foreach (Entity entityAffectedByTether in entitiesAffectedByTethers)
        {
            Varus_R_Entity varusREntity = SetupNextEntity(entityAffectedByTether);
            varusREntitiesToSpawn.Add(varusREntity);
            alreadyAffectedEntities.Add(entityAffectedByTether);
        }
        for (int i = 0; i < varusREntitiesToSpawn.Count; i++)
        {
            varusREntitiesToSpawn[i].SetupEntity(ability, entitiesAffectedByTethers[i], varusW, alreadyAffectedEntities, varusREntitiesCurrentlySpreading);
        }
        foreach (Varus_R_Entity varusREntity in varusREntitiesToSpawn)
        {
            varusREntity.StartEntityLife();
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

        RemoveTether(affectedEntity);
        //This might be how it works, requires 5 to test on live... If it's not that, I have no idea how to code it without breaking all I did
        //(Varus -> target -> 2 waiting behind -> 1 waiting behind the 2 who leaves ONE circle and checks if the debuff is still there)
        ability.AbilityDebuffs[1].ConsumeBuff(affectedEntity);
    }

    private IEnumerator ApplyVarusWPassiveStacks()//TODO: If you cleanse the root, does it still apply the stacks? Currently, yes.
    {
        for (int i = 0; i < stacksToApply; i++)
        {
            yield return delayForStacks;

            varusW.AddNewDebuffToEntityHit(affectedEntity);
        }
    }
}
