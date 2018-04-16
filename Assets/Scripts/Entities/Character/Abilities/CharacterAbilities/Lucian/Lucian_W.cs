using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_W : DirectionTargetedProjectile, CharacterAbility
{
    private string explosionAreaOfEffectPrefabPath;
    private GameObject explosionAreaOfEffectPrefab;

    private float durationAoE;

    protected Lucian_W()
    {
        abilityName = "Ardent Blaze";

        abilityType = AbilityType.Skillshot;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
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
        startCooldownOnAbilityCast = true;

        durationAoE = 0.2f;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianW";

        projectilePrefabPath = "CharacterAbilities/Lucian/LucianW1";
        explosionAreaOfEffectPrefabPath = "CharacterAbilities/Lucian/LucianW2";
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

        AbilityDebuffs[0].OnAbilityBuffRemoved += RemoveDebuffFromEntityHit;
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        OnAreaOfEffectHit(projectile, entityHit);
        AbilityHit();
        OnProjectileReachedEnd((Projectile)projectile);
    }

    protected override void OnProjectileReachedEnd(Projectile projectile)
    {
        AreaOfEffect aoe = Instantiate(explosionAreaOfEffectPrefab, projectile.transform.position, projectile.transform.rotation).GetComponent<AreaOfEffect>();
        aoe.ActivateAreaOfEffect(projectile.UnitsAlreadyHit, character.Team, affectedUnitType, durationAoE, true);
        aoe.OnAbilityEffectHit += OnAreaOfEffectHit;
        Destroy(projectile.gameObject);
    }

    private void OnAreaOfEffectHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(GetAbilityDamage(entityHit));
        AddNewDebuffToEntityHit(entityHit);
        AbilityHit();
    }

    private void OnEntityDamaged()
    {
        AbilityBuffs[0].AddNewBuffToEntityHit(character);
    }

    private void AddNewDebuffToEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.Health.OnHealthReduced += OnEntityDamaged;
        AbilityDebuffs[0].AddNewDebuffToEntityHit(entityHit);
    }

    private void RemoveDebuffFromEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.Health.OnHealthReduced -= OnEntityDamaged;
    }
}
