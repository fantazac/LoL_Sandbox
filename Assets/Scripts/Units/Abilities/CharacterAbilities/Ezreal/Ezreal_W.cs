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
        damage = 80;// 80/135/190/245/300
        damagePerLevel = 55;
        bonusADScaling = 0.6f;// 60%
        totalAPScaling = 0.7f;// 70/75/80/85/90%
        totalAPScalingPerLevel = 0.05f;
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

        AbilityDebuffs[0].OnAbilityBuffRemoved += RemoveDebuffFromAffectedUnit;

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

    protected override void OnProjectileHit(AbilityEffect projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        Destroy(projectile.gameObject);
        AddNewDebuffToAffectedUnit(unitHit);
        AbilityHit(unitHit, 0);
    }

    private void AddNewDebuffToAffectedUnit(Unit affectedUnit)
    {
        affectedUnit.EffectSourceManager.OnUnitHitByAbility += OnMarkedUnitHitByAbility;
        affectedUnit.EffectSourceManager.OnUnitHitByBasicAttack += OnMarkedUnitHitByBasicAttack;
        AbilityDebuffs[0].AddNewBuffToAffectedUnit(affectedUnit);
    }

    private void RemoveDebuffFromAffectedUnit(Unit affectedUnit)
    {
        affectedUnit.EffectSourceManager.OnUnitHitByAbility -= OnMarkedUnitHitByAbility;
        affectedUnit.EffectSourceManager.OnUnitHitByBasicAttack -= OnMarkedUnitHitByBasicAttack;
    }

    private void OnMarkedUnitHitByAbility(Unit unitHit, Ability sourceAbility)
    {
        if (unitHit.BuffManager.IsAffectedByDebuff(AbilityDebuffs[0]) && abilitiesToTriggerMark.Contains(sourceAbility))
        {
            DealDamageToMarkedUnit(unitHit, sourceAbility);
        }
    }

    private void OnMarkedUnitHitByBasicAttack(Unit unitHit, Unit sourceUnit)
    {
        if (unitHit.BuffManager.IsAffectedByDebuff(AbilityDebuffs[0]) && sourceUnit == character)
        {
            DealDamageToMarkedUnit(unitHit);
        }
    }

    private void DealDamageToMarkedUnit(Unit unitHit, Ability sourceAbility = null)
    {
        AbilityDebuffs[0].ConsumeBuff(unitHit);
        character.StatsManager.Resource.Restore(manaRefundedOnDamageDealt + (sourceAbility != null ? sourceAbility.GetResourceCost() : 0));
        float damage = GetAbilityDamage(unitHit);
        DamageUnit(unitHit, damage);
        AbilityHit(unitHit, damage, false);
    }
}
