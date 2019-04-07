using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varus_R_Entity : MonoBehaviour
{
    private Varus_R ability;
    private Varus_W varusW;

    private List<Unit> alreadyAffectedUnits;
    private Unit affectedUnit;

    private float tetherRange;
    private readonly WaitForSeconds delayTetherRangeChecks;

    private readonly WaitForSeconds delaySpread;

    private readonly List<Unit> unitsAffectedByTethers;
    private List<Varus_R_Entity> varusREntitiesCurrentlySpreading;

    private readonly int stacksToApply;
    private readonly WaitForSeconds delayForStacks;

    private Varus_R_Entity()
    {
        tetherRange = 600;
        delayTetherRangeChecks = new WaitForSeconds(0.2f);
        delaySpread = new WaitForSeconds(2);

        unitsAffectedByTethers = new List<Unit>();

        stacksToApply = 3;
        delayForStacks = new WaitForSeconds(0.5f);
    }

    private void Start()
    {
        ModifyValues();
    }

    private void ModifyValues()
    {
        tetherRange *= StaticObjects.MultiplyingFactor;
    }

    public void SetupEntity(Varus_R ability, Unit affectedUnit, Varus_W varusW, List<Unit> alreadyAffectedUnits, List<Varus_R_Entity> varusREntitiesCurrentlySpreading)
    {
        this.ability = ability;
        this.affectedUnit = affectedUnit;
        this.varusW = varusW;
        this.alreadyAffectedUnits = alreadyAffectedUnits;
        this.varusREntitiesCurrentlySpreading = varusREntitiesCurrentlySpreading;
        ApplyAbilityEffectToAffectedUnit(affectedUnit);
        varusREntitiesCurrentlySpreading.Add(this);
    }

    private void ApplyAbilityEffectToAffectedUnit(Unit affectedUnit)
    {
        ability.ApplyDamageAndCrowdControlToUnitHit(affectedUnit);
        foreach (Unit unitInRange in ability.GetUnitsInRange(affectedUnit, alreadyAffectedUnits))
        {
            SetupTether(unitInRange);
        }
    }

    private void SetupTether(Unit unitInRange)
    {
        if (!unitInRange.BuffManager.IsAffectedByDebuff(ability.AbilityDebuffs[1]))
        {
            AddTetherToUnitInRange(unitInRange);
        }
        else
        {
            bool unitIsAffectedByAnotherTether = false;
            foreach (Varus_R_Entity varusREntity in varusREntitiesCurrentlySpreading)
            {
                float tetherLength = varusREntity.GetTetherLength(unitInRange);
                if (tetherLength == float.MaxValue)
                {
                    continue;
                }

                unitIsAffectedByAnotherTether = true;

                if (!TetherIsShorter(tetherLength, unitInRange)) continue;

                varusREntity.RemoveTether(unitInRange);
                AddTetherToUnitInRange(unitInRange);
                break;
            }

            if (!unitIsAffectedByAnotherTether)
            {
                AddTetherToUnitInRange(unitInRange);
            }
        }
    }

    private void AddTetherToUnitInRange(Unit unitInRange)
    {
        ability.ApplyTetherToUnitInRange(unitInRange);
        unitsAffectedByTethers.Add(unitInRange);
    }

    private bool TetherIsShorter(float otherTetherLength, Unit unitInRange)
    {
        return Vector3.Distance(transform.position, unitInRange.transform.position) < otherTetherLength;
    }

    private float GetTetherLength(Unit unitAffectedByTether)
    {
        return unitsAffectedByTethers.Contains(unitAffectedByTether) ? Vector3.Distance(transform.position, unitAffectedByTether.transform.position) : float.MaxValue;
    }

    private void RemoveTether(Unit unitAffectedByTether)
    {
        unitsAffectedByTethers.Remove(unitAffectedByTether);
    }

    public void StartEntityLife()
    {
        StartCoroutine(EntityLife());
        if (varusW)
        {
            StartCoroutine(ApplyVarusWPassiveStacks());
        }

        foreach (Unit unitAffectedByTether in unitsAffectedByTethers)
        {
            StartCoroutine(Tether(unitAffectedByTether));
        }
    }

    private IEnumerator EntityLife()
    {
        yield return delaySpread;

        varusREntitiesCurrentlySpreading.Remove(this);

        List<Varus_R_Entity> varusREntitiesToSpawn = new List<Varus_R_Entity>();
        foreach (Unit unitAffectedByTether in unitsAffectedByTethers)
        {
            Varus_R_Entity varusREntity = SetupNextEntity(unitAffectedByTether);
            varusREntitiesToSpawn.Add(varusREntity);
            alreadyAffectedUnits.Add(unitAffectedByTether);
        }

        for (int i = 0; i < varusREntitiesToSpawn.Count; i++)
        {
            varusREntitiesToSpawn[i].SetupEntity(ability, unitsAffectedByTethers[i], varusW, alreadyAffectedUnits, varusREntitiesCurrentlySpreading);
        }

        foreach (Varus_R_Entity varusREntity in varusREntitiesToSpawn)
        {
            varusREntity.StartEntityLife();
        }

        Destroy(gameObject);
    }

    private Varus_R_Entity SetupNextEntity(Unit unitHit)
    {
        return Instantiate(gameObject, Vector3.right * unitHit.transform.position.x + Vector3.forward * unitHit.transform.position.z, Quaternion.identity)
            .GetComponent<Varus_R_Entity>();
    }

    private IEnumerator Tether(Unit affectedUnit)
    {
        while (unitsAffectedByTethers.Contains(affectedUnit) && Vector3.Distance(transform.position, affectedUnit.transform.position) <= tetherRange)
        {
            yield return delayTetherRangeChecks;
        }

        RemoveTether(affectedUnit);
        //This might be how it works, requires 5 to test on live... If it's not that, I have no idea how to code it without breaking all I did
        //(Varus -> target -> 2 waiting behind -> 1 waiting behind the 2 who leaves ONE circle and checks if the debuff is still there)
        ability.AbilityDebuffs[1].ConsumeBuff(affectedUnit);
    }

    private IEnumerator ApplyVarusWPassiveStacks() //TODO: If you cleanse the root, does it still apply the stacks? Currently, yes.
    {
        for (int i = 0; i < stacksToApply; i++)
        {
            yield return delayForStacks;

            varusW.AddNewDebuffToAffectedUnit(affectedUnit);
        }
    }
}
