using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_Q : DirectionTargetedProjectile, CharacterAbility
{
    private List<Ability> characterAbilitiesThatGetReducedCooldownOnProjectileHit;
    private float cooldownReductionOnProjectileHit;

    protected Ezreal_Q()
    {
        abilityName = "Mystic Shot";

        abilityType = AbilityType.Skillshot;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 5;

        range = 1150;
        speed = 2000;
        damage = 15;// 15/40/65/90/115
        damagePerLevel = 25;
        totalADScaling = 1.1f;// 110%
        totalAPScaling = 0.4f;// 40%
        resourceCost = 28;// 28/31/34/37/40
        resourceCostPerLevel = 3;
        cooldown = 5.5f;// 5.5f/5.25f/5/4.75f/4.5f
        cooldownPerLevel = 0.25f;
        castTime = 0.25f;//TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        cooldownReductionOnProjectileHit = 1.5f;

        startCooldownOnAbilityCast = true;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealQ";
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
