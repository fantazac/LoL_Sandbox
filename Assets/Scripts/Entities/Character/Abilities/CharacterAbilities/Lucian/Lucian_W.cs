using System.Collections;
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

        startCooldownOnAbilityCast = true;

        durationAoE = 0.2f;

        HasCastTime = true;
    }

    protected override void SetAbilitySpritePath()
    {
        abilitySpritePath = "Sprites/CharacterAbilities/Lucian/LucianW";
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        OnAreaOfEffectHit(projectile, entityHit);
        OnProjectileReachedEnd((Projectile)projectile);
    }

    protected override void OnProjectileReachedEnd(Projectile projectile)
    {
        AreaOfEffect aoe = ((GameObject)Instantiate(explosionAreaOfEffectPrefab, projectile.transform.position, projectile.transform.rotation)).GetComponent<AreaOfEffect>();
        aoe.ActivateAreaOfEffect(projectile.UnitsAlreadyHit, character.Team, affectedUnitType, durationAoE, true);
        aoe.OnAbilityEffectHit += OnAreaOfEffectHit;
        Destroy(projectile.gameObject);
    }
}
