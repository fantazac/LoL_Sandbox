using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_Q : DirectionTargetedProjectile, CharacterAbility
{
    private List<Ability> characterAbilitiesThatGetReducedCooldownOnProjectileHit;
    private float cooldownReductionOnProjectileHit;

    protected Ezreal_Q()
    {
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;

        range = 1150;
        speed = 2000;
        damage = 100;
        cooldown = 4;
        castTime = 0.25f;
        delayCastTime = new WaitForSeconds(castTime);

        cooldownReductionOnProjectileHit = 1.5f;

        startCooldownOnAbilityCast = true;

        HasCastTime = true;
    }

    protected override void Start()
    {
        base.Start();

        characterAbilitiesThatGetReducedCooldownOnProjectileHit = new List<Ability>();
        foreach (Ability ability in GetComponents<CharacterAbility>())
        {
            if (!(ability is PassiveCharacterAbility))
            {
                characterAbilitiesThatGetReducedCooldownOnProjectileHit.Add(ability);
            }
        }
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Entity entityHit)
    {
        foreach (Ability ability in characterAbilitiesThatGetReducedCooldownOnProjectileHit)
        {
            ability.ReduceCooldown(cooldownReductionOnProjectileHit);
        }
        base.OnProjectileHit(projectile, entityHit);
    }
}
