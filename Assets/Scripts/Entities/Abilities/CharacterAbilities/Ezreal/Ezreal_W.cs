using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_W : DirectionTargetedProjectile
{
    private float manaRefundedOnDamageDealt;
    private List<Ability> abilitiesToTriggerMark;

    protected Ezreal_W()
    {
        abilityName = "Essence Flux";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitType = AbilityAffectedUnitType.OBJECTIVES_AND_ENEMY_CHARACTERS;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        MaxLevel = 5;

        range = 1150;
        speed = 1550;
        damage = 75;// 75/125/175/225/275
        damagePerLevel = 50;
        bonusADScaling = 0.6f;// 60%
        totalAPScaling = 0.7f;// 70%
        resourceCost = 50;
        baseCooldown = 12;// 12
        castTime = 0.2f;//TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        manaRefundedOnDamageDealt = 60;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealW";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Ezreal/EzrealW";
    }

    protected override void Start()
    {
        base.Start();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Ezreal_W_Debuff>() };

        AbilityDebuffs[0].OnAbilityBuffRemoved += RemoveDebuffFromEntityHit;

        abilitiesToTriggerMark = new List<Ability>();
        foreach (Ability ability in character.CharacterAbilityManager.CharacterAbilities)
        {
            if (ability != this)
            {
                abilitiesToTriggerMark.Add(ability);
            }
        }
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        character.CharacterOrientationManager.RotateCharacterInstantly(destinationOnCast);

        SpawnProjectile(transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.65f), transform.rotation);

        FinishAbilityCast();
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit, bool isACriticalStrike, bool willMiss)
    {
        Destroy(projectile.gameObject);
        AddNewDebuffToEntityHit(entityHit);
        AbilityHit(entityHit, 0);
    }

    private void AddNewDebuffToEntityHit(Entity entityHit)
    {
        entityHit.EntityEffectSourceManager.OnEntityHitByAbility += OnMarkedEntityHitByAbility;
        entityHit.EntityEffectSourceManager.OnEntityHitByBasicAttack += OnMarkedEntityHitByBasicAttack;
        AbilityDebuffs[0].AddNewBuffToAffectedEntity(entityHit);
    }

    private void RemoveDebuffFromEntityHit(Entity entityHit)
    {
        entityHit.EntityEffectSourceManager.OnEntityHitByAbility -= OnMarkedEntityHitByAbility;
        entityHit.EntityEffectSourceManager.OnEntityHitByBasicAttack -= OnMarkedEntityHitByBasicAttack;
    }

    private void OnMarkedEntityHitByAbility(Entity entityHit, Ability sourceAbility)
    {
        if (entityHit.EntityBuffManager.IsAffectedByDebuff(AbilityDebuffs[0]) && abilitiesToTriggerMark.Contains(sourceAbility))
        {
            DealDamageToMarkedEntity(entityHit, sourceAbility);
        }
    }

    private void OnMarkedEntityHitByBasicAttack(Entity entityHit, Entity sourceEntity)
    {
        if (entityHit.EntityBuffManager.IsAffectedByDebuff(AbilityDebuffs[0]) && sourceEntity == character)
        {
            DealDamageToMarkedEntity(entityHit);
        }
    }

    private void DealDamageToMarkedEntity(Entity entityHit, Ability sourceAbility = null)
    {
        AbilityDebuffs[0].ConsumeBuff(entityHit);
        character.EntityStatsManager.Resource.Restore(manaRefundedOnDamageDealt + (sourceAbility != null ? sourceAbility.GetResourceCost() : 0));
        entityHit.EntityStatsManager.ReduceHealth(damageType, GetAbilityDamage(entityHit));
        AbilityHit(entityHit, damage, false);
    }
}
