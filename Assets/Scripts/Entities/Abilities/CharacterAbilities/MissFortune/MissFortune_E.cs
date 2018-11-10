using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFortune_E : GroundTargetedAoE
{
    private int totalTicks;
    private float timeBetweenTicks;
    private WaitForSeconds tickDelay;

    private float radius;

    public MissFortune_E()
    {
        abilityName = "Make It Rain";

        abilityType = AbilityType.AREA_OF_EFFECT;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        damageType = DamageType.MAGIC;
        effectType = AbilityEffectType.AREA_OF_EFFECT;

        MaxLevel = 5;

        range = 1000;
        damage = 80;// 80/115/150/185/220
        damagePerLevel = 35;
        totalAPScaling = 0.1f;
        resourceCost = 80;
        baseCooldown = 18;// 18/16/14/12/10
        baseCooldownPerLevel = -2f;
        castTime = 0.25f;//TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        totalTicks = 8;
        timeBetweenTicks = 0.25f;
        tickDelay = new WaitForSeconds(timeBetweenTicks);

        radius = 200;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneE";

        areaOfEffectPrefabPath = "CharacterAbilitiesPrefabs/MissFortune/MissFortuneE";
    }

    protected override void ModifyValues()
    {
        radius *= StaticObjects.MultiplyingFactor;

        base.ModifyValues();
    }

    protected override void Start()
    {
        base.Start();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<MissFortune_E_Debuff>() };
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        character.CharacterOrientationManager.RotateCharacterInstantly(destinationOnCast);

        AreaOfEffectGround areaOfEffect = (Instantiate(areaOfEffectPrefab, Vector3.right * destinationOnCast.x + Vector3.forward * destinationOnCast.z, Quaternion.identity)).GetComponent<AreaOfEffectGround>();
        areaOfEffect.CreateAreaOfEffect(character.Team, affectedUnitType, tickDelay, totalTicks, radius);
        areaOfEffect.ActivateAreaOfEffect();
        areaOfEffect.OnAbilityEffectGroundHit += OnAbilityEffectGroundHit;

        FinishAbilityCast();
    }

    protected void OnAbilityEffectGroundHit(AbilityEffect abilityEffect, List<Entity> previousEntitiesHit, List<Entity> entitiesHit)
    {
        AbilityDebuffs[0].AddNewBuffToAffectedEntities(previousEntitiesHit, entitiesHit);
        foreach (Entity entityHit in entitiesHit)
        {
            entityHit.EntityStatsManager.ReduceHealth(damageType, GetAbilityDamage(entityHit) / totalTicks);
        }
    }
}
