using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varus_E : GroundTargetedAoE //TODO: Shoot invisible projectile and then spawn area on destination reached
{
    private Varus_W varusW;

    private string projectilePrefabPath;
    private GameObject projectilePrefab;

    private readonly int totalTicks;
    private readonly WaitForSeconds tickDelay;

    private float timeForProjectileToReachDestination;

    //private WaitForSeconds delayActivation;

    //private float radius;

    public Varus_E()
    {
        abilityName = "Hail of Arrows";

        abilityType = AbilityType.AREA_OF_EFFECT;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        damageType = DamageType.PHYSICAL;
        effectType = AbilityEffectType.AREA_OF_EFFECT;

        MaxLevel = 5;

        range = 925;
        damage = 70; // 70/105/140/175/210
        damagePerLevel = 35;
        bonusADScaling = 0.6f;
        resourceCost = 80;
        baseCooldown = 18; // 18/16/14/12/10
        baseCooldownPerLevel = -2f;
        castTime = 0.25f; //TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        totalTicks = 16;
        tickDelay = new WaitForSeconds(0.25f);

        timeForProjectileToReachDestination = 0.5f;
        
        //delayActivation = new WaitForSeconds(0.5f);

        //radius = 200;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusE";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Varus/VarusE";
        areaOfEffectPrefabPath = "CharacterAbilitiesPrefabs/Varus/VarusE2";
    }

    protected override void LoadPrefabs()
    {
        base.LoadPrefabs();

        projectilePrefab = Resources.Load<GameObject>(projectilePrefabPath);
    }

    /*protected override void ModifyValues()
    {
        radius *= StaticObjects.MultiplyingFactor;

        base.ModifyValues();
    }*/

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected override void Start()
    {
        base.Start();

        varusW = GetComponent<Varus_W>();

        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Varus_E_Debuff>() };
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        IsBeingCasted = true;

        yield return delayCastTime;

        IsBeingCasted = false;
        UseResource();
        champion.OrientationManager.RotateCharacterInstantly(destinationOnCast);

        float projectileRange = Vector3.Distance(destinationOnCast, transform.position);
        SpawnProjectile(transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.5f), transform.rotation,
            projectileRange > range ? range : projectileRange);

        FinishAbilityCast();
    }

    private void SpawnProjectile(Vector3 position, Quaternion rotation, float range)
    {
        Projectile projectile = Instantiate(projectilePrefab, position, rotation).GetComponent<Projectile>();
        projectile.ShootProjectile(affectedTeams, affectedUnitTypes, range / timeForProjectileToReachDestination, range);
        projectile.OnProjectileReachedEnd += OnProjectileReachedEnd;
    }

    private void OnProjectileReachedEnd(Projectile projectile)
    {
        AreaOfEffectWithEffectOverTime areaOfEffect =
            Instantiate(areaOfEffectPrefab, Vector3.right * projectile.transform.position.x + Vector3.forward * projectile.transform.position.z, Quaternion.identity)
                .GetComponent<AreaOfEffectWithEffectOverTime>();
        areaOfEffect.CreateAreaOfEffect(affectedTeams, affectedUnitTypes, tickDelay, totalTicks);
        areaOfEffect.OnAreaOfEffectHit += OnAreaOfEffectHit;
        areaOfEffect.OnAreaOfEffectWithEffectOverTimeHit += OnAreaOfEffectWithEffectOverTimeHit;
        areaOfEffect.ActivateAreaOfEffect();
        
        Destroy(projectile.gameObject);
    }

    private void OnAreaOfEffectHit(AreaOfEffect areaOfEffect, Unit unitHit)
    {
        float abilityDamage = GetAbilityDamage(unitHit);
        DamageUnit(unitHit, abilityDamage);
        if (varusW)
        {
            varusW.ProcStacks(unitHit, this);
        }

        AbilityHit(unitHit, abilityDamage);
    }

    private void OnAreaOfEffectWithEffectOverTimeHit(AreaOfEffect areaOfEffect, List<Unit> previouslyAffectedUnits, List<Unit> affectedUnits)
    {
        AbilityDebuffs[0].AddNewBuffToAffectedUnits(previouslyAffectedUnits, affectedUnits);
    }
}
