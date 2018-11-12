using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varus_R : DirectionTargetedProjectile
{
    protected string entityPrefabPath;
    protected GameObject entityPrefab;

    private float radius;

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

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Varus_R_Debuff>(), gameObject.AddComponent<Varus_R_TetherDebuff>() };
    }

    protected override void ModifyValues()
    {
        radius *= StaticObjects.MultiplyingFactor;

        base.ModifyValues();
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        if (effectType == AbilityEffectType.SINGLE_TARGET)
        {
            Destroy(projectile.gameObject);
        }

        Varus_R_Entity varusREntity = Instantiate(entityPrefab, Vector3.right * entityHit.transform.position.x + Vector3.forward * entityHit.transform.position.z, Quaternion.identity).GetComponent<Varus_R_Entity>();
        varusREntity.SetupEntity(this, entityHit, new List<Entity>() { entityHit }, new List<Varus_R_Entity>());
        varusREntity.StartEntityLife();
    }

    public List<Entity> GetEntitiesInRange(Entity affectedEntity, List<Entity> alreadyAffectedEntities)
    {
        List<Entity> entitiesInRange = new List<Entity>();

        Character tempCharacter;
        Vector3 groundPosition = Vector3.right * affectedEntity.transform.position.x + Vector3.forward * affectedEntity.transform.position.z;
        foreach (Collider collider in Physics.OverlapCapsule(groundPosition, groundPosition + Vector3.up * 5, radius))
        {
            tempCharacter = collider.GetComponentInParent<Character>();
            if (tempCharacter != null && !alreadyAffectedEntities.Contains(tempCharacter) && TargetIsValid.CheckIfTargetIsValid(tempCharacter, affectedUnitType, character.Team))
            {
                entitiesInRange.Add(tempCharacter);
            }
        }

        return entitiesInRange;
    }

    public void DamageEntity(Entity entityHit)
    {
        float damage = GetAbilityDamage(entityHit);
        entityHit.EntityStatsManager.ReduceHealth(damageType, damage);
        AbilityHit(entityHit, damage);
    }
}
