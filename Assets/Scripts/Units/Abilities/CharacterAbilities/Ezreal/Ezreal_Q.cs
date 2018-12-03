using UnityEngine;

public class Ezreal_Q : DirectionTargetedProjectile
{
    private float cooldownReductionOnProjectileHit;

    protected Ezreal_Q()
    {
        abilityName = "Mystic Shot";

        abilityType = AbilityType.SKILLSHOT;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.SINGLE_TARGET;
        damageType = DamageType.PHYSICAL;

        MaxLevel = 5;

        range = 1150;
        speed = 2000;
        damage = 15;// 15/40/65/90/115
        damagePerLevel = 25;
        totalADScaling = 1.1f;// 110%
        totalAPScaling = 0.3f;// 30%
        resourceCost = 28;// 28/31/34/37/40
        resourceCostPerLevel = 3;
        baseCooldown = 5.5f;// 5.5f/5.25f/5/4.75f/4.5f
        baseCooldownPerLevel = -0.25f;
        castTime = 0.25f;//TODO: VERIFY ACTUAL VALUE
        delayCastTime = new WaitForSeconds(castTime);

        cooldownReductionOnProjectileHit = 1.5f;

        AppliesOnHitEffects = true;

        affectedByCooldownReduction = true;
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Ezreal/EzrealQ";

        projectilePrefabPath = "CharacterAbilitiesPrefabs/Ezreal/EzrealQ";
    }

    protected override void OnProjectileHit(AbilityEffect projectile, Unit unitHit, bool isACriticalStrike, bool willMiss)
    {
        foreach (Ability ability in character.AbilityManager.CharacterAbilities)
        {
            ability.ReduceCooldown(cooldownReductionOnProjectileHit);
        }
        base.OnProjectileHit(projectile, unitHit, isACriticalStrike, willMiss);
    }
}
