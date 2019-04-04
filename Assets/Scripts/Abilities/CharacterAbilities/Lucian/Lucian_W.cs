using System;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_W : DirectionTargetedProjectile
{
    private string explosionAreaOfEffectPrefabPath;
    private GameObject explosionAreaOfEffectPrefab;

    private readonly float durationAoE;

    protected Lucian_W()
    {
        abilityName = "Ardent Blaze";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        MaxLevel = 5;

        range = 900;
        speed = 1550;
        damage = 85; // 85/125/165/205/245
        damagePerLevel = 40;
        totalAPScaling = 0.9f; // 90%
        resourceCost = 50; // 50
        baseCooldown = 14; // 14/13/12/11/10
        baseCooldownPerLevel = -1;
        castTime = 0.25f;
        delayCastTime = new WaitForSeconds(castTime);

        affectedByCooldownReduction = true;

        durationAoE = 0.2f;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianW";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Lucian/LucianW1";
        explosionAreaOfEffectPrefabPath = "CharacterAbilitiesPrefabs/Lucian/LucianW2";
    }

    protected override void LoadPrefabs()
    {
        base.LoadPrefabs();

        explosionAreaOfEffectPrefab = Resources.Load<GameObject>(explosionAreaOfEffectPrefabPath);
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Lucian_W_Buff>() };
        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Lucian_W_Debuff>() };

        AbilityDebuffs[0].OnAbilityBuffRemoved += RemoveDebuffFromAffectedUnit;
    }

    protected override void OnProjectileHit(Projectile projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        OnProjectileReachedEnd(projectile);
    }

    protected override void OnProjectileReachedEnd(Projectile projectile)
    {
        AreaOfEffect aoe = Instantiate(explosionAreaOfEffectPrefab, projectile.transform.position, projectile.transform.rotation).GetComponent<AreaOfEffect>();
        aoe.CreateAreaOfEffect(affectedTeams, affectedUnitTypes, durationAoE);
        aoe.OnAreaOfEffectHit += OnAreaOfEffectHit;
        aoe.ActivateAreaOfEffect();

        Destroy(projectile.gameObject);
    }

    private void OnAreaOfEffectHit(AreaOfEffect areaOfEffect, Unit unitHit)
    {
        float abilityDamage = GetAbilityDamage(unitHit);
        DamageUnit(unitHit, abilityDamage);
        AddNewDebuffToAffectedUnit(unitHit);
        AbilityHit(unitHit, abilityDamage);
    }

    private void OnUnitDamaged()
    {
        AbilityBuffs[0].AddNewBuffToAffectedUnit(champion);
    }

    private void AddNewDebuffToAffectedUnit(Unit affectedUnit)
    {
        affectedUnit.StatsManager.Health.OnResourceReduced += OnUnitDamaged;
        AbilityDebuffs[0].AddNewBuffToAffectedUnit(affectedUnit);
    }

    private void RemoveDebuffFromAffectedUnit(Unit affectedUnit)
    {
        affectedUnit.StatsManager.Health.OnResourceReduced -= OnUnitDamaged;
    }
}
