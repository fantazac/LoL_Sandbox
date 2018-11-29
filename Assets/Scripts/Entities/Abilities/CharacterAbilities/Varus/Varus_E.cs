using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varus_E : GroundTargetedAoE, DamageSourceOnEntityKill //TODO: Shoot invisible projectile and then spawn area on destination reached
{
    private Varus_P varusP;
    private Varus_W varusW;

    private int totalTicks;
    private float timeBetweenTicks;
    private WaitForSeconds tickDelay;

    private float timeBeforeActivation;
    private WaitForSeconds delayActivation;

    private float radius;

    public Varus_E()
    {
        abilityName = "Hail of Arrows";

        abilityType = AbilityType.AREA_OF_EFFECT;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        damageType = DamageType.PHYSICAL;
        effectType = AbilityEffectType.AREA_OF_EFFECT;

        MaxLevel = 5;

        range = 925;
        damage = 70;// 70/105/140/175/210
        damagePerLevel = 35;
        bonusADScaling = 0.6f;
        resourceCost = 80;
        baseCooldown = 18;// 18/16/14/12/10
        baseCooldownPerLevel = -2f;
        castTime = 0.25f;//TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        totalTicks = 16;
        timeBetweenTicks = 0.25f;
        tickDelay = new WaitForSeconds(timeBetweenTicks);

        timeBeforeActivation = 0.5f;
        delayActivation = new WaitForSeconds(timeBeforeActivation);

        radius = 200;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusE";

        areaOfEffectPrefabPath = "CharacterAbilitiesPrefabs/Varus/VarusE";
    }

    protected override void ModifyValues()
    {
        radius *= StaticObjects.MultiplyingFactor;

        base.ModifyValues();
    }

    protected override void Start()
    {
        base.Start();

        varusP = GetComponent<Varus_P>();
        varusW = GetComponent<Varus_W>();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Varus_E_Debuff>() };
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        character.CharacterOrientationManager.RotateCharacterInstantly(destinationOnCast);

        AreaOfEffectGround areaOfEffect = (Instantiate(areaOfEffectPrefab, Vector3.right * destinationOnCast.x + Vector3.forward * destinationOnCast.z, Quaternion.identity)).GetComponent<AreaOfEffectGround>();
        areaOfEffect.CreateAreaOfEffect(character.Team, affectedUnitType, tickDelay, totalTicks, radius, delayActivation);
        areaOfEffect.OnAbilityEffectGroundHitOnSpawn += OnAbilityEffectGroundHitOnSpawn;
        areaOfEffect.OnAbilityEffectGroundHit += OnAbilityEffectGroundHit;
        areaOfEffect.ActivateAreaOfEffect();

        FinishAbilityCast();
    }

    protected void OnAbilityEffectGroundHitOnSpawn(AbilityEffect abilityEffect, List<Entity> entitiesHit)
    {
        foreach (Entity entityHit in entitiesHit)
        {
            float damage = GetAbilityDamage(entityHit);
            DamageEntity(entityHit, damage);
            if (varusW)
            {
                varusW.ProcStacks(entityHit, this);
            }
            AbilityHit(entityHit, damage);
        }
    }

    protected void OnAbilityEffectGroundHit(AbilityEffect abilityEffect, List<Entity> previousEntitiesHit, List<Entity> entitiesHit)
    {
        AbilityDebuffs[0].AddNewBuffToAffectedEntities(previousEntitiesHit, entitiesHit);
    }

    protected override void DamageEntity(Entity entityToDamage, float damage)
    {
        entityToDamage.EntityStatsManager.ReduceHealth(this, damageType, damage);
    }

    public void KilledEntity(Entity killedEntity)
    {
        if (varusP)
        {
            varusP.KilledEntity(killedEntity);
        }
    }
}
