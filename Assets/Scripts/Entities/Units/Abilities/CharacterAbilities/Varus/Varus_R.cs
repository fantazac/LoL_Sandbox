using System.Collections.Generic;
using UnityEngine;

public class Varus_R : DirectionTargetedProjectile
{
    protected string entityPrefabPath;
    protected GameObject entityPrefab;

    private float radius;

    private Varus_W varusW;

    protected Varus_R()
    {
        abilityName = "Chain of Corruption";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitType = AbilityAffectedUnitType.ENEMY_CHARACTERS;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        MaxLevel = 3;

        range = 1075;
        speed = 1850;
        damage = 100;// 100/175/250
        damagePerLevel = 75;
        totalAPScaling = 1;// 100%
        resourceCost = 100;
        baseCooldown = 110;// 110/90/70
        baseCooldownPerLevel = -20;
        castTime = 0.25f;//TODO: VERIFY ACTUAL VALUE
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

    protected override void OnProjectileHit(AbilityEffect projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }

        Varus_R_Entity varusREntity = Instantiate(entityPrefab, Vector3.right * unitHit.transform.position.x + Vector3.forward * unitHit.transform.position.z, Quaternion.identity).GetComponent<Varus_R_Entity>();
        varusREntity.SetupEntity(this, unitHit, varusW, new List<Unit>() { unitHit }, new List<Varus_R_Entity>());
        varusREntity.StartEntityLife();
    }

    public List<Unit> GetUnitsInRange(Unit affectedUnit, List<Unit> alreadyAffectedUnits)
    {
        List<Unit> unitsInRange = new List<Unit>();

        Character tempCharacter;
        Vector3 groundPosition = Vector3.right * affectedUnit.transform.position.x + Vector3.forward * affectedUnit.transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, radius))
        {
            tempCharacter = collider.GetComponentInParent<Character>();
            if (tempCharacter != null && !alreadyAffectedUnits.Contains(tempCharacter) && tempCharacter.IsTargetable(affectedUnitType, champion.Team))
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
        float damage = GetAbilityDamage(unitHit);
        DamageUnit(unitHit, damage);
        if (varusW)
        {
            varusW.ProcStacks(unitHit, this);
        }
        AbilityHit(unitHit, damage);
    }
}
