using System;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_W : DirectionTargetedProjectile
{
    private string explosionAreaOfEffectPrefabPath;
    private GameObject explosionAreaOfEffectPrefab;

    private float durationAoE;

    protected Lucian_W()
    {
        abilityName = "Ardent Blaze";

        abilityType = AbilityType.SKILLSHOT;
        affectedTeams = AffectedTeams.GetEnemyTeams(champion.Team);
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        MaxLevel = 5;

        range = 900;
        speed = 1550;
        damage = 85;// 85/125/165/205/245
        damagePerLevel = 40;
        totalAPScaling = 0.9f;// 90%
        resourceCost = 50;// 50
        baseCooldown = 14;// 14/13/12/11/10
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

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<Lucian_W_Buff>() };
        AbilityDebuffs = new AbilityBuff[] { gameObject.AddComponent<Lucian_W_Debuff>() };

        AbilityDebuffs[0].OnAbilityBuffRemoved += RemoveDebuffFromAffectedUnit;
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        OnAreaOfEffectHit(projectile, unitHit, isACriticalStrike, willMiss);
        OnProjectileReachedEnd((Projectile)projectile);
    }

    protected override void OnProjectileReachedEnd(Projectile projectile)
    {
        AreaOfEffect aoe = Instantiate(explosionAreaOfEffectPrefab, projectile.transform.position, projectile.transform.rotation).GetComponent<AreaOfEffect>();
        aoe.CreateAreaOfEffect(projectile.UnitsAlreadyHit, affectedTeams, affectedUnitTypes, durationAoE, true);
        aoe.ActivateAreaOfEffect();
        aoe.OnAbilityEffectHit += OnAreaOfEffectHit;
        Destroy(projectile.gameObject);
    }

    private void OnAreaOfEffectHit(AbilityEffect projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        float damage = GetAbilityDamage(unitHit);
        DamageUnit(unitHit, damage);
        AddNewDebuffToAffectedUnit(unitHit);
        AbilityHit(unitHit, damage);
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
