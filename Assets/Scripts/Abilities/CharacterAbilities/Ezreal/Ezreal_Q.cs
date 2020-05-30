using System;
using System.Collections.Generic;
using UnityEngine;

public class Ezreal_Q : DirectionTargetedProjectile
{
    private readonly float cooldownReductionOnProjectileHit;

    protected Ezreal_Q()
    {
        abilityName = "Mystic Shot";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitTypes = new List<Type>() { typeof(Unit) };
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 5;

        range = 1150;
        speed = 2000;
        damage = 20; // 20/45/70/95/120
        damagePerLevel = 25;
        totalADScaling = 1.2f; // 120%
        totalAPScaling = 0.15f; // 15%
        resourceCost = 28; // 28/31/34/37/40
        resourceCostPerLevel = 3;
        baseCooldown = 5.5f; // 5.5f/5.25f/5/4.75f/4.5f
        baseCooldownPerLevel = -0.25f;
        castTime = 0.25f; //TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        cooldownReductionOnProjectileHit = 1.5f;

        appliesOnHitEffects = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealQ";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Ezreal/EzrealQ";
    }

    public override void SetAffectedTeams(Team allyTeam)
    {
        affectedTeams = TeamMethods.GetHostileTeams(allyTeam);
    }

    protected override void OnProjectileHit(Projectile projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        foreach (Ability ability in champion.AbilityManager.CharacterAbilities)
        {
            ability.ReduceCooldown(cooldownReductionOnProjectileHit);
        }

        base.OnProjectileHit(projectile, unitHit, isACriticalStrike, willMiss);
    }
}
