﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_W : DirectionTargetedProjectile, CharacterAbility
{
    [SerializeField]
    private GameObject explosionAreaOfEffectPrefab;

    private float durationAoE;

    protected Lucian_W()
    {
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.MAGIC;

        range = 900;
        speed = 1550;
        damage = 60;
        cooldown = 9;
        castTime = 0.2f;
        delayCastTime = new WaitForSeconds(castTime);

        buffDuration = 1;
        buffFlatBonus = 60;
        debuffDuration = 4;

        startCooldownOnAbilityCast = true;

        durationAoE = 0.2f;

        HasCastTime = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Lucian/LucianW";
        buffSpritePath = "Sprites/CharacterAbilities/Lucian/LucianW_Buff";
        debuffSpritePath = "Sprites/CharacterAbilities/Lucian/LucianW_Debuff";
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        OnAreaOfEffectHit(projectile, entityHit);
        AbilityHit();
        OnProjectileReachedEnd((Projectile)projectile);
    }

    protected override void OnProjectileReachedEnd(Projectile projectile)
    {
        AreaOfEffect aoe = ((GameObject)Instantiate(explosionAreaOfEffectPrefab, projectile.transform.position, projectile.transform.rotation)).GetComponent<AreaOfEffect>();
        aoe.ActivateAreaOfEffect(projectile.UnitsAlreadyHit, character.Team, affectedUnitType, durationAoE, true);
        aoe.OnAbilityEffectHit += OnAreaOfEffectHit;
        Destroy(projectile.gameObject);
    }

    private void OnAreaOfEffectHit(AbilityEffect projectile, Entity entityHit)
    {
        entityHit.EntityStats.Health.Reduce(damage);
        AddNewDebuffToEntityHit(entityHit);
        AbilityHit();
    }

    private void OnEntityDamaged()
    {
        AddNewBuffToEntityHit(character);
    }

    public override void ApplyBuffToEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.MovementSpeed.AddFlatBonus(buffFlatBonus);
    }

    public override void RemoveBuffFromEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.MovementSpeed.RemoveFlatBonus(buffFlatBonus);
    }

    public override void ApplyDebuffToEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.Health.OnHealthReduced += OnEntityDamaged;
    }

    public override void RemoveDebuffFromEntityHit(Entity entityHit)
    {
        entityHit.EntityStats.Health.OnHealthReduced -= OnEntityDamaged;
    }
}
