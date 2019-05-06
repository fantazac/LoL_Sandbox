using System;
using System.Collections.Generic;
using UnityEngine;

public class Varus_R : DirectionTargetedProjectile
{
    private string entityPrefabPath;
    private GameObject entityPrefab;

    private float radius;

    private Varus_W varusW;

    protected Varus_R()
    {
        abilityName = "Chain of Corruption";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitTypes = new List<Type>() { typeof(Character) };
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        MaxLevel = 3;

        range = 1075;
        speed = 1850;
        damage = 150; // 150/200/250
        damagePerLevel = 50;
        totalAPScaling = 1; // 100%
        resourceCost = 100;
        baseCooldown = 110; // 110/90/70
        baseCooldownPerLevel = -20;
        castTime = 0.25f; //TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        radius = 550;

        IsAnUltimateAbility = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusR";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Varus/VarusR";

        entityPrefabPath = "CharacterAbilitiesPrefabs/Varus/VarusR_Entity";
    }

    protected override void LoadPrefabs()
    {
        entityPrefab = Resources.Load<GameObject>(entityPrefabPath);

        base.LoadPrefabs();
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetEnemyTeams(allyTeam);
    }

    protected override void Start()
    {
        base.Start();

        varusW = GetComponent<Varus_W>();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Varus_R_Debuff>(), gameObject.AddComponent<Varus_R_TetherDebuff>() };
    }

    protected override void ModifyValues()
    {
        radius *= StaticObjects.MultiplyingFactor;

        base.ModifyValues();
    }

    protected override void OnProjectileHit(Projectile projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }

        Varus_R_Entity varusREntity =
            Instantiate(entityPrefab, Vector3.right * unitHit.transform.position.x + Vector3.forward * unitHit.transform.position.z, Quaternion.identity)
                .GetComponent<Varus_R_Entity>();
        varusREntity.SetupEntity(this, unitHit, varusW, new List<Unit>() { unitHit }, new List<Varus_R_Entity>());
        varusREntity.StartEntityLife();
    }

    public List<Unit> GetUnitsInRange(Unit affectedUnit, List<Unit> alreadyAffectedUnits)
    {
        List<Unit> unitsInRange = new List<Unit>();

        Vector3 groundPosition = Vector3.right * affectedUnit.transform.position.x + Vector3.forward * affectedUnit.transform.position.z;
        foreach (Collider other in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, radius))
        {
            Character tempCharacter = other.GetComponentInParent<Character>();
            if (tempCharacter && !alreadyAffectedUnits.Contains(tempCharacter) && tempCharacter.IsTargetable(affectedUnitTypes, affectedTeams))
            {
                unitsInRange.Add(tempCharacter);
            }
        }

        return unitsInRange;
    }

    public void ApplyDamageAndCrowdControlToUnitHit(Unit unitHit)
    {
        DamageUnitHit(unitHit);
        AbilityDebuffs[0].AddNewBuffToAffectedUnit(unitHit);
    }

    public void ApplyTetherToUnitInRange(Unit unitInRange)
    {
        AbilityDebuffs[1].AddNewBuffToAffectedUnit(unitInRange);
    }

    private void DamageUnitHit(Unit unitHit)
    {
        float abilityDamage = GetAbilityDamage(unitHit);
        DamageUnit(unitHit, abilityDamage);
        if (varusW)
        {
            varusW.ProcStacks(unitHit, this);
        }

        AbilityHit(unitHit, abilityDamage);
    }
}
